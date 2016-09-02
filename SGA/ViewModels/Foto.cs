using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SGA.ViewModels
{
    public class Foto
    {
        [Required, FileExtensions(Extensions = "png | jpg | jpeg", ErrorMessage = "Seleccione un archivo (png,jpg,jpeg).")]
        public HttpPostedFileBase File { get; set; }
    }
}