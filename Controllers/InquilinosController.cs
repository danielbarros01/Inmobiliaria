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
    public class InquilinosController : Controller
    {

        private readonly RepositorioInquilino Repo;

        public InquilinosController()
        {
            Repo = new RepositorioInquilino();
        }


        // GET: Inquilinos
        public ActionResult Index()
        {
            var listaInquilinos = Repo.GetInquilinos();
            ViewBag.Mensaje = TempData["Mensaje"];

            return View(listaInquilinos);
        }

        // GET: Inquilinos/Details/5
        public ActionResult Details(int id)
        {
            Inquilino inquilino = Repo.GetInquilino(id);
            return View(inquilino);
        }

        // GET: Inquilinos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Inquilinos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inquilino inquilino)
        {
            try
            {
                Repo.Alta(inquilino);
                TempData["Mensaje"] =  $"Inquilino {inquilino.Nombre} {inquilino.Apellido} con ID {inquilino.Id} creado!";

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
            Inquilino inquilino = Repo.GetInquilino(id);
            return View(inquilino);
        }

        // POST: Inquilinos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Inquilino inquilino)
        {
            try
            {
                Repo.Modificar(inquilino);
                TempData["Mensaje"] =  $"Inquilino {inquilino.Nombre} {inquilino.Apellido} con ID {inquilino.Id} modificado!";

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Inquilinos/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            Inquilino inquilino = Repo.GetInquilino(id);
            return View(inquilino);
        }

        // POST: Inquilinos/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, Inquilino i)
        {
            try
            {
                Repo.Eliminar(id);
                TempData["Mensaje"] =  $"Inquilino {i.Nombre} {i.Apellido} con ID {i.Id} eliminado!";

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}