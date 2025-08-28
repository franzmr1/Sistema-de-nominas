using System;
using System.Web.Mvc;
using System.Linq;
using NominaEmpresa.Aplicacion;
using NominaEmpresa.Dominio;

namespace Presentacion.Controllers
{
    public class PeriodosController : Controller
    {
        private readonly PeriodosAplicacion _periodosAplicacion;

        public PeriodosController()
        {
            _periodosAplicacion = new PeriodosAplicacion();
        }

        // GET: Periodos
        public ActionResult Index()
        {
            try
            {
                // Obtenemos ambas listas
                ViewBag.PeriodosActivos = _periodosAplicacion.ObtenerPeriodosActivos();
                ViewBag.PeriodosInactivos = _periodosAplicacion.ObtenerPeriodosInactivos();
                ViewBag.ProximoPeriodo = _periodosAplicacion.ObtenerProximoPeriodoActivo();

                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cargar los periodos: " + ex.Message;
                return View();
            }
        }

        // GET: Periodos/Create
        public ActionResult Create()
        {
            return View(new Periodos { PeriodoActivo = true });
        }

        // POST: Periodos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Periodos periodo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _periodosAplicacion.GuardarPeriodo(periodo);
                    TempData["Success"] = "Periodo creado exitosamente.";
                    return RedirectToAction("Index");
                }
                return View(periodo);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al crear el periodo: " + ex.Message);
                return View(periodo);
            }
        }

        // GET: Periodos/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                var periodosActivos = _periodosAplicacion.ObtenerPeriodosActivos();
                var periodosInactivos = _periodosAplicacion.ObtenerPeriodosInactivos();
                var periodo = periodosActivos.Concat(periodosInactivos)
                                          .FirstOrDefault(p => p.PeriodoCodigo == id);

                if (periodo == null)
                {
                    TempData["Error"] = "Periodo no encontrado.";
                    return RedirectToAction("Index");
                }

                return View(periodo);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cargar el periodo: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // POST: Periodos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Periodos periodo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _periodosAplicacion.GuardarPeriodo(periodo);
                    TempData["Success"] = "Periodo actualizado exitosamente.";
                    return RedirectToAction("Index");
                }
                return View(periodo);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al actualizar el periodo: " + ex.Message);
                return View(periodo);
            }
        }

        // POST: Periodos/Activar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Activar(int id)
        {
            try
            {
                _periodosAplicacion.ActivarPeriodo(id);
                TempData["Success"] = "Periodo activado exitosamente.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al activar el periodo: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // POST: Periodos/Cerrar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cerrar()
        {
            try
            {
                _periodosAplicacion.CerrarPeriodoActivo();
                TempData["Success"] = "Periodo cerrado exitosamente.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cerrar el periodo: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // POST: Periodos/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                var periodo = _periodosAplicacion.ObtenerPeriodosActivos()
                    .FirstOrDefault(p => p.PeriodoCodigo == id);

                if (periodo != null)
                {
                    periodo.PeriodoActivo = false;
                    _periodosAplicacion.GuardarPeriodo(periodo);
                    TempData["Success"] = "Periodo desactivado exitosamente.";
                }
                else
                {
                    TempData["Error"] = "Periodo no encontrado.";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al desactivar el periodo: " + ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}