using AplicacionPrueba.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Security.Claims;

namespace AplicacionPrueba.Controllers
{
    public class InmuebleController : Controller
    {
        private readonly ILogger<InmuebleController> _logger;

        private readonly RepositorioInmueble repositorioInmueble;
        private readonly RepositorioPropietario repositorioPropietario;
        private readonly RepositorioContrato repoContrato;
        private readonly IWebHostEnvironment environment;

        public InmuebleController(ILogger<InmuebleController> logger, IWebHostEnvironment environment)
        {
            this.environment = environment;
            repositorioInmueble = new RepositorioInmueble();
            repositorioPropietario = new RepositorioPropietario();
            repoContrato = new RepositorioContrato();
            _logger = logger;
        }

        // GET: 
        public IActionResult Index()
        {
            var lta = repositorioInmueble.obtener();
            ViewData[nameof(Inmueble)] = lta;
            return View();
        }

        // GET: 
        public IActionResult Alta()
        {
            var lta = repositorioPropietario.obtener();
            ViewData[nameof(Propietario)] = lta;
            return View();
        }

        // POST: 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Alta(Inmueble i)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    if(i.FotoFile !=null)
                        {
                            string wwwPath = environment.WebRootPath;
                            string path = Path.Combine(wwwPath, "img/fotos"+i.id_propietario);
                            if(!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }
                            string fileName = "Inmueble_" + i.id_propietario + Path.GetExtension(i.FotoFile.FileName);
                            string pathCompleto = Path.Combine("img/fotos"+i.id_propietario, fileName);
                            i.foto = Path.Combine(path, fileName);
                            using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                            {
                                i.FotoFile.CopyTo(stream);
                            }

                            repositorioInmueble.Alta(i);                           
                             
                        }
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Error = "Algo fallo en el Alta del Inmueble, intenta nuevamente!";
                    var lta = repositorioPropietario.obtener();
                    ViewData[nameof(Propietario)] = lta;
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrace = ex.StackTrace;
                return RedirectToAction("Index");
            }
        }

        // GET
        public IActionResult Editar(int id)
        {
            Inmueble i = repositorioInmueble.Buscar(id);
            var lta = repositorioPropietario.obtener();
            ViewData[nameof(Propietario)] = lta;
            return View(i);
        }


        // 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(Inmueble i)
        {
            try
            {
            RepositorioInmueble riEdit = new RepositorioInmueble();
            riEdit.Editar(i);
            return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(i);
            }
        }

        public IActionResult Detalles(int id)
        {
            Inmueble i = repositorioInmueble.Buscar(id); 
            return View(i);
        }

        public IActionResult VerContratos(int id)
        {
            var lta = repoContrato.VerContratosXInmueble(id);
            ViewData[nameof(Contrato)] = lta; 
            return View();
        }

        // 
        public IActionResult Delete(int id)
        {
            repositorioInmueble.Borrar(id);
            return RedirectToAction("Index");
        }

        // POST: 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
