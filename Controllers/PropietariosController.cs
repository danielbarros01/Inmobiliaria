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
            ViewBag.Mensaje = TempData["Mensaje"];
            //Tempdata es para pasar datos entre acciones

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

                TempData["Mensaje"] =  $"Propietario {propietario.Nombre} {propietario.Apellido} con ID {propietario.Id} cargado con exito!";
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
                TempData["Mensaje"] =  $"Propietario {p.Nombre} {p.Apellido} con ID {p.Id} modificado correctamente!";

                return RedirectToAction(nameof(Index));
            }
            catch(Exception e)
            {
                return View();
            }
        }

        // GET: Propietarios/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            var propietario = Repo.GetPropietario(id);
            
            return View(propietario);
        }

        // POST: Propietarios/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, Propietario p)
        {
            try
            {
                Repo.Eliminar(id);
                TempData["Mensaje"] =  $"Propietario con ID {p.Id} eliminado!";

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
