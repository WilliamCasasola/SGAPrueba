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
        [StringLength(30, ErrorMessage = "Los nombres no pueden tener más de 30 carácteres..")]
        public string Nombre { set; get; }


        [Required(ErrorMessage = "País Requerido")]
        [Display(Name = "País")]
        [StringLength(30, ErrorMessage = "Los nombres de país no pueden tener más de 30 carácteres..")]
        public string Pais { set; get; }


        [Required(ErrorMessage = "Teléfono Requerido")]
        [StringLength(30, MinimumLength = 7, ErrorMessage = "Cantidad incorrecta de números")]
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

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha Registro")]
        public DateTime FechaRegistro { get; set; }

    }
}