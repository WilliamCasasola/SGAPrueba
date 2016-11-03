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
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;

namespace SGA.Controllers
{
    public class MatriculaController : Controller
    {
        private SGAContext db = new SGAContext();
        private static ICollection<Nota> notas;
        private static System.Uri antiguauri;
        

        // GET: Matricula
        public ActionResult Index()
        {
            CursoNota curosNota = new CursoNota();
            curosNota.Matriculas=  db.Matriculas
                .Include(m => m.Curso.Titulo)
                .Include(m => m.Estudiante).ToList();
            //curosNota.Notas=db.Matriculas.Select(m => m.Calificaciones.Select(c => c.Valor).Sum() / m.Curso.CantidadEvaluaciones).ToList();
            return View(curosNota);
        }

        // GET: Matricula/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Matricula matricula = db.Matriculas.Include(m => m.Calificaciones).Include(m=>m.Estudiante).Include(m=>m.Curso.Titulo).Single(m=>m.ID==id);
            if (matricula == null)
            {
                return HttpNotFound();
            }
            return View(matricula);
        }

        private void crearSelect(Matricula matricula) {
            ViewBag.CursoID = new SelectList(db.Cursos, "Id", "TituloId", matricula.CursoID);
            ViewBag.EstudianteID = new SelectList(db.Clientes.SqlQuery("SELECT * FROM Cliente WHERE Discriminator = 'Estudiante'"), "Id", "Nombre", matricula.EstudianteID);
        }

        // GET: Matricula/Create
        public ActionResult Create(string Estudianteid)
        {
            DateTime d = DateTime.Today;
            d = d.AddMonths(-2);
            ViewBag.CursoID = new SelectList(db.Cursos.Where(c=>c.FechaFinal>d), "Id", "TituloId");

            if (Estudianteid != null)
            {
                ViewBag.EstudianteID = new SelectList(db.Estudiantes.SqlQuery("SELECT * FROM Cliente WHERE Discriminator = 'Estudiante'"), "Id", "Nombre", Estudianteid);
                CursoNota curosNota = new CursoNota();
                curosNota.Matriculas = db.Matriculas
                    .Include(m => m.Curso.Titulo)
                    .Include(m => m.Estudiante).Where(m => m.EstudianteID == Estudianteid).ToList();
                //curosNota.Notas = db.Matriculas.Where(m => m.EstudianteID == Estudianteid).Select(m => m.Calificaciones.Select(c => c.Valor).Sum() / m.Curso.CantidadEvaluaciones).ToList();
                var generacionEstudiante = db.Estudiantes.Where(e => e.Id == Estudianteid).Select(e => e.GeneracionId).Single();
                var requisitos = db.Database.SqlQuery<string>("SELECT Titulo_Id FROM titulogeneracion WHERE Generacion_Id = @gen ", new MySqlParameter("@gen", generacionEstudiante));
                int contador = 0;
                foreach(var item in curosNota.Matriculas)//.Zip(curosNota.Notas, (a, b) => new { matricula = a, nota = b }))
                {

                    if (requisitos.Contains(item.Curso.TituloId)) {
                        if (item.NotaFinal >= 70)
                            contador++;
                    }
                }
                int cantidadRequisitos = requisitos.Count();
                if (contador == cantidadRequisitos)
                    ViewBag.PuedeGraduarse = "Si";
                else
                    ViewBag.PuedeGraduarse = "No puede, necesita pasar "+ (cantidadRequisitos-contador) + " curso(s) más";

                if (contador >= (cantidadRequisitos - 2))
                    ViewBag.PuedeProyecto = "Si";
                else
                    ViewBag.PuedeProyecto = "No puede, necesita pasar " + (cantidadRequisitos - contador - 2) + " curso(s) más";

                return View(curosNota);
            }
            else {
                ViewBag.EstudianteID = new SelectList(db.Estudiantes.SqlQuery("SELECT * FROM Cliente WHERE Discriminator = 'Estudiante'"), "Id", "Nombre");
                return View();
            }



        }

        // POST: Matricula/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CursoID,EstudianteID")] Matricula matricula)
        {//No dejar que se matricule en el mismo curso 2 veces
            if (ModelState.IsValid)
            {
                try
                {
                    db.Matriculas.Add(inicializar(matricula));
                    db.SaveChanges();
                    TempData["mensaje"] = "Se registró la matricula satisfactoriamente";
                    return RedirectToAction("Index");
                }
                catch (DbEntityValidationException mex)
                {
                    TempData["mensajeError"] = "No se pudo realizar la acción. Trate nuevamente, si el problema persiste contacte al administrador del sistema.";
                }
                catch (DbUpdateException e)
                {
                    TempData["mensajeError"] = "No se pudo realizar la acción. Compruebe que el estudiante se encuentre registrado o si ya estaba matriculado en el curso, si el problema persiste contacte al administrador del sistema.";
                }
                catch (Exception e)
                {
                    TempData["mensajeError"] = "No se pudo realizar la acción. Trate nuevamente, si el problema persiste contacte al administrador del sistema.";
                }
            }

            crearSelect(matricula);
            return View(matricula);
        }

        // GET: Matricula/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            antiguauri = System.Web.HttpContext.Current.Request.UrlReferrer;

            Matricula matricula = db.Matriculas.Include(m=>m.Calificaciones).Where(m=>m.ID==id).Single();
            notas = matricula.Calificaciones;
            if (matricula == null)
            {
                return HttpNotFound();
            }
            crearSelect(matricula);
            return View(matricula);
        }

        private Matricula inicializar(Matricula matricula) {
            int c = db.Cursos.Find(matricula.CursoID).CantidadEvaluaciones;
            matricula.Calificaciones = new List<Nota>();
            if (c == 1)
            {
                matricula.Calificaciones.Add(new Nota { Valor = 0, Tipo = "Trabajo Final" });
            }
            else
            {
                for (int i = 0; i < c; i++)
                {
                    matricula.Calificaciones.Add(new Nota { Valor = 0, Tipo = "Tarea "+ (i+1) });
                }
            }
            matricula.DiaMatricula = DateTime.Now;
            return matricula;
        }

        // POST: Matricula/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CursoID,EstudianteID,DiaMatricula,NotaFinal")] Matricula matricula,  string[] valores, string[] tipos)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(matricula).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["mensaje"] = "Se registraron los cambios en la matricula satisfactoriamente";
                    Dispose(true);
                }
                catch (DbEntityValidationException mex)
                {
                    TempData["mensajeError"] = "No se pudo realizar la acción. Trate nuevamente, si el problema persiste contacte al administrador del sistema.";
                }
                catch (DbUpdateException e)
                {
                    TempData["mensajeError"] = "No se pudo realizar la acción. Compruebe que el estudiante se encuentre registrado o si ya estaba matriculado en el curso, si el problema persiste contacte al administrador del sistema."; 
                }
                catch (Exception e)
                {
                    TempData["mensajeError"] = "No se pudo realizar la acción. Trate nuevamente, si el problema persiste contacte al administrador del sistema.";
                }
                db = new SGAContext();
                foreach (var item in notas.Select((nota,i)=>new {Indice=i, Nota=nota })) {
                    item.Nota.Tipo = tipos[item.Indice];
                    double i = 0;
                    if (Double.TryParse(valores[item.Indice], out i))
                        item.Nota.Valor = Convert.ToDouble(valores[item.Indice]);
                    else {
                        TempData["mensajeError"] = "No se pudo realizar la acción. Ingrese otro valor en vez de este: \""+ valores[item.Indice]+"\"";
                        matricula.Calificaciones = notas;
                        crearSelect(matricula);
                        return View(matricula);
                        }
                    if (ModelState.IsValid)
                    {
                        db.Entry(item.Nota).State = EntityState.Modified;
                    }
                }
                try
                {                    
                        db.SaveChanges();
                        TempData["mensaje"] = "Se registraron los cambios en las notas satisfactoriamente";
                        if (new Regex("/Tutor/Index/.+$").IsMatch(antiguauri.AbsolutePath))
                            return Redirect(antiguauri.AbsoluteUri);
                        return RedirectToAction("Index");
                    
                }
                catch (DbEntityValidationException mex)
                {
                    TempData["mensajeError"] = "No se pudo realizar la acción. Trate nuevamente, si el problema persiste contacte al administrador del sistema.";
                }
                catch (DbUpdateException e)
                {
                    TempData["mensajeError"] = "No se pudo realizar la acción. Compruebe que las notas están bien digitadas o su respectiva identificación, si el problema persiste contacte al administrador del sistema.";
                }
                catch (Exception e)
                {
                    TempData["mensajeError"] = "No se pudo realizar la acción. Trate nuevamente, si el problema persiste contacte al administrador del sistema.";
                }
               
            }
            matricula.Calificaciones = notas;
            crearSelect(matricula);
            return View(matricula);
        }

        // GET: Matricula/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Matricula matricula = db.Matriculas.Find(id);
            if (matricula == null)
            {
                return HttpNotFound();
            }
            return View(matricula);
        }

        // POST: Matricula/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Matricula matricula = db.Matriculas.Find(id);
            db.Matriculas.Remove(matricula);
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
