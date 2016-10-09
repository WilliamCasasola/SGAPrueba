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
    public class EstudianteParaFacturaController : Controller
    {
        private SGAContext db = new SGAContext();

        // GET: EstudianteParaFactura
        public ActionResult Index()
        {
            var estudianteParaFacturas = db.EstudianteParaFacturas.Include(e => e.Estudiante);
            return View(estudianteParaFacturas.ToList());
        }

        // GET: EstudianteParaFactura/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EstudianteParaFactura estudianteParaFactura = db.EstudianteParaFacturas.Find(id);
            if (estudianteParaFactura == null)
            {
                return HttpNotFound();
            }
            return View(estudianteParaFactura);
        }

        // GET: EstudianteParaFactura/Create
        public ActionResult Create()
        {
            ViewBag.EstudianteId = new SelectList(db.Estudiantes, "Id", "Apellidos");
            ViewBag.TituloId = new SelectList(db.Titulos, "Id", "Id"+" " +"Nombre");

            return View();
        }

        // POST: EstudianteParaFactura/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,EstudianteId")] EstudianteParaFactura estudianteParaFactura)
        {
            if (ModelState.IsValid)
            {
                db.EstudianteParaFacturas.Add(estudianteParaFactura);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EstudianteId = new SelectList(db.Estudiantes, "Id", "Apellidos", estudianteParaFactura.EstudianteId);
            return View(estudianteParaFactura);
        }

        // GET: EstudianteParaFactura/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EstudianteParaFactura estudianteParaFactura = db.EstudianteParaFacturas.Find(id);
            if (estudianteParaFactura == null)
            {
                return HttpNotFound();
            }
            ViewBag.EstudianteId = new SelectList(db.Estudiantes, "Id", "Apellidos", estudianteParaFactura.EstudianteId);
            return View(estudianteParaFactura);
        }

        // POST: EstudianteParaFactura/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,EstudianteId")] EstudianteParaFactura estudianteParaFactura)
        {
            if (ModelState.IsValid)
            {
                db.Entry(estudianteParaFactura).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EstudianteId = new SelectList(db.Estudiantes, "Id", "Apellidos", estudianteParaFactura.EstudianteId);
            return View(estudianteParaFactura);
        }

        // GET: EstudianteParaFactura/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EstudianteParaFactura estudianteParaFactura = db.EstudianteParaFacturas.Find(id);
            if (estudianteParaFactura == null)
            {
                return HttpNotFound();
            }
            return View(estudianteParaFactura);
        }

        // POST: EstudianteParaFactura/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EstudianteParaFactura estudianteParaFactura = db.EstudianteParaFacturas.Find(id);
            db.EstudianteParaFacturas.Remove(estudianteParaFactura);
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
