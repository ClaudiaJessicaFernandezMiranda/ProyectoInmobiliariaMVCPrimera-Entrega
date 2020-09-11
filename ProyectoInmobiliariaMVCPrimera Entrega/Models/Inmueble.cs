using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoInmobiliariaMVCPrimera_Entrega.Models
{
    public class Inmueble
    {
        [Key]
        public int InmuebleId { get; set; }
        [Required]
        public string Direccion { get; set; }
        public int Ambientes { get; set; }
        public string Tipo { get; set; }
        public decimal Costo { get; set; }
        public decimal Superficie { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public int PropietarioId { get; set; }
        public Propietario Propietario { get; set; }
    }
}
