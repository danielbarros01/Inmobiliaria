using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Inmobiliaria.Models;
using Microsoft.AspNetCore.Authorization;

namespace Inmobiliaria.Controllers
{
    public class InmueblesController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly RepositorioInmueble Repo;
        private readonly IRepositorioPropietario RepoPropietarios;
        private readonly RepositorioTipoInmueble RepoTipoInmueble;
        private readonly RepositorioContrato RepoContratos;

        public InmueblesController(IConfiguration configuration, IRepositorioPropietario repoPropietarios)
        {
            //string connectionString = configuration.GetSection("ConnectionStrings")["MySql"];
            string connectionString = configuration["ConnectionStrings:MySql"];

            Repo = new RepositorioInmueble();
            RepoPropietarios = repoPropietarios;
            RepoTipoInmueble = new RepositorioTipoInmueble();
            RepoContratos = new RepositorioContrato(connectionString);
            this.configuration = configuration;
        }

        // GET: Inmuebles
        [Authorize]
        public ActionResult Index()
        {
            var lista = Repo.GetInmuebles();
            ViewBag.Mensaje = TempData["Mensaje"];
            ViewBag.Error = TempData["Error"];

            return View(lista);
        }

        // GET: Inmuebles/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var inm = Repo.GetInmueble(id);
                ViewBag.Contrato = RepoContratos.obtenerIdPorInmueble(id);
                ViewBag.Disponible = RepoContratos.GetContratoVigentePorInmueble(id);

                return View(inm);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        // GET: Inmuebles/Create
        public ActionResult Create()
        {
            ViewBag.Propietarios = RepoPropietarios.ObtenerTodos();
            ViewBag.TiposInmueble = RepoTipoInmueble.GetTipos();
            ViewBag.Usos = Inmueble.ObtenerUsos();

            return View();
        }

        // POST: Inmuebles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inmueble inmueble)
        {
            try
            {
                // TODO: Add insert logic here
                Repo.Alta(inmueble);
                TempData["Mensaje"] = $"Inmueble con direccion {inmueble.Direccion} y ID {inmueble.Id} creado!";

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Inmuebles/Edit/5
        public ActionResult Edit(int id)
        {
            var inm = Repo.GetInmueble(id);
            ViewBag.Propietarios = RepoPropietarios.ObtenerTodos(); //dato anexo, no se pasa en View porque es justamente anexo a Inmueble, el ViewBag le metemos lo que queremos y se pasa al otro lado
            ViewBag.TiposInmueble = RepoTipoInmueble.GetTipos();
            //ViewData["Propietarios"] = RepoPropietarios.GetPropietarios(); //lo mismo de arriba pero tipo diccionario
            ViewBag.Usos = Inmueble.ObtenerUsos();

            return View(inm);
        }

        // POST: Inmuebles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Inmueble inmueble)
        {
            try
            {
                Repo.Modificar(inmueble);
                TempData["Mensaje"] = $"Inmueble con {inmueble.Direccion} y ID {inmueble.Id} modificado!";

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Inmuebles/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            var inm = Repo.GetInmueble(id);
            return View(inm);
        }

        // POST: Inmuebles/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, Inmueble inmueble)
        {
            try
            {
                Repo.Eliminar(id);
                TempData["Mensaje"] = $"Inmueble con direccion {inmueble.Direccion} con ID {inmueble.Id} eliminado!";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                TempData["Error"] = $"Error: {e.Message}";
                return RedirectToAction(nameof(Index));
            }
        }




        //---extra

        // GET: Inmuebles/Disponibles
        [HttpGet]
        public ActionResult Disponibles(int id)
        {
            var datos = Repo.GetDisponibles();
            ViewBag.FiltroDisp = true;

            return View("Index", datos);
        }

        // GET: Inmuebles/VerInmueblesPorPropietario
        [HttpGet]
        public ActionResult VerInmueblesPorPropietario(int idPropietario)
        {
            var datos = Repo.GetInmueblesPropietario(idPropietario);
            ViewBag.FiltroPropietario = true;
            ViewBag.NumeroDeInmuebles = datos.Count();
            return View("Index", datos);
        }

        // GET: Inmuebles/FiltrarPorFechas
        [HttpGet]
        public ActionResult FiltrarPorFechas(DateTime fechaDesde, DateTime fechaHasta)
        {
            var datos = Repo.GetInmueblesPorFechas(fechaDesde, fechaHasta);
            ViewBag.FiltroFechas = true;
            ViewBag.Fechas =
            new
            {
                fechaDesde = fechaDesde.ToString("dd/MM/yyyy"),
                fechaHasta = fechaHasta.ToString("dd/MM/yyyy")
            };
            ViewBag.NumeroDeInmuebles = datos.Count();

            return View("Index", datos);
        }

    }
}