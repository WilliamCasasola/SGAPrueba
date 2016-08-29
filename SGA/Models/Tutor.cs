using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SGA.Models
{
    public class Tutor
    {
        public string TutorID { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        [StringLength(50)]
        public string nombre { get; set; }

        [Required]
        [Display(Name = "Apellidos")]
        [StringLength(50)]
        public string apellidos { get; set; }

     

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha Contratación")]
        public DateTime Fechacontratacion { get; set; }

        [Display(Name = "Nombre Completo")]
        public string nombreCompleto
        {
            get { return nombre + ", " + apellidos; }
        }

        public virtual ICollection<Curso> Cursos { get; set; }
       // public virtual OfficeAssignment OfficeAssignment { get; set; }
    }
}