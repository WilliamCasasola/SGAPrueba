using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SGA.Models
{
    public class Estudiante : Persona
    {
        [Display(Name ="Generación")]
        [Required(ErrorMessage ="Necesita escoger una generación")]
        public string GeneracionId { set; get; }
        public virtual Generacion Generacion {set; get;}
        public virtual  ICollection<Matricula> matriculas { set; get; }

    }
}