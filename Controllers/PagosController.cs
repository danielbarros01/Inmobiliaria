using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inmobiliaria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Inmobiliaria.Controllers
{
    public class PagosController : Controller
    {

        private readonly RepositorioPago Repo;
        private readonly RepositorioContrato RepoContrato;

        public PagosController()
        {
            Repo = new RepositorioPago();
            RepoContrato = new RepositorioContrato();
        }

        // GET: Pagos
        public ActionResult Index()
        {
            var listPagos = Repo.GetPagos();
            return View(listPagos);
        }

        // GET: Pagos/Details/5
        public ActionResult Details(int idPago, int idContrato)
        {
            var pago = Repo.GetPago(idPago, idContrato);
            return View(pago);
        }

        // GET: Pagos/Create
        public ActionResult Create()
        {
            ViewBag.Contratos = RepoContrato.GetContratos();

            return View();
        }

        // POST: Pagos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Pago p)
        {
            try
            {
                Repo.Alta(p);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return View();
            }
        }
        
        // POST: Pagos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTwo(Pago p, int contratoId)
        {
            try
            {
                Repo.Alta(p);

                return RedirectToAction("ListPagosContrato", "Pagos", new { id = contratoId });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return View();
            }
        }



        // GET: Pagos/Edit/5
        public ActionResult Edit(int idPago, int idContrato)
        {
            var p = Repo.GetPago(idPago, idContrato);
            return View(p);
        }

        // POST: Pagos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int idPago, int idContrato, Pago pago)
        {
            try
            {
                Repo.Modificar(pago);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Pagos/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int idPago, int idContrato)
        {
            var pago = Repo.GetPago(idPago, idContrato);
            return View(pago);
        }

        // POST: Pagos/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int idPago, int idContrato, Pago p)
        {
            try
            {
                Repo.Eliminar(idPago, idContrato);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return View();
            }
        }


        // GET: Pagos/DatosExtra/5
        [HttpGet]
        public Object DatosExtra(int id)
        {
            var datos = Repo.GetDatosExtra(id);

            return datos;
        }


        public ActionResult ListPagosContrato(int id)
        {
            try
            {
                var list = Repo.PagosContrato(id);
                ViewBag.Contrato = RepoContrato.GetContrato(id);
                return View(list);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}