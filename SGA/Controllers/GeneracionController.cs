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
            ViewBag.Titulos = db.Titulos.ToList();
            return View();
        }

        // POST: Generacion/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string[] titulosSeleccionados, [Bind(Include = "Id,Fecha")] Generacion generacion,HttpPostedFileBase Foto)
        {
            generacion.Foto = ClaseSelect.GetInstancia().guardarArchivo(generacion.Id, Foto, "~/Imagenes/Portada/");
            if (ModelState.IsValid)
            {
                db.Generacions.Add(generacion);
                generacion.TitulosRequisito = new List<Titulo>();//Para no inicializar aquí se puede inicializar en el modelo en el get y el set
                foreach (var titulo in titulosSeleccionados)
                {
                    var incluirtitulo = db.Titulos.Find(titulo);
                    generacion.TitulosRequisito.Add(incluirtitulo);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(generacion);
        }

        private void ActualizarCursosInstructor(string[] titulosSeleccionados, Generacion generacionActualizar)
        {
            if (titulosSeleccionados == null)
            {
                generacionActualizar.TitulosRequisito = new List<Titulo>();
                return;
            }

            var titulosSeleccionadosHS = new HashSet<string>(titulosSeleccionados);
            var titulosGeneracion = new HashSet<string>
                (generacionActualizar.TitulosRequisito.Select(c => c.Id));
            foreach (var titulo in db.Titulos)
            {
                if (titulosSeleccionadosHS.Contains(titulo.Id))
                {
                    if (!titulosGeneracion.Contains(titulo.Id))
                    {
                        generacionActualizar.TitulosRequisito.Add(titulo);
                    }
                }
                else
                {
                    if (titulosGeneracion.Contains(titulo.Id))
                    {
                        generacionActualizar.TitulosRequisito.Remove(titulo);
                    }
                }
            }
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
        public ActionResult Edit(string id, HttpPostedFileBase Foto, string FotoActual)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var generacionActualizar = db.Generacions.Single(g => g.Id == id);

            if (!FotoActual.Equals("noPortada.jpg.png") && Foto == null)
                generacionActualizar.Foto = FotoActual;
            else
            generacionActualizar.Foto = ClaseSelect.GetInstancia().guardarArchivo(generacionActualizar.Id, Foto, "~/Imagenes/Portada/");

            if (TryUpdateModel(generacionActualizar,"",
                new string[] { "Id,Fecha,Foto" }))
            {
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException dex)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "No se pudo realizar la acción. Trate nuevamente, si el problema persiste contacte al administrador del sistema.");
                }
            }
            return View(generacionActualizar);
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
