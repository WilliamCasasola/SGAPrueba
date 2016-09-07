using SGA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGA.ViewModels
{
    public class CursoNota
    {
        public IEnumerable<Matricula>  Matriculas { set; get; }
        public IEnumerable<double> Notas { set; get; }
    }
}