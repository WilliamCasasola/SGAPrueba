using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SGA.Models
{
    public class Generacion
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required(ErrorMessage = "Nombre Requerido")]
        [Display(Name = "Nombre")]
        [StringLength(50,ErrorMessage = "Los nombres no pueden tener más de 50 caracteres..")]
        public string Id { set; get; }

        [Required(ErrorMessage = "Fecha requerida")]
        [Display(Name = "Fecha")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Fecha { set; get; }

        [Display(Name = "Imágen")]
        [RegularExpression(@"(?i).*\.(jpe?g|png)$", ErrorMessage = "Seleccione un archivo (png,jpg,jpeg).")]
        [StringLength(250, ErrorMessage = "El tamaño de la imágen es muy grande")]
        public string Foto { set; get; }

        public virtual ICollection<Curso> Cursos { set; get; }

        public virtual ICollection<Titulo> TitulosRequisito { get; set; }

    }
}