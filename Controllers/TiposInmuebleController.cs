using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Inmobiliaria.Models;
using MySql.Data.MySqlClient;

namespace Inmobiliaria.Controllers
{
    public class TipoInmuebleController : Controller
    {
        private readonly RepositorioTipoInmueble Repo;

        public TipoInmuebleController()
        {
            Repo = new RepositorioTipoInmueble();
        }

        // GET: TipoInmueble
        public ActionResult Index()
        {
            var list = Repo.GetTipos();
            ViewBag.Mensaje = TempData["Mensaje"];
            return View(list);
        }

        // GET: TipoInmueble/Details/5
        public ActionResult Details(int id)
        {
            var tipo = Repo.GetTipo(id);
            return View(tipo);
        }

        // GET: TipoInmueble/Edit/5
        public ActionResult Edit(int id)
        {
            var tipoInmueble = Repo.GetTipo(id);
            return View(tipoInmueble);
        }

        // POST: TipoInmueble/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, TipoInmueble t)
        {
            try
            {
                Repo.Modificar(t);
                TempData["Mensaje"] =  $"Tipo de Inmueble {t.Tipo} modificado!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                throw;
                return View();
            }
        }

        // GET: TipoInmueble/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TipoInmueble/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string tipo)
        {
            try
            {
                Repo.Alta(tipo);
                TempData["Mensaje"] =  $"Tipo de Inmueble {tipo} creado!";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
            finally
            {
                Console.WriteLine(@"en controller" + tipo);
            }
        }

        // GET: TipoInmueble/Delete/5
        public ActionResult Delete(int id)
        {
            var tipoInmueble = Repo.GetTipo(id);
            return View(tipoInmueble);
        }

        // POST: TipoInmueble/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, TipoInmueble t)
        {
            try
            {
                // TODO: Add delete logic here
                Repo.Eliminar(id);
                TempData["Mensaje"] =  $"Tipo de Inmueble {t.Tipo} eliminado!";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}