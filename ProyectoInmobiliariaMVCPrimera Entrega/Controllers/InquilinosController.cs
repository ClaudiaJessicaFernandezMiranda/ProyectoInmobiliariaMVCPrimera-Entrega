using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ProyectoInmobiliariaMVCPrimera_Entrega.Models;

namespace ProyectoInmobiliariaMVCPrimera_Entrega.Controllers
{
    public class InquilinosController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly RepositorioInquilino repositorioInquilino;
        public InquilinosController(IConfiguration configuration)
        {
            this.configuration = configuration;
            repositorioInquilino = new RepositorioInquilino(configuration);
        }
        // GET: Inquilinos
        public ActionResult Index()
        {
            var lista = repositorioInquilino.ObtenerTodos();
            return View(lista);
        }

        // GET: Inquilinos/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Inquilinos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Inquilinos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inquilino i)
        {
            try
            {
                // TODO: Add insert logic here
                int res = repositorioInquilino.Alta(i);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Inquilinos/Edit/5
        public ActionResult Edit(int id)
        {
            var i = repositorioInquilino.ObtenerPorId(id);
            return View(i);
        }

        // POST: Inquilinos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Inquilino i)
        {
            try
            {
                // TODO: Add update logic here
                repositorioInquilino.Modificacion(i);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ViewBag.Inquilinos = repositorioInquilino.ObtenerTodos();
                var y = repositorioInquilino.ObtenerPorId(id);
                return View(y);
            }
        }

        // GET: Inquilinos/Delete/5
        public ActionResult Delete(int id)
        {
            var p = repositorioInquilino.ObtenerPorId(id);
            return View(p);
        }

        // POST: Inquilinos/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Inquilino entidad)
        {
            try
            {
                // TODO: Add delete logic here
                repositorioInquilino.Baja(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
