using NominaEmpresa.Aplicacion;
using NominaEmpresa.Dominio;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Presentacion.Controllers
{
    public class TrabajadoresController : Controller
    {
        private readonly TrabajadoresAplicacion _trabajadoresApp;

        public TrabajadoresController()
        {
            _trabajadoresApp = new TrabajadoresAplicacion();
        }

        // GET: Trabajadores
        public ActionResult Index()
        {
            List<Trabajadores> trabajadores = _trabajadoresApp.ObtenerTrabajadoresActivos();
            return View(trabajadores);
        }

        // GET: Trabajadores/Details/5
        public ActionResult Details(int id)
        {
            Trabajadores trabajador = _trabajadoresApp.ObtenerTrabajadorPorId(id);
            if (trabajador == null)
            {
                TempData["ErrorMessage"] = "El trabajador no fue encontrado.";
                return RedirectToAction("Index");
            }
            return View(trabajador);
        }

        public ActionResult DetailsInactivos(int id)
        {
            // Obtener un trabajador específico por ID
            Trabajadores trabajador = _trabajadoresApp.ObtenerTrabajadorPorId(id);

            if (trabajador == null)
            {
                return HttpNotFound();
            }

            return View(trabajador);
        }

        // GET: Trabajadores/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Trabajadores/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Trabajadores trabajador)
        {
            if (!ModelState.IsValid)
            {
                return View(trabajador);
            }

            try
            {
                _trabajadoresApp.AgregarTrabajador(trabajador);
                TempData["SuccessMessage"] = "Trabajador creado exitosamente.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al crear el trabajador: " + ex.Message);
                TempData["ErrorMessage"] = "Error al crear el trabajador: " + ex.Message;
                return View(trabajador);
            }
        }

        // GET: Trabajadores/Edit/5
        public ActionResult Edit(int id)
        {
            Trabajadores trabajador = _trabajadoresApp.ObtenerTrabajadorPorId(id);
            if (trabajador == null)
            {
                TempData["ErrorMessage"] = "El trabajador no fue encontrado.";
                return RedirectToAction("Index");
            }
            return View(trabajador);
        }

        // POST: Trabajadores/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Trabajadores trabajador)
        {
            if (!ModelState.IsValid)
            {
                return View(trabajador);
            }

            try
            {
                _trabajadoresApp.ActualizarTrabajador(trabajador);
                TempData["SuccessMessage"] = "Trabajador actualizado exitosamente.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al actualizar el trabajador: " + ex.Message);
                TempData["ErrorMessage"] = "Error al actualizar el trabajador: " + ex.Message;
                return View(trabajador);
            }
        }

        // GET: Trabajadores/Delete/5
        public ActionResult Delete(int id)
        {
            Trabajadores trabajador = _trabajadoresApp.ObtenerTrabajadorPorId(id);
            if (trabajador == null)
            {
                TempData["ErrorMessage"] = "El trabajador no fue encontrado.";
                return RedirectToAction("Index");
            }
            return View(trabajador);
        }

        // POST: Trabajadores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                _trabajadoresApp.EliminarTrabajador(id);
                TempData["SuccessMessage"] = "Trabajador eliminado exitosamente.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al eliminar el trabajador: " + ex.Message;
                return RedirectToAction("Delete", new { id });
            }
        }

        // GET: Trabajadores/TrabajadoresInactivos
        public ActionResult TrabajadoresInactivos()
        {
            List<Trabajadores> trabajadoresInactivos = _trabajadoresApp.ObtenerTrabajadoresInactivos();
            return View(trabajadoresInactivos);
        }


  
        public ActionResult Recuperar(int id)
        {
            try
            {
                _trabajadoresApp.RecuperarTrabajador(id);
                TempData["SuccessMessage"] = "Trabajador recuperado exitosamente.";
                return RedirectToAction("TrabajadoresInactivos");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al recuperar el trabajador: " + ex.Message;
                return RedirectToAction("TrabajadoresInactivos");
            }
        }
    }
}
