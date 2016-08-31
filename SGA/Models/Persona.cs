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



    }
}