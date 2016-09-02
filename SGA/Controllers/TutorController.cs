using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SGA.DAL;
using SGA.Models;
using SGA.ViewModels;
using System.Data.Entity.Infrastructure;

namespace SGA.Controllers
{
    public class TutorController : Controller
    {
        private SGAContext db = new SGAContext();

        // GET: Tutor
        public ActionResult Index(string Id, string cursoID)
        {
            var viewModel = new DatosIndexTutor();

            viewModel.Tutores = db.Tutores
                .Include(t => t.Cursos.Select(c => c.Titulo))
                .Include(t => t.Cursos.Select(c => c.Matriculas))
                .Include(t => t.Cursos.Select(c => c.Matriculas.Select(m => m.Estudiante)))
                .OrderBy(t => t.Nombre);
            if (Id != null)
            {
                ViewBag.TutorID = Id;
                viewModel.Cursos = viewModel.Tutores.Where(i => i.Id == Id).Single().Cursos;
            }
            if (cursoID != null)
            {
                ViewBag.CursoID = cursoID;//Otra forma
                viewModel.Matriculas = viewModel.Cursos.Where(c => c.Id == cursoID).Single().Matriculas;
            }
            return View(viewModel);
        }

        // GET: Tutor/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tutor tutor = db.Tutores.Find(id);
            if (tutor == null)
            {
                return HttpNotFound();
            }
            return View(tutor);
        }

        // GET: Tutor/Create
        public ActionResult Create()
        {
            var tutor = new Tutor();
            tutor.Cursos = new List<Curso>();
            populateCursoAsignadoTutor(tutor);
            return View();
        }

        // POST: Tutor/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string[] cursosSeleccionados,[Bind(Include = "Id,Apellidos,Clave,Sexo,Identificacion,Profesion,Institucion,Fotografia,Estado,Nombre,Pais,Telefono,Correo,CorreoAlternativo,Direccion")] Tutor tutor)
        {
            if (cursosSeleccionados != null)
            {
                tutor.Cursos = new List<Curso>();
                foreach (var curso in cursosSeleccionados)
                {
                    var incluircurso = db.Cursos.Find(curso);
                    tutor.Cursos.Add(incluircurso);
                }

            }
            if (ModelState.IsValid)
            {
                tutor.FechaRegistro = DateTime.Now;
                db.Tutores.Add(tutor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            populateCursoAsignadoTutor(tutor);
            return View(tutor);
        }

        // GET: Tutor/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Tutor tutor = db.Tutores.Include(t => t.Cursos).Single(t => t.Id == id);
            populateCursoAsignadoTutor(tutor);
            if (tutor == null)
            {
                return HttpNotFound();
            }
            return View(tutor);
        }
        private void populateCursoAsignadoTutor(Tutor tutor)
        {
            var todosLosCursos = db.Cursos.Include(c => c.Titulo);
            var cursosTutor = new HashSet<string>(tutor.Cursos.Select(c => c.Id));
            var viewModel = new List<AsignarCursoTutor>();
            foreach (var curso in todosLosCursos)
            {
                AsignarCursoTutor act = new AsignarCursoTutor
                {
                    CursoID = curso.Id,
                    Titulo = curso.Titulo.Nombre,
                    Asignado = cursosTutor.Contains(curso.Id)

                };
                viewModel.Add(act);
            }
            ViewBag.Cursos = viewModel;
        }
        // POST: Tutor/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id,string[] cursosSeleccionados,[Bind(Include = "Id,Apellidos,Clave,Sexo,Identificacion,Profesion,Institucion,Fotografia,Estado,Nombre,Pais,Telefono,Correo,CorreoAlternativo,Direccion")] Tutor tutor)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tutorActualizar = db.Tutores
               .Include(i => i.Cursos)
               .Where(i => i.Id == id)
               .Single();

            if (TryUpdateModel(tutorActualizar, "",
               new string[] { "nombre", "apellidos" }))
            {
                try
                {
                    ActualizarCursosInstructor(cursosSeleccionados, tutorActualizar);

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException dex)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                     ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
              }
            }
            populateCursoAsignadoTutor(tutorActualizar);
            return View(tutorActualizar);
        }

        private void ActualizarCursosInstructor(string[] cursosSeleccionados, Tutor tutorActualizar)
        {
            if (cursosSeleccionados == null)
            {
                tutorActualizar.Cursos = new List<Curso>();
                return;
            }

            var cursoSeleccionadosHS = new HashSet<string>(cursosSeleccionados);
            var cursosInstructor = new HashSet<string>
                (tutorActualizar.Cursos.Select(c => c.Id));
            foreach (var course in db.Cursos)
            {
                if (cursoSeleccionadosHS.Contains(course.Id))
                {
                    if (!cursosInstructor.Contains(course.Id))
                    {
                        tutorActualizar.Cursos.Add(course);
                    }
                }
                else
                {
                    if (cursosInstructor.Contains(course.Id))
                    {
                        tutorActualizar.Cursos.Remove(course);
                    }
                }
            }
        }

        // GET: Tutor/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tutor tutor = db.Tutores.Find(id);
            if (tutor == null)
            {
                return HttpNotFound();
            }
            return View(tutor);
        }

        // POST: Tutor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Tutor tutor = db.Tutores.Find(id);
            db.Tutores.Remove(tutor);

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
