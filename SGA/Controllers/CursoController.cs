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
using LumenWorks.Framework.IO.Csv;

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
        public ActionResult Create([Bind(Include = "FechaInicio,FechaFinal,CantidadEvaluaciones,Estado,TituloId,GeneracionId")] Curso curso)
        {
            if (ModelState.IsValid)
            {
                db.Cursos.Add(curso);
                db.SaveChanges();
                return RedirectToAction("Index");
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
        public ActionResult EditPost(int? id, HttpPostedFileBase upload)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var ActualizarCurso = db.Cursos.Find(id);
            if (TryUpdateModel(ActualizarCurso, "", new String[] {
                "FechaInicio,FechaFinal,CantidadEvaluaciones,Estado,TituloId,GeneracionId"}))
            {
                try
                {

                 


                    if (upload != null && upload.ContentLength > 0)
                    {
                        
                        if (upload.FileName.EndsWith(".csv"))
                        {
                            using (StreamReader CsvReader = new StreamReader(upload.InputStream))
                            {
                                string inputLine = "";
                                string[] persona;
                                inputLine = CsvReader.ReadLine();
                                    string[] headers = inputLine.Split(new char[] { ';' });
                                while ((inputLine = CsvReader.ReadLine()) != null)
                                {
                                    persona = inputLine.Split(new char[] { ';' });
                                    for (int i = 6; i < 10; i++)
                                    {
                                        Nota n = new Nota { Tipo = headers[i],Valor=Double.Parse(persona[i]) };
                                    }
                                }
                                CsvReader.Close();
                                
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("File", "This file format is not supported");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("File", "Please Upload Your file");
                    }

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /*/dex*/)
                {
                    ModelState.AddModelError("", "No se pudo realizar la acción. Vulva a intentarlo, si el problema persiste contante al administrador del sistema");
                }
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
