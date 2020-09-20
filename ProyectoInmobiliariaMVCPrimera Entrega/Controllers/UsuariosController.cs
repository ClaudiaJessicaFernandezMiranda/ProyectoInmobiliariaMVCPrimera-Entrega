using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ProyectoInmobiliariaMVCPrimera_Entrega.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProyectoInmobiliariaMVCPrimera_Entrega.Controllers
{
    [Authorize]
    public class UsuariosController : Controller
    {
        private readonly IConfiguration configuracion;
        private readonly RepositorioUsuario repositorioUsuario;

        public UsuariosController(IConfiguration configuration)
        {
            this.configuracion = configuration;
            repositorioUsuario = new RepositorioUsuario(configuration);
        }
        // GET: Usuarios
        //[Authorize(Policy = "Administrador")]
        public ActionResult Index()
        {
            var lista = repositorioUsuario.ObtenerTodos();
            return View(lista);
        }
        [Authorize(Policy = "Administrador")]
        // GET: Usuarios/Details/5
        public ActionResult Details(int id)
        {
            var i = repositorioUsuario.ObtenerPorId(id);
            return View(i);
        }

        // GET: Usuarios/Create
        //[Authorize(Policy = "Administrador")]
        public ActionResult Create()
        {
            ViewBag.Roles = Usuario.ObtenerRoles();
            return View();
        }

        // POST: Usuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Policy = "Administrador")]
        public ActionResult Create(Usuario u)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = Usuario.ObtenerRoles();
                return View();
            }

            try
            {
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: u.Clave,
                        salt: System.Text.Encoding.ASCII.GetBytes(configuracion["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));
                u.Clave = hashed;
                u.Rol = User.IsInRole("Administrador") ? u.Rol : (int)enRoles.Empleado;
                var nbreRnd = Guid.NewGuid();//posible nombre aleatorio
                int res = repositorioUsuario.Alta(u);
                //if (u.AvatarFile != null && u.UsuarioId > 0)
                //{
                //    string wwwPath = environment.WebRootPath;
                //    string path = Path.Combine(wwwPath, "Uploads");
                //    if (!Directory.Exists(path))
                //    {
                //        Directory.CreateDirectory(path);
                //    }
                //    //Path.GetFileName(u.AvatarFile.FileName);//este nombre se puede repetir
                //    string fileName = "avatar_" + u.UsuarioId + Path.GetExtension(u.AvatarFile.FileName);
                //    string pathCompleto = Path.Combine(path, fileName);
                //    u.Avatar = Path.Combine("/Uploads", fileName);
                //    using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                //    {
                //        u.AvatarFile.CopyTo(stream);
                //    }
                //    repositorioUsuario.Modificacion(u);
                //}
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ViewBag.Roles = Usuario.ObtenerRoles();
                return View();

            }
        }

        // GET: Usuarios/Edit/5
        //[Authorize(Policy = "Administrador")]
        public ActionResult Edit(int id)
        {
            ViewData["Title"] = "Editar usuario";
            var u = repositorioUsuario.ObtenerPorId(id);
            ViewBag.Roles = Usuario.ObtenerRoles();
            return View(u);
        }

        // POST: Usuarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Policy = "Administrador")]
        public ActionResult Edit(int id, Usuario u)
        {
            var vista = "Edit";
            try
            {
                if (!User.IsInRole("Administrador"))
                {
                    vista = "Perfil";
                    var usuarioActual = repositorioUsuario.ObtenerPorEmail(User.Identity.Name);
                    if (usuarioActual.UsuarioId != id)//si no es admin, solo puede modificarse él mismo
                    {
                        repositorioUsuario.Modificacion(u);
                        return RedirectToAction(nameof(Index), "Home");
                    }
                    else
                    {
                        repositorioUsuario.Modificacion(u);
                    }
                }
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ViewBag.Roles = Usuario.ObtenerRoles();
                return View(vista, u);
            }
        }

        // GET: Usuarios/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            var res = repositorioUsuario.ObtenerPorId(id);
            return View(res);
        }

        // POST: Usuarios/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, Usuario entidad)
        {
            try
            {
                repositorioUsuario.Baja(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        [AllowAnonymous]
        // GET: Usuarios/Login/
        public ActionResult Login()
        {
            return View();
        }

        // POST: Usuarios/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginView login)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: login.Clave,salt: System.Text.Encoding.ASCII.GetBytes(configuracion["Salt"]),prf: KeyDerivationPrf.HMACSHA1,iterationCount: 1000,numBytesRequested: 256 / 8));

                    var e = repositorioUsuario.ObtenerPorEmail(login.Email);

                    if (e == null || e.Clave != hashed)
                    {
                        ModelState.AddModelError("", "El email o clave no son correctos");
                        return View();
                    }
                    var claims = new List<Claim>
                    {
                        //new Claim(ClaimTypes.Name,e.UsuarioId),
                        new Claim(ClaimTypes.Name, e.Email),
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

                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }
        // GET: Usuarios/Logout
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}