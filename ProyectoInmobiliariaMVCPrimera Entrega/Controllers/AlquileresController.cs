using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ProyectoInmobiliariaMVCPrimera_Entrega.Models;

namespace ProyectoInmobiliariaMVCPrimera_Entrega.Controllers
{
    public class AlquileresController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly RepositorioAlquiler repositorioAlquiler;
        private readonly RepositorioInquilino repositorioInquilino;
        private readonly RepositorioInmueble repositorioInmueble;

        public AlquileresController(IConfiguration configuration)
        {
            this.configuration = configuration;
            repositorioAlquiler = new RepositorioAlquiler(configuration);
            repositorioInquilino = new RepositorioInquilino(configuration);
            repositorioInmueble = new RepositorioInmueble(configuration);
        }
        // GET: Alquileres
        public ActionResult Index()
        {
            var lista = repositorioAlquiler.ObtenerTodos();
            return View(lista);
        }

        // GET: Alquileres/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Alquileres/Create
        public ActionResult Create()
        {
            ViewBag.Inmuebless = repositorioInmueble.ObtenerTodos();
            ViewBag.Inquilinoss = repositorioInquilino.ObtenerTodos();

            return View();
        }

        // POST: Alquileres/Create
        [HttpPost]
        public ActionResult Create(Alquiler alquiler)
        {
            try
            {
                repositorioAlquiler.Alta(alquiler);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Alquileres/Edit/5
        public ActionResult Edit(int id)
        {
            var al = repositorioAlquiler.ObtenerPorId(id);
            ViewBag.Inmuebless = repositorioInmueble.ObtenerTodos();
            ViewBag.Inquilinoss = repositorioInquilino.ObtenerTodos();
            return View(al);
        }

        // POST: Alquileres/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Alquiler a)
        {
            try
            {
                repositorioAlquiler.Modificacion(a);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Alquileres/Delete/5
      
        public ActionResult Delete(int id)
        {
            var p = repositorioAlquiler.ObtenerPorId(id);
            return View(p);
        }

        // POST: Alquileres/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Alquiler entidad)
        {
            try
            {
                // TODO: Add delete logic here
                repositorioAlquiler.Baja(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}