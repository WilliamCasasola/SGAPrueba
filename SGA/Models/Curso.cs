using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SGA.Models
{
    public enum Estado {
        [Display(Name = "Concluido")]
        Concluido,
        [Display(Name = "En Progreso")]
        Progreso,
        [Display(Name = "Próximamente")]
        Proximo
    }
    public class Curso
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public String Id { set; get; }

        [Required(ErrorMessage = "Fecha requerida")]
        [Display(Name = "Fecha de Inicio")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaInicio { set; get; }

        [Required(ErrorMessage = "Fecha requerida")]
        [Display(Name = "Fecha de Finalización")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaFinal { set; get; }

        [Required(ErrorMessage = "Cantidad requerida")]
        [Display(Name = "Cantidad de Evaluaciones")]
        public int CantidadEvaluaciones { set; get; }

        [Required(ErrorMessage = "Estado requerido")]
        [Display(Name = "Estado")]
        public bool Estado { set; get; }

        [Required(ErrorMessage = "Título requerido")]
        [Display(Name = "Título")]
        public String TituloId { set; get; }
        public virtual Titulo Titulo { set; get; }

        public virtual ICollection<Matricula> Matriculas { set; get; }

        public virtual ICollection<Tutor> Tutores { set; get; }

        [Required(ErrorMessage = "Generación requerida")]
        public string GeneracionId { set; get; }
        public virtual Generacion Generacion { set; get; }
    }
}