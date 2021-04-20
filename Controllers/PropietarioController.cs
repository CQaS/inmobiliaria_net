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
    public class PropietarioController : Controller
    {
        private readonly ILogger<PropietarioController> _logger;
        private readonly RepositorioPropietario repositorioPropietario;
        private readonly RepositorioInmueble repoInmu;
        public PropietarioController(ILogger<PropietarioController> logger, IConfiguration config)
        {
            this.repositorioPropietario = new RepositorioPropietario(config);
            this.repoInmu = new RepositorioInmueble(config);
            _logger = logger;
        }

        // GET: 
        [Authorize]
        public IActionResult Index()
        {
            var lta = repositorioPropietario.obtener();
            ViewData[nameof(Propietario)] = lta;
            return View();
        }

        // GET: 
        [Authorize]
        public IActionResult Alta()
        {
            return View();
        }

        // POST: 
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Alta(Propietario p)
        {
            try
            {
                repositorioPropietario.Alta(p);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrace = ex.StackTrace;
                return RedirectToAction("Index");
            }
        }

        // 
        [Authorize]
        public IActionResult Editar(int id)
        { 
            Propietario p = repositorioPropietario.Buscar(id);            
            return View(p);
        }

        // 
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Editar(Propietario p)
        {
            try
            {
            repositorioPropietario.Editar(p);
            return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(p);
            }
        }

        [Authorize]
        public IActionResult Detalles(int id)
        { 
            var lta = repoInmu.obtenerPorPropietario(id);
            ViewData[nameof(Inmueble)] = lta;
            Propietario p = repositorioPropietario.Buscar(id);            
            return View(p);
        }

        // GET
        [Authorize(Policy = "Administrador")]
        public IActionResult Delete(int id)
        {
            repositorioPropietario.Borrar(id);
            return RedirectToAction("Index");
        }

        // POST: /Delete/5
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
