using NominaEmpresa.Aplicacion;
using NominaEmpresa.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Presentacion.Controllers
{
    public class ContratosLaboralesController : Controller
    {
        private readonly ContratosLaboralesAplicacion _contratosLaboralesApp;

        // Constructor por defecto necesario para MVC
        public ContratosLaboralesController()
        {
            _contratosLaboralesApp = new ContratosLaboralesAplicacion();
        }

        // GET: ContratosLaborales
        public ActionResult Index()
        {
            return ExecuteAction(() =>
            {
                var contratosActivos = _contratosLaboralesApp.ObtenerContratosActivos();
                return View(contratosActivos);
            }, "contratos activos");
        }

        // GET: ContratosLaborales/Inactivos
        public ActionResult Inactivos()
        {
            return ExecuteAction(() =>
            {
                var contratosInactivos = _contratosLaboralesApp.ObtenerContratosInactivos();
                return View(contratosInactivos);
            }, "contratos inactivos");
        }

        // GET: ContratosLaborales/Create
        public ActionResult Create()
        {
            return ExecuteAction(() =>
            {
                CargarListas();
                return View(new ContratoLaborales());
            }, "formulario de creación");
        }

        // POST: ContratosLaborales/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ContratoLaborales contrato)
        {
            if (!ModelState.IsValid)
            {
                CargarListas(); // Asegúrate de cargar los datos necesarios para la vista
                return View(contrato);
            }

            try
            {
                int nuevoContratoCodigo = _contratosLaboralesApp.CrearContrato(contrato);
                TempData["SuccessMessage"] = $"Contrato N° {nuevoContratoCodigo} creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al crear el contrato: {ex.Message}");
                TempData["ErrorMessage"] = $"Error al crear el contrato: {ex.Message}";
                CargarListas(); // Recarga los datos necesarios si es un formulario con listas desplegables o similares
                return View(contrato);
            }
        }


        // GET: ContratosLaborales/Edit/5
        public ActionResult Edit(int id)
        {
            return ExecuteAction(() =>
            {
                var contrato = _contratosLaboralesApp.ObtenerContratoPorId(id);
                if (contrato == null)
                {
                    TempData["ErrorMessage"] = $"No se encontró el contrato con código {id}.";
                    return RedirectToAction(nameof(Index));
                }

                CargarListas();
                return View(contrato);
            }, "edición de contrato");
        }

        // POST: ContratosLaborales/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ContratoLaborales contrato)
        {
            if (!ModelState.IsValid)
            {
                CargarListas();
                return View(contrato);
            }

            try
            {
                _contratosLaboralesApp.ActualizarContrato(contrato);
                TempData["SuccessMessage"] = $"Contrato N° {contrato.ContratoCodigo} actualizado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al actualizar el contrato: " + ex.Message);
                TempData["ErrorMessage"] = "Error al actualizar el contrato: " + ex.Message;
                CargarListas();
                return View(contrato);
            }
        }


        // GET: ContratosLaborales/Delete/5
        public ActionResult Delete(int id)
        {
            return ExecuteAction(() =>
            {
                var contrato = _contratosLaboralesApp.ObtenerContratoPorId(id);
                if (contrato == null)
                {
                    TempData["ErrorMessage"] = $"No se encontró el contrato con código {id}.";
                    return RedirectToAction(nameof(Index));
                }
                return View(contrato);
            }, "carga de contrato para eliminación");
        }

        // POST: ContratosLaborales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            return ExecuteAction(() =>
            {
                _contratosLaboralesApp.EliminarContrato(id);
                TempData["SuccessMessage"] = $"Contrato N° {id} eliminado exitosamente.";
                return RedirectToAction(nameof(Index));
            }, "eliminar contrato");
        }

        // GET: ContratosLaborales/Recuperar/5
        public ActionResult Recuperar(int id)
        {
            return ExecuteAction(() =>
            {
                var contratoARecuperar = _contratosLaboralesApp.ObtenerContratoPorId(id);
                if (contratoARecuperar == null)
                {
                    throw new Exception($"No se encontró el contrato con código {id}.");
                }

                _contratosLaboralesApp.RecuperarContrato(id);
                TempData["SuccessMessage"] = $"Contrato N° {id} recuperado exitosamente.";
                return RedirectToAction(nameof(Inactivos));
            }, "recuperar contrato");
        }

        private void CargarListas()
        {
            ViewBag.TiposContrato = ConvertirASelectList(
                _contratosLaboralesApp.ObtenerTiposContrato(),
                tc => tc.TipoContratosCodigo,
                tc => tc.TipoContratosNombre);

            ViewBag.Departamentos = ConvertirASelectList(
                _contratosLaboralesApp.ObtenerDepartamentos(),
                d => d.DepartamentosCodigo,
                d => d.DepartamentosNombre);

            ViewBag.Cargos = ConvertirASelectList(
                _contratosLaboralesApp.ObtenerCargos(),
                c => c.CargosCodigo,
                c => c.CargosNombre);

            ViewBag.Modalidades = ConvertirASelectList(
                _contratosLaboralesApp.ObtenerModalidades(),
                m => m.ModalidadCodigo,
                m => m.ModalidadNombre);

            ViewBag.Trabajadores = ConvertirASelectList(
                _contratosLaboralesApp.ObtenerTrabajadores(),
                t => t.TrabajadoresCodigo,
                t => t.TrabajadoresNombreCompleto);

            ViewBag.DiasSemana = new List<SelectListItem>
            {
                new SelectListItem { Value = "LUN", Text = "Lunes" },
                new SelectListItem { Value = "MAR", Text = "Martes" },
                new SelectListItem { Value = "MIE", Text = "Miércoles" },
                new SelectListItem { Value = "JUE", Text = "Jueves" },
                new SelectListItem { Value = "VIE", Text = "Viernes" },
                new SelectListItem { Value = "SAB", Text = "Sábado" },
                new SelectListItem { Value = "DOM", Text = "Domingo" }
            };
        }

        private ActionResult ExecuteAction(Func<ActionResult> action, string operationName)
        {
            try
            {
                return action();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al {operationName}: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }
        // GET: ContratosLaborales/Details/5
        public ActionResult Details(int id)
        {
            return ExecuteAction(() =>
            {
                var contrato = _contratosLaboralesApp.ObtenerContratoPorId(id);
                if (contrato == null)
                {
                    TempData["ErrorMessage"] = $"No se encontró el contrato con código {id}.";
                    return RedirectToAction(nameof(Index));
                }

                return View(contrato);
            }, "obtener detalles del contrato");
        }

        private static List<SelectListItem> ConvertirASelectList<T>(
            IEnumerable<T> items,
            Func<T, object> valueSelector,
            Func<T, string> textSelector)
        {
            return items.Select(item => new SelectListItem
            {
                Value = valueSelector(item).ToString(),
                Text = textSelector(item)
            }).ToList();
        }
    }
}