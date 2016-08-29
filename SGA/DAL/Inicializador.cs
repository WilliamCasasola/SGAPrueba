using SGA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace SGA.DAL
{
    public class Inicializador : System.Data.Entity.DropCreateDatabaseIfModelChanges<SGAContext>
    {
        protected override void Seed(SGAContext context)
        {
            var Estudiantes = new List<Estudiante>
            {
            new Estudiante{nombre="Carson",apellidos="Alexander",diaMatricula=DateTime.Parse("2005-09-01")},
            new Estudiante{nombre="Meredith",apellidos="Alonso",diaMatricula=DateTime.Parse("2002-09-01")},
            new Estudiante{nombre="Arturo",apellidos="Anand",diaMatricula=DateTime.Parse("2003-09-01")},
            new Estudiante{nombre="Gytis",apellidos="Barzdukas",diaMatricula=DateTime.Parse("2002-09-01")},
            new Estudiante{nombre="Yan",apellidos="Li",diaMatricula=DateTime.Parse("2002-09-01")},
            new Estudiante{nombre="Peggy",apellidos="Justice",diaMatricula=DateTime.Parse("2001-09-01")},
            new Estudiante{nombre="Laura",apellidos="Norman",diaMatricula=DateTime.Parse("2003-09-01")},
            new Estudiante{nombre="Nino",apellidos="Olivetto",diaMatricula=DateTime.Parse("2005-09-01")}
            };

            Estudiantes.ForEach(s => context.Estudiantes.Add(s));
            context.SaveChanges();
           /* var Cursos = new List<Curso>
            {
            new Curso{CursoID="1050",titulo="Chemistry",},
            new Curso{CursoID="4022",titulo="Microeconomics",},
            new Curso{CursoID="4041",titulo="Macroeconomics",},
            new Curso{CursoID="1045",titulo="Calculus",},
            new Curso{CursoID="3141",titulo="Trigonometry",},
            new Curso{CursoID="2021",titulo="Composition",},
            new Curso{CursoID="2042",titulo="Literature",}
            };
            Cursos.ForEach(s => context.Cursos.Add(s));
            context.SaveChanges();*/
            var Matriculas = new List<Matricula>
            {
            new Matricula{EstudianteID="1",CursoID="1050",nota=Nota.A},
            new Matricula{EstudianteID="1",CursoID="4022",nota=Nota.C},
            new Matricula{EstudianteID="1",CursoID="4041",nota=Nota.B},
            new Matricula{EstudianteID="2",CursoID="1045",nota=Nota.B},
            new Matricula{EstudianteID="2",CursoID="3141",nota=Nota.F},
            new Matricula{EstudianteID="2",CursoID="2021",nota=Nota.F},
            new Matricula{EstudianteID="3",CursoID="1050"},
            new Matricula{EstudianteID="4",CursoID="1050",},
            new Matricula{EstudianteID="4",CursoID="4022",nota=Nota.F},
            new Matricula{EstudianteID="5",CursoID="4041",nota=Nota.C},
            new Matricula{EstudianteID="6",CursoID="1045"},
            new Matricula{EstudianteID="7",CursoID="3141",nota=Nota.A},
            };
            Matriculas.ForEach(s => context.Matriculas.Add(s));
            context.SaveChanges();
        }
    }
}