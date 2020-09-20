using System.ComponentModel.DataAnnotations;

namespace ProyectoInmobiliariaMVCPrimera_Entrega.Models
{
    public class LoginView
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Clave { get; set; }
    }
}
