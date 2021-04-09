using AplicacionPrueba.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacionPrueba.Controllers
{
    public class InmuebleController : Controller
    {
        private readonly ILogger<InmuebleController> _logger;

        private readonly RepositorioInmueble repositorioInmueble;
        private readonly RepositorioPropietario repositorioPropietario;

        public InmuebleController(ILogger<InmuebleController> logger)
        {
            repositorioInmueble = new RepositorioInmueble();
            repositorioPropietario = new RepositorioPropietario();
            _logger = logger;
        }

        // GET: 
        public IActionResult Index()
        {
            var lta = repositorioInmueble.obtener();
            ViewData[nameof(Inmueble)] = lta;
            ViewBag.Persona = lta;
            return View();
        }

        // GET: 
        public IActionResult Alta()
        {
            var lta = repositorioPropietario.obtener();
            ViewData[nameof(Propietario)] = lta;
            ViewBag.Persona = lta;
            return View();
        }

        // POST: 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Alta(Inmueble i)
        {
            try
            {
            RepositorioInmueble alta = new RepositorioInmueble();
            alta.Alta(i);
            return RedirectToAction("Index");
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
