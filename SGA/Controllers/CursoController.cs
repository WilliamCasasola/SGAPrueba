/*using System;
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

namespace SGA.Controllers
{
    public class CursoController : Controller
    {
        private SGAContext db = new SGAContext();

        // GET: Curso
        public ActionResult Index()
        {
            var cursos = db.Cursos.Include(c => c.Titulo);
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
            PopulateTitulosDropDownList();
            return View();
        }

        // POST: Curso/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CursoID,TituloID")] Curso curso)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Cursos.Add(curso);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }
            catch (RetryLimitExceededException dex)
            {
                ModelState.AddModelError("", "No se pudo realizar la acción. Iintente de nuevo, si el problema persiste comuniquese con el administrador del sistema ");
            }
            PopulateTitulosDropDownList(curso.Id);

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
            PopulateTitulosDropDownList(curso.Id);
            return View(curso);
        }

        // POST: Curso/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var cursoActualizar = db.Cursos.Find(id);
            if (TryUpdateModel(cursoActualizar, "", new string[] { "Codigo", "TituloID" }))
            {
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException dex)
                {
                    ModelState.AddModelError("", "No se pudo realizar la acción. Iintente de nuevo, si el problema persiste comuniquese con el administrador del sistema ");
                }
            }
            PopulateTitulosDropDownList(cursoActualizar.Id);
            return View(cursoActualizar);
        }

        private void PopulateTitulosDropDownList(object tituloSeleccionado = null)
        {
            var tituloQuery = from d in db.Titulos
                              orderby d.Id
                              select d;
            ViewBag.TituloID = new SelectList(tituloQuery, "TituloID", "nombre", tituloSeleccionado);
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
*/