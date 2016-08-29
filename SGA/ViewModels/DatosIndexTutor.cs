using SGA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGA.ViewModels
{
    public class DatosIndexTutor
    {
        public IEnumerable<Tutor> Tutores { get; set; }
        public IEnumerable<Curso> Cursos { get; set; }
        public IEnumerable<Matricula> Matriculas { get; set; }
    }
}