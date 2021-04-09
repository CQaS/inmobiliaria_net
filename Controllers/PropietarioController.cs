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
    public class PropietarioController : Controller
    {
        private readonly ILogger<PropietarioController> _logger;
        private readonly RepositorioPropietario repositorioPropietario;
        public PropietarioController(ILogger<PropietarioController> logger)
        {
            repositorioPropietario = new RepositorioPropietario();
            _logger = logger;
        }

        // GET: 
        public IActionResult Index()
        {
            var lta = repositorioPropietario.obtener();
            ViewData[nameof(Propietario)] = lta;
            ViewBag.Persona = lta;
            return View();
        }

        // GET: 
        public IActionResult Alta()
        {
            return View();
        }

        // POST: 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Alta(Propietario p)
        {
            try
            {
                RepositorioPropietario alta = new RepositorioPropietario();
                alta.Alta(p);
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
        public IActionResult Editar(int id)
        { 
            Propietario p = repositorioPropietario.Buscar(id);            
            return View(p);
        }

        // 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(Propietario p)
        {
            try
            {
            RepositorioPropietario riEdit = new RepositorioPropietario();
            riEdit.Editar(p);
            return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(p);
            }
        }

        public IActionResult Detalles(int id)
        { 
            Propietario p = repositorioPropietario.Buscar(id);            
            return View(p);
        }

        // GET: /Delete/5
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
