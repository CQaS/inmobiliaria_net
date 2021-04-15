﻿using AplicacionPrueba.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Security.Claims;

namespace AplicacionPrueba.Controllers
{
    public class PagoController : Controller
    {
        private readonly ILogger<PagoController> _logger;

        private readonly RepositorioPago repositorioPago;
        private readonly RepositorioContrato repositorioContrato;

        public PagoController(ILogger<PagoController> logger)
        {
            repositorioPago = new RepositorioPago();
            repositorioContrato = new RepositorioContrato();
            _logger = logger;
        }

        // GET: 
        [Authorize]
        public IActionResult Index()
        {
            var lta = repositorioPago.obtener();
            ViewData[nameof(Pago)] = lta;
            return View();
        }

        [Authorize]
        public ActionResult PorContrato(int id)
        {
            var lista = repositorioPago.BuscarPorContrato(id);            
            ViewData[nameof(Pago)] = lista;
            return View();
        }

        // GET: 
        [Authorize]
        public IActionResult Alta()
        {
            var lta = repositorioContrato.obtener();
            ViewData[nameof(Contrato)] = lta;
            return View();
        }

        // POST: 
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Alta(Pago i)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    repositorioPago.Alta(i);
                    return RedirectToAction("Index");
                }
                else
                {
                    var lta = repositorioContrato.obtener();
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
        [Authorize]
        public IActionResult Editar(int id)
        {
            Pago i = repositorioPago.Buscar(id);
            return View(i);
        }


        // 
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Editar(Pago i)
        {
            try
            {
            RepositorioPago pEdit = new RepositorioPago();
            pEdit.Editar(i);
            return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(i);
            }
        }

        // 
        [Authorize(Policy = "Administrador")]
        public IActionResult Delete(int id)
        {
            repositorioPago.Borrar(id);
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
