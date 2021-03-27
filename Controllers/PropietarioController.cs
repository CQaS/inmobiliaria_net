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

        // GET: PersonaController1
        public IActionResult Index()
        {
            /*IList<Persona> lista = new List<Persona>();
            lista.Add(new Persona { Id = 1, Nombre = "Victoria" });
            lista.Add(new Persona { Id = 2, Nombre = "Samuel" });
            lista.Add(new Persona { Id = 3, Nombre = "Damian" });
            lista.Add(new Persona { Id = 4, Nombre = "Janett" });*/
            var lta = repositorioPropietario.obtener();
            ViewData[nameof(Propietario)] = lta;
            ViewBag.Persona = lta;
            return View();
        }

        // GET: PersonaController1/Details/5
        public IActionResult Details(int id)
        {
            var p = new Propietario { Id = id, Nombre = "Victoria" };
            return View();
        }

        // GET: PersonaController1/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PersonaController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IFormCollection collection)
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

        // GET: PersonaController1/Edit/5
        public IActionResult Edit(int id)
        {
            return View();
        }

        // POST: PersonaController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, IFormCollection collection)
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

        // GET: PersonaController1/Delete/5
        public IActionResult Delete(int id)
        {
            return View();
        }

        // POST: PersonaController1/Delete/5
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
