using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SGA.Models
{
    public class Titulo
    {
        [Required]
        [Display(Name ="Titulo")]
        public string TituloID { set; get; }
        public string nombre { set; get; }
        public string foto { set; get; }
        public double precio { set; get; }


        public virtual ICollection<Curso> cursos { set; get; }
    }
}