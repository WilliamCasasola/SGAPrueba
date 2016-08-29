using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SGA.Models
{
    public enum Nota {
    A,B,C,D,F
    }
    
    public class Matricula
    {
        public String ID { set; get; }
        public String CursoID { set; get; }
        public String EstudianteID { set; get; }

        [DisplayFormat(NullDisplayText="Sin calificar")]
        public Nota? nota { set; get; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha Matricula")]
        public DateTime diaMatricula { set; get; }

        public virtual Curso Curso { set; get; }
        public virtual Estudiante Estudiante { set; get; }
    }
}