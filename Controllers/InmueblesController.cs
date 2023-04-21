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
        private readonly RepositorioInmueble Repo;
        private readonly RepositorioPropietario RepoPropietarios;
        private readonly RepositorioTipoInmueble RepoTipoInmueble;

        public InmueblesController()
        {
            Repo = new RepositorioInmueble();
            RepoPropietarios = new RepositorioPropietario();
            RepoTipoInmueble = new RepositorioTipoInmueble();
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
        public ActionResult Details2(int id)
        {
            var inm = Repo.GetInmueble(id);   
            return View(inm);
        }

        // GET: Inmuebles/Details/5
        public ActionResult Details(int id)
        {
            var inm = Repo.GetInmueble(id);   
            return View(inm);
        }

        // GET: Inmuebles/Create
        public ActionResult Create()
        {
            ViewBag.Propietarios = RepoPropietarios.GetPropietarios();
            ViewBag.TiposInmueble = RepoTipoInmueble.GetTipos();
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
                TempData["Mensaje"] =  $"Inmueble con direccion {inmueble.Direccion} y ID {inmueble.Id} creado!";

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
            ViewBag.Propietarios = RepoPropietarios.GetPropietarios(); //dato anexo, no se pasa en View porque es justamente anexo a Inmueble, el ViewBag le metemos lo que queremos y se pasa al otro lado
            ViewBag.TiposInmueble = RepoTipoInmueble.GetTipos();
            //ViewData["Propietarios"] = RepoPropietarios.GetPropietarios(); //lo mismo de arriba pero tipo diccionario
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
                TempData["Mensaje"] =  $"Inmueble con {inmueble.Direccion} y ID {inmueble.Id} modificado!";

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
                TempData["Mensaje"] =  $"Inmueble con direccion {inmueble.Direccion} con ID {inmueble.Id} eliminado!";

                return RedirectToAction(nameof(Index));
            }
            catch(Exception e)
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
    }
}