using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGA.ViewModels
{
    public class AsignarTituloEstudiante
    {
        public string TituloId { get; set; }
        public double Precio { get; set; }
        public string Nombre { get; set; }
        public bool Asignado { get; set; }
    }
}