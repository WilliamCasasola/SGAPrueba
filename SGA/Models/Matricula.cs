using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SGA.Models
{
    public class Nota {
        public int Id { set; get; }
        public double Valor { set; get; }
        public string Tipo { set; get; }
        public int MatriculaId { set; get; }
        public virtual Matricula Matricula { set; get; }
    }
    
    public class Matricula
    {
        public int ID { set; get; }
        public int CursoID { set; get; }
        public double NotaFinal { set; get; }
        public String EstudianteID { set; get; }

//[DisplayFormat(NullDisplayText="Sin calificar")]
        public virtual ICollection<Nota> Calificaciones { set; get; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha Matricula")]
        public DateTime DiaMatricula { set; get; }

        public virtual Curso Curso { set; get; }
        public virtual Estudiante Estudiante { set; get; }
    }
}