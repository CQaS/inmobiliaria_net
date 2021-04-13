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

        // GET: 
        public IActionResult Index()
        {
            var lta = repositorioInquilino.obtener();
            ViewData[nameof(Inquilino)] = lta;
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
        public IActionResult Alta(Inquilino i)
        {
            try
            {
            RepositorioInquilino alta = new RepositorioInquilino();
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
            Inquilino i = repositorioInquilino.Buscar(id); 
            return View(i);
        }


        // 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(Inquilino i)
        {
            try
            {
            RepositorioInquilino riEdit = new RepositorioInquilino();
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
            Inquilino i = repositorioInquilino.Buscar(id); 
            return View(i);
        }

        // 
        public IActionResult Delete(int id)
        {
            repositorioInquilino.Borrar(id);
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
