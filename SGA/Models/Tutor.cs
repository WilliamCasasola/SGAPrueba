using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SGA.Models
{
    public class Tutor:Persona
    {

        public virtual ICollection<Curso> Cursos { get; set; }
       // public virtual OfficeAssignment OfficeAssignment { get; set; }
    }
}