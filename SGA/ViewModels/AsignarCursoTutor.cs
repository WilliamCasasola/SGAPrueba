using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGA.ViewModels
{
    public class AsignarCursoTutor
    {
        public int CursoID { set; get; }
        public int GeneracionId { set; get; }
        public string Titulo { set; get; }
        public bool Asignado { set; get; }
    }
}