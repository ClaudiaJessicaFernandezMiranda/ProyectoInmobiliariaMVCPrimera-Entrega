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
    public class InmueblesController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly RepositorioPropietario repositorioPropietario;
        private readonly RepositorioInmueble repositorioInmueble;
        public InmueblesController(IConfiguration configuration)
        {
            this.configuration = configuration;
            repositorioPropietario = new RepositorioPropietario(configuration);
            repositorioInmueble = new RepositorioInmueble(configuration);
        }

        // GET: Inmuebles
        public ActionResult Index(int id, DateTime? inicio, DateTime? fin)
        {
            var lista = repositorioInmueble.ObtenerTodos();
            if (id == 2)
            {
                lista = repositorioInmueble.ObtenerTodosDisponible();
            }
            else if (id == 3)
            {
                lista = repositorioInmueble.ObtenerTodosNoDisponible();
            }
            else if (inicio != null && fin != null)
            {
                lista = repositorioInmueble.ObtenerTodosRangoDeFechas(inicio, fin);
            }
            else
            {
                lista = repositorioInmueble.ObtenerTodos();
            }
            return View(lista);
        } 
        // GET: Inmuebles/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Inmuebles/Create
        public ActionResult Create()
        {
            ViewBag.Propietarios = repositorioPropietario.ObtenerTodos();
            return View();
        }

        // POST: Inmuebles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create(Inmueble i)
        {
            try
            {
                //trow new Exception("Error");
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    int res = repositorioInmueble.Alta(i);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.Propietarios = repositorioPropietario.ObtenerTodos();
                    return View();
                }
            }
            catch (Exception ex)
            {

                return View();
            }
        }

        // GET: Inmuebles/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.Propietarios = repositorioPropietario.ObtenerTodos();
            var i = repositorioInmueble.ObtenerPorId(id);
            return View(i);
        }

        // POST: Inmuebles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit(int id, Inmueble i)
        {
            try
            {
                int res = repositorioInmueble.Modificacion(i);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Inmuebles = repositorioInmueble.ObtenerTodos();
                var ii = repositorioInmueble.ObtenerPorId(id);
                return View(ii);
            }
        }

        // GET: Inmuebles/Delete/5
        [Authorize(Policy = "Administrador")]

        public ActionResult Delete(int id)
        {
            var i = repositorioInmueble.ObtenerPorId(id);

            return View(i);
        }

        // POST: Inmuebles/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]

        public ActionResult Delete(int id, Inmueble entidad)
        {
            try
            {
                //var res = repositorioInmueble.ObtenerPorId(id);
                //return View(res);
                repositorioInmueble.Baja(id);
                return RedirectToAction(nameof(Index));
            }
            catch//(Exception ex)
            {
                return View();
            }

        }

        public ActionResult ListarInmuebleDisponiblesConSusDuenio()
        {
            var lista = repositorioInmueble.ListarLosInmueblesQueDisponiblesConSuDuenio();
            return View(lista);
        }


        public ActionResult ListarInmuebleDeUnPropietario(string dni)
        {
            try
            {
                var lista = repositorioInmueble.ListarTodosLosInmueblesQueLesCorrespondanAUnPropietario(dni);
                return View(lista);
            }
            catch
            {
                return View();
            }
        }


    }
}
