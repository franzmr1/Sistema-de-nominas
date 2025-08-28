using NominaEmpresa.Aplicacion;
using NominaEmpresa.Dominio;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace NominaEmpresa.Controllers
{
    public class PagosController : Controller
    {
        private readonly PagosAplicacion _pagosAplicacion;

        public PagosController()
        {
            _pagosAplicacion = new PagosAplicacion();
        }

        // GET: Pagos
        public ActionResult Index()
        {
            try
            {
                var pagos = _pagosAplicacion.ObtenerTodosPagos();
                return View(pagos);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cargar los pagos: " + ex.Message;
                return View(new List<Pagos>());
            }
        }

        // POST: Pagos/GenerarPago
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GenerarPago()
        {
            try
            {
                var resultado = _pagosAplicacion.RealizarPago();

                if (resultado.Exitoso)
                {
                    TempData["Mensaje"] = resultado.Mensaje;
                }
                else
                {
                    TempData["Error"] = resultado.Mensaje;
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al procesar los pagos: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        // GET: Pagos/ObtenerDetalle/{id}
        [HttpGet]
        public JsonResult ObtenerDetalle(int id)
        {
            try
            {
                var detalle = _pagosAplicacion.ObtenerDetallePago(id);

                if (detalle == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "No se encontró el pago especificado"
                    }, JsonRequestBehavior.AllowGet);
                }

                var resultado = new
                {
                    success = true,
                    PagosCodigo = detalle.PagosCodigo,
                    ContratoCodigo = detalle.ContratoCodigo,
                    NombreTrabajador = detalle.NombreTrabajador,
                    CargoNombre = detalle.CargoNombre ?? "No especificado",
                    TipoContratoNombre = detalle.TipoContratoNombre ?? "No especificado",
                    ModalidadNombre = detalle.ModalidadNombre ?? "No especificado",
                    PeriodoNombre = detalle.PeriodoNombre,
                    PagosFechaPago = detalle.PagosFechaPago.ToString("yyyy-MM-dd"),
                    SueldoBase = detalle.SueldoBase,
                    PorcentajeBonificaciones = detalle.PorcentajeBonificaciones ?? 0,
                    BonosCalculados = detalle.BonosCalculados ?? 0,
                    PorcentajeDeducciones = detalle.PorcentajeDeducciones ?? 0,
                    DeduccionesCalculadas = detalle.DeduccionesCalculadas ?? 0,
                    HorasExtras = detalle.HorasExtras ?? 0,
                    HorasExtrasCalculadas = detalle.HorasExtrasCalculadas ?? 0,
                    PagosSalarioNeto = detalle.PagosSalarioNeto
                };

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Error al obtener el detalle: " + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Pagos/DescargarBoleta/{id}
        public ActionResult DescargarBoleta(int id)
        {
            try
            {
                var detalle = _pagosAplicacion.ObtenerDetallePago(id);

                if (detalle == null)
                {
                    TempData["Error"] = "No se encontró el pago especificado";
                    return RedirectToAction("Index");
                }

                // Aquí puedes implementar la generación del PDF de la boleta
                // Por ahora, redirigimos con un mensaje
                TempData["Mensaje"] = $"Descarga de boleta para {detalle.NombreTrabajador} - Pago #{id} (Funcionalidad en desarrollo)";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al descargar la boleta: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // GET: Pagos/Resumen
        public ActionResult Resumen()
        {
            try
            {
                var resumen = _pagosAplicacion.ObtenerResumenPagos();
                return View(resumen);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cargar el resumen: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Liberar recursos si es necesario
            }
            base.Dispose(disposing);
        }
    }
}