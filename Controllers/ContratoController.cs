using AplicacionPrueba.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacionPrueba.Controllers
{
    public class ContratoController : Controller
    {
        private readonly ILogger<ContratoController> _logger;

        private readonly RepositorioContrato repositorioContrato;
        private readonly RepositorioPropietario repositorioPropietario;
        private readonly RepositorioInquilino repositorioInquilino;
        private readonly RepositorioInmueble repositorioInmueble;

        public ContratoController(ILogger<ContratoController> logger, IConfiguration config)
        {
            this.repositorioContrato = new RepositorioContrato(config);
            this.repositorioPropietario = new RepositorioPropietario(config);
            this.repositorioInquilino = new RepositorioInquilino(config);
            this.repositorioInmueble = new RepositorioInmueble(config);
            _logger = logger;
        }

        // GET:
        [Authorize]
        public IActionResult Index()
        {
            var lta = repositorioContrato.obtener();
            ViewData[nameof(Contrato)] = lta;
            ViewBag.multa = TempData["multa"];
            return View();
        }

        /* GET:
        [Authorize] 
        public IActionResult Contratar(int id)
        {
            Inmueble i = repositorioInmueble.Buscar(id);
            ViewData["unInmueble"] = i;
            var lta = repositorioInmueble.obtener();
            ViewData[nameof(Inmueble)] = lta;
            var lta2 = repositorioInquilino.obtener();
            ViewData[nameof(Inquilino)] = lta2;
            return View(nameof(Alta));
        }*/

        // GET:
        [Authorize] 
        public IActionResult Alta()
        {
            var lta = repositorioInmueble.obtener();
            ViewData[nameof(Inmueble)] = lta;
            var lta2 = repositorioInquilino.obtener();
            ViewData[nameof(Inquilino)] = lta2;
            return View();
        }

        // POST: 
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Alta(Contrato i)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    repositorioContrato.Alta(i);
                    return RedirectToAction("Index");
                }
                else
                {
                    var lta = repositorioPropietario.obtener();
                    ViewData[nameof(Propietario)] = lta;
                    return View(i);
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
        [Authorize]
        public IActionResult Editar(int id)
        {
            Contrato i = repositorioContrato.Buscar(id);
            var lta = repositorioInmueble.obtener();
            ViewData[nameof(Inmueble)] = lta;
            var lta2 = repositorioInquilino.obtener();
            ViewData[nameof(Inquilino)] = lta2;
            return View(i);
        }


        // 
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Editar(Contrato i)
        {
            try
            {
            repositorioContrato.Editar(i);
            return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(i);
            }
        }

        [Authorize]
        public IActionResult Detalles(int id)
        {
            Contrato i = repositorioContrato.Buscar(id);
            var lta = repositorioInmueble.Buscar(i.id_inmueble);
            ViewData[nameof(Inmueble)] = lta;
            var lta2 = repositorioInquilino.Buscar(i.id_inquilino);
            ViewData[nameof(Inquilino)] = lta2; 
            return View(i);
        }

        // 
        [Authorize(Policy = "Administrador")]
        public IActionResult Delete(int id)
        {
            var multa = repositorioContrato.Borrar(id);
            TempData["multa"] = multa;
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
