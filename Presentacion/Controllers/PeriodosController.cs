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
                // Obtener períodos con validaciones
                ViewBag.PeriodosActivos = _periodosAplicacion.ObtenerPeriodosActivos();
                ViewBag.PeriodosInactivos = _periodosAplicacion.ObtenerPeriodosInactivosDisponibles(); // Solo los que se pueden activar
                ViewBag.ProximoPeriodo = _periodosAplicacion.ObtenerProximoPeriodoActivo();

                // Información adicional para mostrar en la vista
                ViewBag.UltimoPeriodoPagado = _periodosAplicacion.ObtenerUltimoPeriodoPagado();

                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cargar los períodos: " + ex.Message;
                return View();
            }
        }

        // POST: Periodos/Activar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Activar(int id)
        {
            try
            {
                // VALIDACIÓN TRIPLE DE SEGURIDAD:

                // 1. Verificar que el período existe en la lista de activables
                var periodosActivables = _periodosAplicacion.ObtenerPeriodosInactivos();
                if (!periodosActivables.Any(p => p.PeriodoCodigo == id))
                {
                    TempData["Error"] = "El período seleccionado no está disponible para activación.";
                    return RedirectToAction("Index");
                }

                // 2. Validar reglas de negocio
                var validacion = _periodosAplicacion.ValidarActivacionPeriodo(id);
                if (!validacion.PuedeActivar)
                {
                    TempData["Error"] = $"No se puede activar el período: {validacion.Razon}";
                    return RedirectToAction("Index");
                }

                // 3. Verificar que es exactamente el próximo en secuencia
                var proximoPeriodo = _periodosAplicacion.ObtenerProximoPeriodoActivo();
                if (proximoPeriodo == null || proximoPeriodo.PeriodoCodigo != id)
                {
                    TempData["Error"] = "Solo se puede activar el siguiente período en la secuencia cronológica.";
                    return RedirectToAction("Index");
                }

                // Si pasa todas las validaciones, proceder
                _periodosAplicacion.ActivarPeriodo(id);
                TempData["Success"] = $"Período '{proximoPeriodo.PeriodoNombre}' activado exitosamente.";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al activar el período: " + ex.Message;
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
                // Verificar si el período activo tiene pagos antes de cerrar
                var periodosActivos = _periodosAplicacion.ObtenerPeriodosActivos();

                if (periodosActivos != null && periodosActivos.Any())
                {
                    var periodoActivo = periodosActivos.First();
                    bool tienePagos = _periodosAplicacion.PeriodoTienePagos(periodoActivo.PeriodoCodigo);

                    if (!tienePagos)
                    {
                        TempData["Warning"] = "El período activo no tiene pagos realizados. Se cerrará sin procesar pagos.";
                    }
                }

                _periodosAplicacion.CerrarPeriodoActivo();
                TempData["Success"] = "Período cerrado exitosamente.";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cerrar el período: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // GET: Periodos/ValidarActivacion/{id} - Para validación AJAX
        [HttpGet]
        public JsonResult ValidarActivacion(int id)
        {
            try
            {
                var validacion = _periodosAplicacion.ValidarActivacionPeriodo(id);

                return Json(new
                {
                    puedeActivar = validacion.PuedeActivar,
                    razon = validacion.Razon,
                    success = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    puedeActivar = false,
                    razon = "Error al validar: " + ex.Message,
                    success = false
                }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Periodos/EstadoPagos - Para mostrar información adicional
        public JsonResult EstadoPagos()
        {
            try
            {
                var periodosActivos = _periodosAplicacion.ObtenerPeriodosActivos();
                var periodosInactivos = _periodosAplicacion.ObtenerPeriodosInactivos();
                var ultimoPagado = _periodosAplicacion.ObtenerUltimoPeriodoPagado();

                return Json(new
                {
                    success = true,
                    totalActivos = periodosActivos?.Count ?? 0,
                    totalInactivos = periodosInactivos?.Count ?? 0,
                    ultimoPagado = ultimoPagado?.PeriodoNombre ?? "Ninguno"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}