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
    public class TituloController : Controller
    {
        private SGAContext db = new SGAContext();

        // GET: Titulo
        public ActionResult Index()
        {
            return View(db.Titulos.ToList());
        }

        // GET: Titulo/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Titulo titulo = db.Titulos.Find(id);
            if (titulo == null)
            {
                return HttpNotFound();
            }
            return View(titulo);
        }

        // GET: Titulo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Titulo/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,Foto,Precio")] Titulo titulo)
        {
            if (ModelState.IsValid)
            {
                db.Titulos.Add(titulo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(titulo);
        }

        // GET: Titulo/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Titulo titulo = db.Titulos.Find(id);
            if (titulo == null)
            {
                return HttpNotFound();
            }
            return View(titulo);
        }

        // POST: Titulo/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,Foto,Precio")] Titulo titulo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(titulo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(titulo);
        }

        // GET: Titulo/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Titulo titulo = db.Titulos.Find(id);
            if (titulo == null)
            {
                return HttpNotFound();
            }
            return View(titulo);
        }

        // POST: Titulo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Titulo titulo = db.Titulos.Find(id);
            db.Titulos.Remove(titulo);
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
