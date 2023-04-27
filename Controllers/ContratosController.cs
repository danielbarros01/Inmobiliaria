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
        private readonly RepositorioContrato RepoContratos;

        public ContratosController()
        {
            Repo = new RepositorioContrato();
            RepoInquilinos = new RepositorioInquilino();
            RepoInmuebles = new RepositorioInmueble();
            RepoContratos = new RepositorioContrato();
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
            try
            {
                var contrato = Repo.GetContrato(id);
                ViewBag.Multa = TempData["Multa"];
                ViewBag.SinVigencia = 
                    contrato.Desde >= DateTime.Now && contrato.Hasta >= DateTime.Now 
                    || contrato.Desde <= DateTime.Now && contrato.Hasta <= DateTime.Now;

                return View(contrato);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        // GET: Contratos/Create
        public ActionResult CreateConInmueble(int idInmueble)
        {
            ViewBag.Inquilinos2 = RepoInquilinos.GetInquilinos();
            ViewBag.Inmueble = RepoInmuebles.GetInmueble(idInmueble);
            ViewBag.Fechas = Repo.GetFechasContratos(idInmueble);

            return View("Create");
        }

        // GET: Contratos/Create
        public ActionResult Create()
        {
            try
            {
                ViewBag.Inquilinos2 = RepoInquilinos.GetInquilinos();
                ViewBag.Inmuebles = RepoInmuebles.GetInmuebles();

                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
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

        // GET: Inmuebles/ContratosPorInmueble/?
        [HttpGet]
        public ActionResult ContratosPorInmueble(int idInmueble)
        {
            var datos = Repo.GetContratosInmueble(idInmueble);
            ViewBag.FiltroInmueble = true;
            ViewBag.NumeroDeContratos = datos.Count();
            return View("Index", datos);
        }

        // GET: Inmuebles/ContratosVigentes
        [HttpGet]
        public ActionResult ContratosVigentes()
        {
            var datos = Repo.GetContratosVigentes();
            ViewBag.FiltroVigentes = true;
            ViewBag.NumeroDeContratos = datos.Count();
            return View("Index", datos);
        }

        // POST: Inmuebles/ContratosVigentes
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CancelarContrato(int id)
        {
            var datos = Repo.CancelarContrato(id);
            TempData["Multa"] = $"Contrato Cancelado, debera pagar una multa";
            
            return RedirectToAction(nameof(Details), new { id = id });
        }


        // GET: Contratos/Vigente
        [HttpGet]
        public Object Vigente(int idInmueble)
        {
            var fechas = Repo.GetContratoVigentePorInmueble(idInmueble);
            
            return fechas;
        }

        // GET: Contratos/FechasInmueble
        [HttpGet]
        public Object FechasInmueble(int idInmueble)
        {
            var fechas = Repo.GetFechasContratos(idInmueble);
            
            return fechas;
        }
    }
}