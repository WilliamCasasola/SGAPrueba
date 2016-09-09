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
        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$", ErrorMessage = "Los nombres solo pueden tener letras y la primera en mayúsucula.")]
        [StringLength(50,ErrorMessage = "Los nombres no pueden tener más de 50 caracteres..")]
        public string Id { set; get; }

        [Required(ErrorMessage = "Fecha requerida")]
        [Display(Name = "Fecha")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Fecha { set; get; }

        [Display(Name = "Ruta de Foto")]
        public string Foto { set; get; }

        public virtual ICollection<Curso> Cursos { set; get; }

    }
}