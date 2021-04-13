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
    public class ContratoController : Controller
    {
        private readonly ILogger<ContratoController> _logger;

        private readonly RepositorioContrato repositorioContrato;
        private readonly RepositorioPropietario repositorioPropietario;
        private readonly RepositorioInquilino repositorioInquilino;
        private readonly RepositorioInmueble repositorioInmueble;

        public ContratoController(ILogger<ContratoController> logger)
        {
            repositorioContrato = new RepositorioContrato();
            repositorioPropietario = new RepositorioPropietario();
            repositorioInquilino = new RepositorioInquilino();
            repositorioInmueble = new RepositorioInmueble();
            _logger = logger;
        }

        // GET: 
        public IActionResult Index()
        {
            var lta = repositorioContrato.obtener();
            ViewData[nameof(Contrato)] = lta;
            return View();
        }

        // GET: 
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
        public IActionResult Alta(Contrato i)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    RepositorioContrato alta = new RepositorioContrato();
                    alta.Alta(i);
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
        public IActionResult Editar(Contrato i)
        {
            try
            {
            RepositorioContrato riEdit = new RepositorioContrato();
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
            Contrato i = repositorioContrato.Buscar(id);
            var lta = repositorioInmueble.Buscar(id);
            ViewData[nameof(Inmueble)] = lta;
            var lta2 = repositorioInquilino.Buscar(id);
            ViewData[nameof(Inquilino)] = lta2; 
            return View(i);
        }

        // 
        public IActionResult Delete(int id)
        {
            repositorioContrato.Borrar(id);
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
