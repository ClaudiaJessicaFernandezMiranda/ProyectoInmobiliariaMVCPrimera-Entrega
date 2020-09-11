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
        public ActionResult Index()
        {
            var lista = repositorioInmueble.ObtenerTodos();
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
        public ActionResult Delete(int id)
        {
            var i = repositorioInmueble.ObtenerPorId(id);

            return View(i);
        }

        // POST: Inmuebles/Delete/5
        [HttpPost]
      
        //Delete(int id)
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


       
    }
}
