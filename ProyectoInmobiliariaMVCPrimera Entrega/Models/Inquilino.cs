using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoInmobiliariaMVCPrimera_Entrega.Models
{
    public class Inquilino
    {
        public int InquilinoId { get; set; }
        public string Dni { get; set; }

        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
                    

    }
}
