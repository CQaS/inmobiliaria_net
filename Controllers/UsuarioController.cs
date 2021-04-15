using AplicacionPrueba.Models;
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
    public class UsuarioController : Controller
    {
        private readonly ILogger<UsuarioController> _logger;

        private readonly RepositorioUsuario repositorioUsuario;
        private readonly RepositorioPropietario repositorioPropietario;
        private readonly RepositorioInquilino repositorioInquilino;
        private readonly RepositorioInmueble repositorioInmueble;
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment environment;

        public UsuarioController(ILogger<UsuarioController> logger, IConfiguration configuration, IWebHostEnvironment environment)
        {
            this.configuration = configuration;
            this.environment = environment;
            repositorioUsuario = new RepositorioUsuario();
            repositorioPropietario = new RepositorioPropietario();
            repositorioInquilino = new RepositorioInquilino();
            repositorioInmueble = new RepositorioInmueble();
            _logger = logger;
        }

        // GET: Usuario
        [Authorize(Policy = "Administrador")]
        public ActionResult Index()
        {
            var usuarios = repositorioUsuario.ObtenerTodos();
            ViewData[nameof(Usuario)] = usuarios;
            ViewBag.Id = TempData["Id"];
            if (TempData.ContainsKey("Id"))
                ViewBag.Id = TempData["Id"];
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            return View();
        }

        // GET: Usuario/Details/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Detailles(int id)
        {
            var e = repositorioUsuario.ObtenerPorId(id);
            return View(e);
        }

        // GET: Usuario/Create
        [Authorize(Policy = "Administrador")]
        public ActionResult Alta()
        {
            ViewBag.Roles = Usuario.ObtenerRoles();
            return View();
        }

        // POST: Usuario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Alta(Usuario u)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.Roles = Usuario.ObtenerRoles();
                    if(u.Id == 0)
                       TempData["Mensaje"] = "Debe ingresar todo los datos del usuario!";
                       ViewBag.Error= TempData["Mensaje"];
                    return View();
                }
                {
                    var user = repositorioUsuario.ObtenerPorEmail(u.Mail);
                    //var inqui = repositorioInquilino.ObtenerPorEmail(u.Email);
                    //var prop = repositorioPropietario.ObtenerPorEmail(u.Email);

                    if (user == null )
                    {

                        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                password: u.Clave,
                                salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                                prf: KeyDerivationPrf.HMACSHA1,
                                iterationCount: 1000,
                                numBytesRequested: 256 / 8));
                        u.Clave = hashed;

                        u.Rol = User.IsInRole("Administrador") ? u.Rol : (int)enRoles.Empleado;
                        var nbreRnd = Guid.NewGuid();//posible nombre aleatorio
                        int res = repositorioUsuario.Alta(u);

                        if(u.AvatarFile !=null  && res > 0)
                        {
                            u.Id = res;
                            string wwwPath = environment.WebRootPath;
                            string path = Path.Combine(wwwPath, "img/avatars");
                            if(!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }
                            //Path.GetFileName(u.AvatarFile.FileName);//este nombre se puede repetir
                            string fileName = "avatar_" + u.Mail + Path.GetExtension(u.AvatarFile.FileName);
                            string pathCompleto = Path.Combine(path, fileName);
                            u.Avatar = Path.Combine("/img/avatars", fileName);
                            using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                            {
                                u.AvatarFile.CopyTo(stream);
                            }
                            
                            repositorioUsuario.Modificacion(u);
                        }
                        TempData["Id"] = u.Id;
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["Mensaje"] = "El Email o Usuario ingresado ya se encuentra registrado en el sistema!";
                        ViewBag.Error = TempData["Mensaje"];
                        ViewBag.Roles = Usuario.ObtenerRoles();
                        return View();
                    }                      

                }                
                
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                ViewBag.Roles = Usuario.ObtenerRoles();
               
                return View();
            }
        }


        [Authorize]
        public ActionResult Perfil()
        {
            ViewData["Title"] = "Mi perfil";
            var u = repositorioUsuario.ObtenerPorEmail(User.Identity.Name);
            ViewBag.Roles = Usuario.ObtenerRoles();
            if (TempData.ContainsKey("Id"))
                ViewBag.Id = TempData["Id"];
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            return View("Edit", u);
        }


        // GET: Usuario/Edit/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Editar(int id)
        {
            ViewData["Title"] = "Editar usuario";
            var u = repositorioUsuario.ObtenerPorId(id);
            ViewBag.Roles = Usuario.ObtenerRoles();
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            return View(u);
        }

        // POST: Usuario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Editar(int id, Usuario u)
        {
            var vista = "Edit";
            try
            {
                if (!User.IsInRole("Administrador"))
                {
                    vista = "Perfil";
                    var usuarioActual = repositorioUsuario.ObtenerPorEmail(User.Identity.Name);
                    if (usuarioActual.Id != id)//si no es admin, solo puede modificarse él mismo
                        return RedirectToAction(nameof(Index), "Home");
                    else
                    {
                        u.Rol = usuarioActual.Rol;
                        repositorioUsuario.Modificacion(u);
                        TempData["Mensaje"] = "Datos guardados correctamente"; 
                        if (TempData.ContainsKey("Mensaje"))
                            ViewBag.Mensaje = TempData["Mensaje"];

                        return RedirectToAction("Perfil", new { id = id });
                    }
                }
                else
                {
                    repositorioUsuario.Modifica(u);
                    TempData["Mensaje"] = "Datos guardados correctamente";

                }

                if (TempData.ContainsKey("Mensaje"))
                    ViewBag.Error = TempData["Mensaje"];

                return RedirectToAction(nameof(Index));

            }
            catch
            {
                ViewBag.Roles = Usuario.ObtenerRoles();
                return View(vista, u);
            }
        }

        // GET: Usuario/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Borrar(int id)
        {
            repositorioUsuario.Baja(id);
            return RedirectToAction(nameof(Index));
        }

	    [AllowAnonymous]
        // GET: Usuario/Login/
        public ActionResult Login(string returnUrl)
        {
            TempData["returnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginView login)
        {
            try
            {
                //var returnUrl = String.IsNullOrEmpty(TempData["returnUrl"] as String) ? "/Home" : TempData["returnUrl"].ToString();
                if (ModelState.IsValid)
                {
                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: login.Clave,
                        salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));

                    var e = repositorioUsuario.ObtenerPorEmail(login.Usuario);
                    
                    if (e == null || e.Clave != hashed)
                    {
                        ModelState.AddModelError("", "El mail o la clave no son correctos");
                        //TempData["returnUrl"] = returnUrl;
                        return View();
                    }

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, e.Mail),
                        new Claim("FullName", e.Nombre + " " + e.Apellido),
                        new Claim(ClaimTypes.Role, e.RolNombre),
                    };

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity));
                        //TempData.Remove("returnUrl");
                    return RedirectToAction(nameof(Index), "Home");
                }
                //TempData["returnUrl"] = returnUrl;
                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        // GET: Usuario/Logout
        [Route("salir", Name = "logout")]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
