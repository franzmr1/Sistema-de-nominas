using System;
using System.ComponentModel.DataAnnotations;

namespace NominaEmpresa.Dominio
{
    public class Trabajadores
    {
        public int TrabajadoresCodigo { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "El nombre no debe contener números ni caracteres especiales")]
        public string TrabajadoresNombre { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "El apellido no debe contener números ni caracteres especiales")]
        public string TrabajadoresApellido { get; set; }

        public string TrabajadoresNombreCompleto => $"{TrabajadoresNombre} {TrabajadoresApellido}".ToUpper();

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
        public DateTime? TrabajadoresFechaNacimiento { get; set; }

        [Required(ErrorMessage = "El documento de identidad es obligatorio")]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "El DNI debe contener exactamente 8 números")]
        public string TrabajadoresDocumentoIdentidad { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria")]
        public string TrabajadoresDireccion { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "El teléfono debe contener exactamente 9 números")]
        public string TrabajadoresTelefono { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido")]
        public string TrabajadoresEmail { get; set; }

        public bool TrabajadoresDiscapacidad { get; set; }

        public char TrabajadoresEstado { get; set; }

        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }

        // Constructor vacío
        public Trabajadores() { }

        // Constructor con parámetros
        public Trabajadores(int codigo, string nombre, string apellido, DateTime fechaNacimiento,
                            string documentoIdentidad, string direccion, string telefono,
                            string email, bool discapacidad, char estado)
        {
            TrabajadoresCodigo = codigo;
            TrabajadoresNombre = nombre;
            TrabajadoresApellido = apellido;
            TrabajadoresFechaNacimiento = fechaNacimiento;
            TrabajadoresDocumentoIdentidad = documentoIdentidad;
            TrabajadoresDireccion = direccion;
            TrabajadoresTelefono = telefono;
            TrabajadoresEmail = email;
            TrabajadoresDiscapacidad = discapacidad;
            TrabajadoresEstado = estado;
            FechaCreacion = DateTime.Now;
            FechaModificacion = DateTime.Now;
        }
    }
}
