using AplicacionPrueba.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace AplicacionPrueba.Controllers
{
    public class ContratoController : Controller
    {
        private readonly ILogger<ContratoController> _logger;
        private readonly RepositorioContrato repositorioContrato;
        public ContratoController(ILogger<ContratoController> logger)
        {
            repositorioContrato = new RepositorioContrato();
            _logger = logger;
        }

        // GET: 
        public IActionResult Index()
        {
            try{
            var lta = repositorioContrato.obtener();
            ViewData[nameof(Contrato)] = lta;
            ViewData["Error"] = TempData["Error"];
            return View();
            }
            catch(MySqlException ex)
            {
                TempData["Error"] = ex.Message.ToString();
                return RedirectToAction(nameof(Index));
            }
            catch(Exception)
            {
                TempData["Error"] = "ALERT: Ocurio un Error!";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
