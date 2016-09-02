using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SGA.Models
{
    public class Titulo
    {
        [Display(Name="Código")]
        public string Id { set; get; }

        [Required(ErrorMessage = "Nombre requerido")]
        [Display(Name = "Nombre")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$", ErrorMessage = "Los nombres solo pueden tener letras y la primera en mayúsucula.")]
        [StringLength(50, ErrorMessage = "Los nombres no pueden tener más de 50 carácteres..")]
        public string Nombre { set; get; }

        [Display(Name = "Ruta de Foto")]
        public string Foto { set; get; }

        [Required(ErrorMessage = "Precio requerido")]
        [Display(Name = "Precio")]
        public double Precio { set; get; }

        public virtual ICollection<Curso> cursos { set; get; }
    }
}