using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SGA.Models
{
    public class Estudiante
    {
        [Required]
        [Display(Name ="Carnet")]
        public String ID { set; get; }

        [Required]
        [Display(Name ="Nombre")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$", ErrorMessage = "Los nombres solo pueden tener letras y la primera en mayúsucula.")]
        [StringLength(20, ErrorMessage = "Los nombres no pueden tener más de 20 caracteres..")]
        public String nombre { set; get; }

        [Required]
        [Display(Name = "Apellidos")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$",ErrorMessage = "Los apellidos solo pueden tener letras y la primera en mayúsucula.")]
        [StringLength(40, ErrorMessage = "Los apellidos no pueden tener más de 40 caracteres.")]
        public String apellidos { set; get; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:dd-MM-yyyy}", ApplyFormatInEditMode =true)]
        [Display(Name = "Fecha Matricula")]
        public DateTime diaMatricula { set; get; }

        public virtual  ICollection<Matricula> matriculas { set; get; }
    }
}