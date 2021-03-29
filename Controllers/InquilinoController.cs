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
            /*Inquilino in = new Inquilino
            {
                Dni = int.Parse(collection["dni"]),
                Nombre = collection["nombre"],
                Mail = collection["mail"],
                Direccion = collection["direccion"]
                
            };
            repositorioInquilino.Alta(in);*/
            return RedirectToAction("Index");
        }

        // 
        public IActionResult Editar(int id)
        {
            Inquilino i = repositorioInquilino.Buscar(id); 
            return View(i);
        }

        // 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(Inquilino i)
        {
            RepositorioInquilino riEdit = new RepositorioInquilino();
            riEdit.Editar(i);
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
