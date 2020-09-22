using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ProyectoInmobiliariaMVCPrimera_Entrega.Models;

namespace ProyectoInmobiliariaMVCPrimera_Entrega.Controllers
{
    [Authorize]
    public class PagosController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly RepositorioPago repositorioPago;
        private readonly RepositorioAlquiler repositorioAlquiler;
        private readonly RepositorioInquilino repositorioInquilino;
        public PagosController(IConfiguration configuration)
        {
            this.configuration = configuration;
            repositorioAlquiler = new RepositorioAlquiler(configuration);
            repositorioPago = new RepositorioPago(configuration);
            repositorioInquilino = new RepositorioInquilino(configuration);
        }
        // GET: Pagos
        public ActionResult Index(string dni)
        {
            try
            {
                var lista = repositorioAlquiler.ObtenerInmueblePorDni(dni);
                return View(lista);
            }
            catch
            {
                return View();
            }
        }

        // GET: Pagos/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Pagos/Create
        public ActionResult Create(int id)
        {
            ViewBag.Alquiler = repositorioAlquiler.ObtenerPorId(id);
            return View();
        }

        // POST: Pagos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Pago p)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    int res = repositorioPago.Alta(p);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.Alquileres = repositorioAlquiler.ObtenerTodos();
                    return View();
                }
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: Pagos/Edit/5
        //[Authorize(Policy = "Administrador")]
        public ActionResult Edit(int id)
        {
            ViewBag.Pagos = repositorioPago.ObtenerTodos();
            var p = repositorioPago.ObtenerPorId(id);
            return View(p);
        }

        // POST: Pagos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Policy = "Administrador")]
        public ActionResult Edit(int id, Pago p)
        {
            try
            {
                int res = repositorioPago.Modificacion(p);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Propietarios = repositorioPago.ObtenerTodos();
                var i = repositorioPago.ObtenerPorId(id);
                return View(i);
            }
        }


        // GET: Pagos/Delete/5
        public ActionResult Delete(int id)
        {
            var p = repositorioPago.ObtenerPorId(id);
            return View(p);
        }

        // POST: Pagos/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Pago entidad)
        {
            try
            {
                repositorioPago.Baja(id);
                return RedirectToAction(nameof(Index));
            }
            catch//(Exception ex)
            {
                return View();
            }
        }
        // GET: Pago/Buscar/5
        [Route("[controller]/Buscar/{q}", Name = "Buscar")]
        public IActionResult Buscar(string q)
        {
            var res = repositorioPago.BuscarPorNombre(q);
            return View(res);
        }

    }
}