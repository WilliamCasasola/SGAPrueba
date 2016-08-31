using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SGA.Models
{
    public class EstudianteParaFactura
    {
        public int Id { set; get; }

        [Display(Name = "Nombre de Estudiante")]
        public String EstudianteId { set; get; }
        public virtual Estudiante Estudiante { set; get; }

        public virtual ICollection<Titulo> Titulos { set; get; }

    }
}