using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using ProyectoInmobiliariaMVCPrimera_Entrega.Models;

namespace ProyectoInmobiliariaMVCPrimera_Entrega.Controllers
{
    public class PropietariosController : Controller
    {
        
            private readonly IConfiguration configuration;
            private readonly RepositorioPropietario repositorioPropietario;
            // GET: Propietarios
            public PropietariosController(IConfiguration configuration)
            {
                this.configuration = configuration;
                repositorioPropietario = new RepositorioPropietario(configuration);
            }
            public ActionResult Index()
            {
                var lista = repositorioPropietario.ObtenerTodos();
                return View(lista);
            }

            // GET: Propietarios/Details/5
            public ActionResult Details(int id)
            {
                return View();
            }

            // GET: Propietarios/Create
            public ActionResult Create()
            {
                return View();
            }

            // POST: Propietarios/Create
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Create(Propietario p)
            {
                try
                {
                    // TODO: Add insert logic here
                    int res = repositorioPropietario.Alta(p);

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    return View();
                }
            }

            // GET: Propietarios/Edit/5
            public ActionResult Edit(int id)
            {
               var p = repositorioPropietario.ObtenerPorId(id);
                return View(p);
            }

            // POST: Propietarios/Edit/5
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Edit(int id, Propietario p)
            {
            try
            {

                repositorioPropietario.Modificacion(p);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ViewBag.Propietarios = repositorioPropietario.ObtenerTodos();
                var a = repositorioPropietario.ObtenerPorId(id);
                return View(a);
            }
        }

            // GET: Propietarios/Delete/5
            public ActionResult Delete(int id)
            {
               var p = repositorioPropietario.ObtenerPorId(id);

                 return View(p);
            }

            // POST: Propietarios/Delete/5
            [HttpPost]
            [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Propietario entidad)
        {
            try
            {
                repositorioPropietario.Baja(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
    }
