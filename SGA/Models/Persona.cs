using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SGA.Models
{
    public class Persona : Cliente
    {
        [Required(ErrorMessage = "Apellidos Requerido")]
        [Display(Name = "Apellidos")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$", ErrorMessage = "Los apellidos solo pueden tener letras y la primera en mayúsucula.")]
        [StringLength(60, ErrorMessage = "Los apellidos no pueden tener más de 60 caracteres..")]
        public string Apellidos { set; get; }

        [Required(ErrorMessage = "Clave Requerida")]
        [Display(Name = "Clave")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Las claves no pueden tener menos de 6 carácteres..")]
        public string Clave { set; get; }

        public bool Sexo { set; get; }

        [Display(Name = "Ruta de Foto")]
        public HttpPostedFile Identificacion { set; get; }

        [StringLength(60, ErrorMessage = "El texto no puede tener más de 60 caracteres..")]
        public string Profecion { set; get; }

        [StringLength(60, ErrorMessage = "El texto no puede tener más de 60 caracteres..")]
        public string Institucion { set; get; }

        [Display(Name = "Ruta de Foto")]
        public HttpPostedFile Fotografia { set; get; }

        public bool Estado { set; get;


    }
}