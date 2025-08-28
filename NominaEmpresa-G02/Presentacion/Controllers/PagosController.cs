using System;
using System.Web.Mvc;
using NominaEmpresa.Aplicacion;
using NominaEmpresa.Dominio;
using System.Collections.Generic;

namespace Presentacion.Controllers
{
    public class PagosController : Controller
    {
        private readonly PagosAplicacion _pagosAplicacion;

        public PagosController()
        {
            _pagosAplicacion = new PagosAplicacion();
        }

        /// <summary>
        /// Muestra la lista de todos los pagos
        /// </summary>
        public ActionResult Index()
        {
            try
            {
                var pagos = _pagosAplicacion.ObtenerTodosPagos();
                return View(pagos);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al obtener la lista de pagos: " + ex.Message;
                return View(new List<Pagos>());
            }
        }

        /// <summary>
        /// Procesa la generación de un nuevo pago
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GenerarPago()
        {
            try
            {
                // Verificar si hay un proceso en curso
                if (!ModelState.IsValid)
                {
                    TempData["Error"] = "Datos de entrada no válidos.";
                    return RedirectToAction("Index");
                }

                var (exitoso, mensaje) = _pagosAplicacion.RealizarPago();

                if (exitoso)
                {
                    TempData["Mensaje"] = mensaje ?? "El pago se generó exitosamente.";
                }
                else
                {
                    TempData["Error"] = mensaje ?? "No se pudo completar el proceso de pago.";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al generar el pago: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// Muestra los pagos de un período específico
        /// </summary>
        /// <param name="periodoCodigo">Código del período a consultar</param>
        public ActionResult VerPeriodo(int periodoCodigo)
        {
            try
            {
                if (periodoCodigo <= 0)
                {
                    TempData["Error"] = "El código del período debe ser mayor que cero.";
                    return RedirectToAction("Index");
                }

                var pagosPeriodo = _pagosAplicacion.ObtenerPagosPorPeriodo(periodoCodigo);

                if (pagosPeriodo == null || pagosPeriodo.Count == 0)
                {
                    TempData["Advertencia"] = $"No se encontraron pagos para el período {periodoCodigo}.";
                    return RedirectToAction("Index");
                }

                return View(pagosPeriodo);
            }
            catch (ArgumentException ex)
            {
                TempData["Error"] = "Error de validación: " + ex.Message;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al obtener el detalle del período: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// Maneja errores no controlados en el controlador
        /// </summary>
        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
            TempData["Error"] = "Ha ocurrido un error inesperado. Por favor, contacte al administrador.";
            filterContext.Result = RedirectToAction("Index");
            base.OnException(filterContext);
        }
    }
}