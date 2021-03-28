﻿using AplicacionPrueba.Models;
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
            var lta = repositorioPropietario.obtener();
            ViewData[nameof(Propietario)] = lta;
            ViewBag.Persona = lta;
            return View();
        }

        // GET: PersonaController1/Details/5
        public IActionResult Details(int id)
        {
            //var p = new Propietario { Id = id, Nombre = "Victoria" };
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
            Propietario in = new Propietario
            {
                Dni = int.Parse(collection["dni"]),
                Nombre = collection["nombre"],
                Direccion = collection["direccion"]
                
            };
            repositorioPropietario.Alta(in);
            return RedirectToAction("Index");
        }

        // GET: PersonaController1/Edit/5
        public IActionResult Editar(int id)
        {
            //RepositorioPropietario ri = new RepositorioPropietario();
            Propietario in = repositorioPropietario.Buscar(id);
            return View(in);
        }

        // POST: PersonaController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(IFormCollection collection)
        {
            Propietario in = new Propietario
            {
                Id = int.Parse(collection["id"].ToString()),
                Dni = int.Parse(collection["dni"].ToString()),
                Nombre = collection["nombre"].ToString(),
                Direccion = collection["nombre"].ToString()
            };
            repositorioPropietario.Editar(in);
            return RedirectToAction("Index");
        }

        // GET: PersonaController1/Delete/5
        public IActionResult Delete(int id)
        {
            repositorioPropietario.Borrar(id);
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
