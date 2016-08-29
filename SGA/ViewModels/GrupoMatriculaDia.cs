using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SGA.ViewModels
{
    public class GrupoMatriculaDia
    {
        [DataType(DataType.Date)]
        public DateTime? diaMatricula { set; get; }
        public int contadorEstudiantes { set; get; }
    }
}