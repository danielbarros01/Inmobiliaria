using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Inmobiliaria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Inmobiliaria.Controllers
{
    public class PropietariosController : Controller
    {

        private readonly IRepositorioPropietario Repo;
        private readonly IConfiguration config;

        public PropietariosController(IRepositorioPropietario repo, IConfiguration config)
        {
            this.Repo = repo;
            this.config = config;
        }

        // GET: Propietarios
        public ActionResult Index()
        {
            var lista = Repo.ObtenerTodos();
            ViewBag.Mensaje = TempData["Mensaje"];
            //Tempdata es para pasar datos entre acciones

            return View(lista);
        }

        // GET: Propietarios/Details/5
        public ActionResult Details(int id)
        {
            var propietario = Repo.ObtenerPorId(id);
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
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                password: propietario.Password,
                                salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
                                prf: KeyDerivationPrf.HMACSHA1,
                                iterationCount: 10000,
                                numBytesRequested: 256 / 8));

                propietario.Password = hashed;
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
            var propietario = Repo.ObtenerPorId(id);
            
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
            var propietario = Repo.ObtenerPorId(id);
            
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
