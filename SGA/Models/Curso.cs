using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SGA.Models
{
    public class Curso
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public String CursoID { set; get; }
        
        public String TituloID { set; get; }
        public virtual Titulo Titulo { set; get; }
        public virtual ICollection<Matricula> Matriculas { set; get; }
        public virtual ICollection<Tutor> Tutores { set; get; }
        
    }
}