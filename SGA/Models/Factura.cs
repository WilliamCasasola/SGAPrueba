using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SGA.Models
{
    public enum EstadoFactura {
        Cancelado, Pendiente, Congelado
    }
    public class Factura
    {
        [Display (Name="Número Factura")]
        public int Id { set; get; }

        [Required(ErrorMessage = "Fecha requerida")]
        [Display(Name = "Fecha")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Fecha { set; get; }

        [Required(ErrorMessage = "Cliente requerido")]
        [Display(Name = "Cliente")]
        public string ClienteId { set; get; }
        public virtual Cliente Cliente { set; get; }

        [Required(ErrorMessage = "Descripción requerida")]
        [Display(Name = "Descripción")]
        [StringLength(5000)]
        public string Descripcion { set; get; }

        [Required(ErrorMessage ="Estado Requerido")]
        [Display(Name ="Estado de Factura")]
        public EstadoFactura estado { set; get; }

        public virtual ICollection<EstudianteParaFactura> Detalles { set; get; }
    }
}