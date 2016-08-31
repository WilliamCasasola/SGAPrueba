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
    public class GeneracionController : Controller
    {
        private SGAContext db = new SGAContext();

        // GET: Generacion
        public ActionResult Index()
        {
            return View(db.Generacions.ToList());
        }

        // GET: Generacion/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Generacion generacion = db.Generacions.Find(id);
            if (generacion == null)
            {
                return HttpNotFound();
            }
            return View(generacion);
        }

        // GET: Generacion/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Generacion/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Fecha")] Generacion generacion)
        {
            if (ModelState.IsValid)
            {
                db.Generacions.Add(generacion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(generacion);
        }

        // GET: Generacion/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Generacion generacion = db.Generacions.Find(id);
            if (generacion == null)
            {
                return HttpNotFound();
            }
            return View(generacion);
        }

        // POST: Generacion/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Fecha")] Generacion generacion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(generacion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(generacion);
        }

        // GET: Generacion/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Generacion generacion = db.Generacions.Find(id);
            if (generacion == null)
            {
                return HttpNotFound();
            }
            return View(generacion);
        }

        // POST: Generacion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Generacion generacion = db.Generacions.Find(id);
            db.Generacions.Remove(generacion);
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
