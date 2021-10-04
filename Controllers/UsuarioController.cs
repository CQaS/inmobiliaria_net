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
            this.environment = environment;
            this.configuration = configuration;
            this.repositorioUsuario = new RepositorioUsuario(configuration);
            this.repositorioPropietario = new RepositorioPropietario(configuration);
            this.repositorioInquilino = new RepositorioInquilino(configuration);
            this.repositorioInmueble = new RepositorioInmueble(configuration);
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
            var vista = "Editar";
            try
            {
                
                    var usuarioActual = repositorioUsuario.ObtenerPorEmail(User.Identity.Name);
                    
                    if (usuarioActual.Id != id && !User.IsInRole("Administrador"))//si no es admin, solo puede modificarse él mismo
                    {
                        return RedirectToAction(nameof(Index), "Home");
                    }                        
                    else
                    {
                        if (!User.IsInRole("Administrador"))
                        {
                           u.Rol = usuarioActual.Rol; 
                        }                    

                        if(u.Clave !=null)
                        {
                            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                password: u.Clave,
                                salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                                prf: KeyDerivationPrf.HMACSHA1,
                                iterationCount: 1000,
                                numBytesRequested: 256 / 8));
                            u.Clave = hashed;
                        }
                        else
                        {
                            u.Clave = usuarioActual.Clave;
                        }

                        if(u.AvatarFile !=null)
                        {
                            string wwwPath = environment.WebRootPath;
                            string path = Path.Combine(wwwPath, "img/avatars");
                            if(!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }
                            string fileName = "avatar_" + u.Mail + Path.GetExtension(u.AvatarFile.FileName);
                            string pathCompleto = Path.Combine(path, fileName);
                            u.Avatar = Path.Combine("/img/avatars", fileName);
                            using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                            {
                                u.AvatarFile.CopyTo(stream);
                            }
                        }
                        else
                        {
                            u.Avatar = usuarioActual.Avatar;
                        }


                        repositorioUsuario.Modifica(u);
                        TempData["Mensaje"] = "Datos guardados correctamente";
                        if (TempData.ContainsKey("Mensaje"))
                            ViewBag.Error = TempData["Mensaje"];

                        return RedirectToAction(nameof(Index));
                    }

            }
            catch
            {
                ViewBag.Roles = Usuario.ObtenerRoles();
                TempData["Mensaje"] = "Algo Fallo!";
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

                    if(String.IsNullOrEmpty(login.Pregunta))
                    {
                        if (e == null || e.Clave != hashed)
                        {
                            ModelState.AddModelError("", "El Mail o Pass no son correctos");
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
                        
                        return RedirectToAction(nameof(Index), "Home");
                    }
                    else
                    {
                        if (e == null || login.Pregunta != e.Pregunta)
                        {
                            ModelState.AddModelError("", "Los datos no son correctos");
                            return View();
                        }
                        else
                        {
                            string hashedNew = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                password: login.Clave,
                                salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                                prf: KeyDerivationPrf.HMACSHA1,
                                iterationCount: 1000,
                                numBytesRequested: 256 / 8));
                            
                            repositorioUsuario.ModificarPass(e.Mail, hashedNew); 
                            ModelState.AddModelError("", "Todo OK. Password Renovado");
                            return View(); 
                        }
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        // GET: Usuario/recuperaPass
        public ActionResult RecuperaPass()
        {
            return View();
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
