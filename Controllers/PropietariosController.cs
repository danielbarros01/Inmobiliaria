using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Inmobiliaria.Models;

namespace Inmobiliaria.Controllers
{
    public class PropietariosController : Controller
    {

        private readonly RepositorioPropietario Repo;

        public PropietariosController()
        {
            Repo = new RepositorioPropietario();
        }

        // GET: Propietarios
        public ActionResult Index()
        {
            var lista = Repo.GetPropietarios();

            return View(lista);
        }

        // GET: Propietarios/Details/5
        public ActionResult Details(int id)
        {
            var propietario = Repo.GetPropietario(id);
            return View(propietario);
        }

        // GET: Propietarios/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Propietarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Propietario propietario)
        {
            try
            {
                Repo.Alta(propietario);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Propietarios/Edit/5
        public ActionResult Edit(int id)
        {
            var propietario = Repo.GetPropietario(id);
            return View(propietario);
        }

        // POST: Propietarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Propietario p)
        {
            try
            {
                Repo.Modificar(p);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                throw;
                return View();
            }
        }

        // GET: Propietarios/Delete/5
        public ActionResult Delete(int id)
        {
            var propietario = Repo.GetPropietario(id);
            return View(propietario);
        }

        // POST: Propietarios/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                Repo.Eliminar(id);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                throw;
                return View();
            }
        }
    }
}
