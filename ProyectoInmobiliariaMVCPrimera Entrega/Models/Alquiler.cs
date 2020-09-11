using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoInmobiliariaMVCPrimera_Entrega.Models
{
    public class Alquiler
    {
        [Key]
        public int AlquilerId { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaAlta { get; set; }
        public DateTime FechaBaja { get; set; }
        public string Monto { get; set; }
        public int InmuebleId { get; set; }
        public Inmueble inmueble { get; set; }
        public int InquilinoId { get; set; }
        public Inquilino inquilino { get; set; }
    }
}