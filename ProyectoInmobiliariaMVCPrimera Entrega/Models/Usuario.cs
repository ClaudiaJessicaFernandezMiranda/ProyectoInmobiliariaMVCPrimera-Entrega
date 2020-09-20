using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace ProyectoInmobiliariaMVCPrimera_Entrega.Models
{
    public enum enRoles
    {
        SuperAdministrador = 1,
        Administrador = 2,
        Empleado = 3,
    }

    public class Usuario
    {
        [Key]
        [Display(Name = "Código")]
        public int UsuarioId { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Clave { get; set; }
        //public string Rol { get; set; }

        public int Rol { get; set; }

        public string RolNombre => Rol > 0 ? ((enRoles)Rol).ToString() : "";

        public static IDictionary<int, string> ObtenerRoles()
        {
            SortedDictionary<int, string> roles = new SortedDictionary<int, string>();
            Type tipoEnumRol = typeof(enRoles);
            foreach (var valor in Enum.GetValues(tipoEnumRol))
            {
                roles.Add((int)valor, Enum.GetName(tipoEnumRol, valor));
            }
            return roles;
        }
    }
}