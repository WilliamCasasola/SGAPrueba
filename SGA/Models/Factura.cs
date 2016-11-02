using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SGA.Models
{
   
    public class Factura
    {
        [Display (Name="Número Factura")]
        public int Id { set; get; }

        [Required(ErrorMessage = "Fecha requerida")]
        [Display(Name = "Fecha")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
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
        public bool estado { set; get; }

        [Required(ErrorMessage = "Valor de monto cancelado requerido")]
        [Display(Name = "Monto cancelado")]
        public double TotalCancelado { set; get; }

        [Display(Name = "Comprobante")]
        [RegularExpression(@"(?i).*\.(gif|jpe?g|png|bmp)$", ErrorMessage = "Seleccione un archivo (png,jpg,jpeg).")]
        [StringLength(250, ErrorMessage = "El tamaño de la imágen es muy grande")]
        public string Comprobante { set; get; }

        public virtual ICollection<EstudianteParaFactura> Detalles { set; get; }
    }
}