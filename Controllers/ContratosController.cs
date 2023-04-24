using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inmobiliaria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inmobiliaria.Controllers
{
    public class ContratosController : Controller
    {


        private readonly RepositorioContrato Repo;
        private readonly RepositorioInquilino RepoInquilinos;
        private readonly RepositorioInmueble RepoInmuebles;

        public ContratosController()
        {
            Repo = new RepositorioContrato();
            RepoInquilinos = new RepositorioInquilino();
            RepoInmuebles = new RepositorioInmueble();
        }
        // GET: Contratos
        public ActionResult Index()
        {
            var lista = Repo.GetContratos();
            ViewBag.Mensaje = TempData["Mensaje"];

            return View(lista);
        }

        // GET: Contratos/Details/5
        public ActionResult Details(int id)
        {
            var contrato = Repo.GetContrato(id);
            return View(contrato);
        }

        // GET: Contratos/Details2/5
        public ActionResult Details2(int id)
        {
            var contrato = Repo.GetContrato(id);
            return View(contrato);
        }

        // GET: Contratos/Create
        public ActionResult Create(int idInmueble)
        {
            ViewBag.Inquilinos2 = RepoInquilinos.GetInquilinos();
            ViewBag.Inmueble = RepoInmuebles.GetInmueble(idInmueble);

            return View();
        }

        // GET: Contratos/Create
        public ActionResult Create2(int idInmueble)
        {
            ViewBag.Inquilinos2 = RepoInquilinos.GetInquilinos();
            ViewBag.Inmueble = RepoInmuebles.GetInmueble(idInmueble);

            return View();
        }

        // POST: Contratos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Contrato contrato)
        {
            try
            {
                Repo.Alta(contrato);
                TempData["Mensaje"] = $"Contrato {contrato.Id} de {contrato.Inquilino.ToString()} creado!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = $"Contrato {ex}!";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Contratos/Edit/5
        public ActionResult Edit(int id)
        {
            var contrato = Repo.GetContrato(id);
            ViewBag.Inquilinos = RepoInquilinos.GetInquilinos();
            ViewBag.Inmuebles = RepoInmuebles.GetInmuebles();
            TempData["Mensaje"] = $"Contrato {contrato.Id} de {contrato.Inquilino.ToString()} modificado!";
            return View(contrato);
        }

        // POST: Contratos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Contrato contrato)
        {
            try
            {
                // TODO: Add update logic here
                Repo.Modificar(contrato);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return View();
            }
        }

        // GET: Contratos/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            var contrato = Repo.GetContrato(id);
            return View(contrato);
        }

        // POST: Contratos/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                Repo.Eliminar(id);
                TempData["Mensaje"] = $"Contrato {id} eliminado!";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult ListPagosContrato(int id)
        {
            try
            {
                //Repo.PagosContrato(id);
                return View();
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}