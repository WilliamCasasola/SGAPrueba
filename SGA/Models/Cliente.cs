using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SGA.Models
{
    public class Cliente
    {
        public string Id { set; get; }

        [Required(ErrorMessage = "Nombre Requerido")]
        [Display(Name = "Nombre")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$", ErrorMessage = "Los nombres solo pueden tener letras y la primera en mayúsucula.")]
        [StringLength(30, ErrorMessage = "Los nombres no pueden tener más de 30 carácteres..")]
        public string Nombre { set; get; }


        [Required(ErrorMessage = "País Requerido")]
        [Display(Name = "País")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$", ErrorMessage = "Los nombres de país solo pueden tener letras y la primera en mayúsucula.")]
        [StringLength(30, ErrorMessage = "Los nombres de país no pueden tener más de 30 carácteres..")]
        public string Pais { set; get; }


        [Required(ErrorMessage = "Teléfono Requerido")]
        [Display(Name = "Teléfono")]
        public string Telefono { set; get; }

        [Required(ErrorMessage = "Correo Requerido")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Correo no válido")]
        [Display(Name = "Correo")]
        public string Correo { set; get; }

        [Required(ErrorMessage = "Correo Alternativo Requerido")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Correo no válido")]
        [Display(Name = "Correo Alternativo")]
        public string CorreoAlternativo { set; get; }

        [Required(ErrorMessage = "Dirección Requerida")]
        [Display(Name = "Dirección")]
        [StringLength(10000, ErrorMessage = "Las direcciones no pueden tener más de 10000 carácteres..")]
        public string Direccion { set; get; }

        [Required(ErrorMessage = "Clave Requerida")]
        [Display(Name = "Clave")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Las claves no pueden tener menos de 6 carácteres..")]
        public string Clave { set; get; }

    }
}