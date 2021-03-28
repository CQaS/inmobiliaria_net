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
    public class InquilinoController : Controller
    {
        private readonly ILogger<InquilinoController> _logger;
        private readonly RepositorioInquilino repositorioInquilino;
        public InquilinoController(ILogger<InquilinoController> logger)
        {
            repositorioInquilino = new RepositorioInquilino();
            _logger = logger;
        }

        // GET: PersonaController1
        public IActionResult Index()
        {
            var lta = repositorioInquilino.obtener();
            ViewData[nameof(Inquilino)] = lta;
            ViewBag.Persona = lta;
            return View();
        }

        // GET: PersonaController1/Details/5
        public IActionResult Details(int id)
        {
            //var p = new Inquilino { Id = id, Nombre = "Victoria" };
            return View();
        }

        // GET: PersonaController1/Alta
        public IActionResult Alta()
        {
            return View();
        }

        // POST: PersonaController1/Alta
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Alta(IFormCollection collection)
        {
            Inquilino in = new Inquilino
            {
                Dni = int.Parse(collection["dni"]),
                Nombre = collection["nombre"],
                Mail = collection["mail"],
                Direccion = collection["direccion"]
                
            };
            repositorioInquilino.Alta(in);
            return RedirectToAction("Index");
        }

        // GET: PersonaController1/Edit/5
        public IActionResult Editar(int id)
        {
            //RepositorioInquilino ri = new RepositorioInquilino();
            Inquilino in = repositorioInquilino.Buscar(id);
            return View(in);
        }

        // POST: PersonaController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(IFormCollection collection)
        {
            Inquilino in = new Inquilino
            {
                Id = int.Parse(collection["id"].ToString()),
                Dni = int.Parse(collection["dni"].ToString()),
                Nombre = collection["nombre"].ToString(),
                Mail = collection["mail"].ToString(),
                Direccion = collection["nombre"].ToString()
            };
            repositorioInquilino.Editar(in);
            return RedirectToAction("Index");
        }

        // GET: PersonaController1/Delete/5
        public IActionResult Delete(int id)
        {
            repositorioInquilino.Borrar(id);
            return RedirectToAction("Index");
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
