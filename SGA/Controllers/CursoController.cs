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
using System.Data.Entity.Infrastructure;
using System.IO;
//using LumenWorks.Framework.IO.Csv;
using System.Text.RegularExpressions;
using System.Data.Entity.Validation;
using System.Globalization;

namespace SGA.Controllers
{
    public class CursoController : Controller
    {
        private SGAContext db = new SGAContext();

        // GET: Curso
        public ActionResult Index()
        {
            var cursos = db.Cursos.Include(c => c.Generacion).Include(c => c.Titulo);
            return View(cursos.ToList());
        }

        // GET: Curso/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Curso curso = db.Cursos.Include(c => c.Generacion)
                 .Include(c => c.Titulo).Single(c => id == c.Id);
           
            if (curso == null)
            {
                return HttpNotFound();
            }
            ViewBag.Estado = DateTime.Now < curso.FechaInicio ? "Por Iniciar" : curso.FechaFinal < DateTime.Now ? "Concluido" : "En progreso";
                 curso.Matriculas = db.Matriculas.Include(m => m.Estudiante).Include(m => m.Curso).Where(m => m.CursoID == id).ToList();
            return View(curso);
        }

        // GET: Curso/Create
        public ActionResult Create()
        {
            ViewBag.GeneracionId = new SelectList(db.Generacions.OrderBy(g => g.Id), "Id", "Id");
            ViewBag.TituloId = new SelectList(db.Titulos.OrderBy(t => t.Nombre), "Id", "Nombre");
            return View();
        }

        // POST: Curso/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FechaInicio,FechaFinal,CantidadEvaluaciones,TituloId,GeneracionId")] Curso curso)
        {
            if (curso.FechaInicio < curso.FechaFinal)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        db.Cursos.Add(curso);
                        db.SaveChanges();
                        TempData["mensaje"] = "Se registró el curso satisfactoriamente";
                        return RedirectToAction("Index");
                    }
                    catch (DbEntityValidationException mex)
                    {
                        TempData["mensajeError"] = "No se pudo realizar la acción. Trate nuevamente, si el problema persiste contacte al administrador del sistema.";
                    }
                    catch (DbUpdateException e)
                    {
                        TempData["mensajeError"] = "No se pudo realizar la acción. Compruebe si ya existe un curso registrado con el mismo código o si existe la generación o título específicado , si el problema persiste contacte al administrador del sistema.";
                    }
                    catch (Exception e)
                    {
                        TempData["mensajeError"] = "No se pudo realizar la acción. Trate nuevamente, si el problema persiste contacte al administrador del sistema.";
                    }
                }
            }
            else {
                TempData["mensajeError"] = "No se pudo realizar la acción. Compruebe que la fecha inicial no sea mayor a la fecha Final";
            }
            ViewBag.GeneracionId = new SelectList(db.Generacions, "Id", "Id", curso.GeneracionId);
            ViewBag.TituloId = new SelectList(db.Titulos, "Id", "Nombre", curso.TituloId);
            return View(curso);
        }

        // GET: Curso/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Curso curso = db.Cursos.Find(id);
            if (curso == null)
            {
                return HttpNotFound();
            }
            generarSelect(curso);            
            return View(curso);
        }

         
        // POST: Curso/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost([Bind(Include = "Id,FechaInicio,FechaFinal,CantidadEvaluaciones,TituloId,GeneracionId")] Curso ActualizarCurso, HttpPostedFileBase upload)
        {                      
                               
                    if (upload != null && upload.ContentLength > 0)
                    {
                        
                        if (upload.FileName.EndsWith(".csv"))
                        {                         
                            using (StreamReader CsvReader = new StreamReader(upload.InputStream))
                            {
                            try
                            {
                                var numeroTarea = new Dictionary<int, int>(); ;
                                Regex patrontarea = new Regex(@"\d+$");
                                Regex patronCorreo = new Regex(@"correo$");
                                Regex patronNotaFinal = new Regex(@"^Total del curso$");                                
                                string inputLine = "";
                                string[] persona;
                                int correo= -1;
                                int NotaFinalPosicion = -1;
                                inputLine = CsvReader.ReadLine();
                                    string[] headers = inputLine.Split(new char[] { ';' });
                                foreach (var header in headers.Select((valor, i) => new { i, valor })) {
                                    if (patronCorreo.IsMatch(header.valor))
                                        correo = header.i;
                                    if (patronNotaFinal.IsMatch(header.valor))
                                        NotaFinalPosicion = header.i;
                                    if (patrontarea.IsMatch(header.valor))
                                    {
                                        numeroTarea.Add(int.Parse(patrontarea.Match(header.valor).Value),header.i);
                                    }
                                }

                                var indicesOrdenados= numeroTarea.OrderBy(n => n.Key).Select(n=>n.Value).ToList();

                                while ((inputLine = CsvReader.ReadLine()) != null)
                                {
                                    persona = inputLine.Split(new char[] { ';' });
                                    string temp = persona[correo];
                                    string codigoestudiante = db.Estudiantes.Where(e => e.Correo.Equals(temp)).Select(e => e.Id).Single();
                                    Matricula matricula = new Matricula {
                                        Calificaciones = new List<Nota>(),
                                        NotaFinal =Double.Parse(persona[NotaFinalPosicion]),
                                        EstudianteID = codigoestudiante,
                                        CursoID=ActualizarCurso.Id  //id.Value por que lo que le llega a la función es un int? !!                            
                                    };
                                    foreach (var i in indicesOrdenados)
                                    {
                                        double nota = (persona[i].Equals("-")) ? 0 :  Double.Parse(persona[i]);//Preguntar 100/4=33,6?
                                        matricula.Calificaciones.Add(new Nota { Tipo = new Regex(@"Tarea No\.\d+$").Match(headers[i]).Value,Valor=nota });
                                    }
                                    db.Matriculas.Add(matricula);
                                }
                            
                                db.SaveChanges();
                                TempData["mensaje"] = "Se registraron las matriculas satisfactoriamente";
                            }
                            catch (DbEntityValidationException mex)
                            {
                                TempData["mensajeError"] = "No se pudo realizar la acción. Trate nuevamente, si el problema persiste contacte al administrador del sistema.";
                            }
                            catch (DbUpdateException e)
                            {
                                TempData["mensajeError"] = "No se pudo realizar la acción. Compruebe que todos los estudiantes se encuentren registrados o si ya estaba matriculado en el curso , si el problema persiste contacte al administrador del sistema.";
                            }
                            catch (Exception e)
                            {
                                TempData["mensajeError"] = "No se pudo realizar la acción. Trate nuevamente, si el problema persiste contacte al administrador del sistema.";
                            }
                            CsvReader.Close();
                                
                            }
                        }
                        else
                        {
                        TempData["mensajeError"] = "El archivo no tiene el formato requerido";
                    }
                }
            if (ActualizarCurso.FechaInicio < ActualizarCurso.FechaFinal)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        db.Entry(ActualizarCurso).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["mensaje"] = "Se registraron los cambios en el curso satisfactoriamente";
                        return RedirectToAction("Index");
                    }
                }
                catch (DbEntityValidationException mex)
                {
                    TempData["mensajeError"] = "No se pudo realizar la acción. Trate nuevamente, si el problema persiste contacte al administrador del sistema.";
                }
                catch (DbUpdateException e)
                {
                    TempData["mensajeError"] = "No se pudo realizar la acción. Compruebe si ya existe un curso registrado con el mismo código o si existe la generación o título específicado , si el problema persiste contacte al administrador del sistema.";
                }
                catch (Exception e)
                {
                    TempData["mensajeError"] = "No se pudo realizar la acción. Trate nuevamente, si el problema persiste contacte al administrador del sistema.";
                }
            }
            else {
                TempData["mensajeError"] = "No se pudo realizar la acción. Compruebe que la fecha inicial no sea mayor a la fecha Final";
            }
            
            generarSelect(ActualizarCurso);
            return View(ActualizarCurso);
        }

        private void generarSelect(Curso curso =null) {
            ViewBag.GeneracionId = new SelectList(db.Generacions.OrderBy(g=>g.Id), "Id", "Id", curso.GeneracionId);
            ViewBag.TituloId = new SelectList(db.Titulos.OrderBy(t=>t.Nombre), "Id", "Nombre", curso.TituloId);
        }
        // GET: Curso/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Curso curso = db.Cursos.Include(c =>  c.Generacion)
                .Include(c=>c.Titulo).Single(c => id == c.Id);
            if (curso == null)
            {
                return HttpNotFound();
            }
            return View(curso);
        }

        // POST: Curso/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Curso curso = db.Cursos.Find(id);
            db.Cursos.Remove(curso);
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
