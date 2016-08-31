using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SGA.Models
{
    public class Estudiante:Persona
    {
        public virtual  ICollection<Matricula> matriculas { set; get; }

    }
}