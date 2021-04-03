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
        public IActionResult Details(int id)
        {
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
            RepositorioPropietario alta = new RepositorioPropietario();
            alta.Alta(p);
            return RedirectToAction("Index");
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
            RepositorioPropietario riEdit = new RepositorioPropietario();
            riEdit.Editar(p);
            return RedirectToAction("Index");
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
