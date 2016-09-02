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
        public ActionResult Details(string id)
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
            return View(curso);
        }

        // GET: Curso/Create
        public ActionResult Create()
        {
            ViewBag.GeneracionId = new SelectList(db.Generacions, "Id", "Id");
            ViewBag.TituloId = new SelectList(db.Titulos, "Id", "Nombre");
            return View();
        }

        // POST: Curso/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FechaInicio,FechaFinal,CantidadEvaluaciones,Estado,TituloId,GeneracionId")] Curso curso)
        {
            if (ModelState.IsValid)
            {
                db.Cursos.Add(curso);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.GeneracionId = new SelectList(db.Generacions, "Id", "Foto", curso.GeneracionId);
            ViewBag.TituloId = new SelectList(db.Titulos, "Id", "Nombre", curso.TituloId);
            return View(curso);
        }

        // GET: Curso/Edit/5
        public ActionResult Edit(string id)
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
            ViewBag.GeneracionId = new SelectList(db.Generacions, "Id", "Foto", curso.GeneracionId);
            ViewBag.TituloId = new SelectList(db.Titulos, "Id", "Nombre", curso.TituloId);
            return View(curso);
        }

        // POST: Curso/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FechaInicio,FechaFinal,CantidadEvaluaciones,Estado,TituloId,GeneracionId")] Curso curso)
        {
            if (ModelState.IsValid)
            {
                db.Entry(curso).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GeneracionId = new SelectList(db.Generacions, "Id", "Foto", curso.GeneracionId);
            ViewBag.TituloId = new SelectList(db.Titulos, "Id", "Nombre", curso.TituloId);
            return View(curso);
        }

        // GET: Curso/Delete/5
        public ActionResult Delete(string id)
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
            return View(curso);
        }

        // POST: Curso/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
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
