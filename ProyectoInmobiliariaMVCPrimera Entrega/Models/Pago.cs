using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoInmobiliariaMVCPrimera_Entrega.Models
{
    public class Pago
    {
        [Key]
        public int PagoId { get; set; }
        public string Numero { get; set; }
        public DateTime Fecha { get; set; }
        public string Importe { get; set; }
        public int AlquilerId { get; set; }
        public Alquiler alquiler { get; set; }

        public Inquilino inquilino { get; set; }

        public string Buscar { get; set; }
    }
}
