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
        [StringLength(60, ErrorMessage = "Los apellidos no pueden tener más de 60 caracteres..")]
        public string Apellidos { set; get; }

        [Required(ErrorMessage = "Clave Requerida")]
        [Display(Name = "Clave")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Las claves no pueden tener menos de 6 carácteres..")]
        public string Clave { set; get; }

        public bool Sexo { set; get; }

        [Display(Name = "Documento Oficial(Pasaporte)")]
        [RegularExpression(@"(?i).*\.(gif|jpe?g|png|bmp)$", ErrorMessage = "Seleccione un archivo (png,jpg,jpeg).")]
        [StringLength(20, ErrorMessage = "El tamaño de la imágen es muy grande")]
        public string Identificacion { set; get; }

        [Display(Name = "Profesión")]
        [StringLength(60, ErrorMessage = "El texto no puede tener más de 60 caracteres..")]
        public string Profesion { set; get; }

        [StringLength(60, ErrorMessage = "El texto no puede tener más de 60 caracteres..")]
        public string Institucion { set; get; }

        [Display(Name = "Foto")]
        [RegularExpression(@"(?i).*\.(jpe?g|png)$", ErrorMessage = "Seleccione un archivo (png,jpg,jpeg).")]
        [StringLength(20, ErrorMessage = "El tamaño de la imágen es muy grande")]
        public string Fotografia { set; get; }

        [Display(Name ="Activo")]
        public bool Estado { set; get; }


    }
}