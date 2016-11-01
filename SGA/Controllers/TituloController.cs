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
using System.Data.Entity.Validation;

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
        public ActionResult Create([Bind(Include = "Id,Nombre,Precio")] Titulo titulo, HttpPostedFileBase Foto)
        {
            titulo.Foto = ClaseSelect.GetInstancia().guardarArchivo(titulo.Id, Foto, "~/Imagenes/Portada/");

            if (ModelState.IsValid)
            {
                try
                {
                    db.Titulos.Add(titulo);
                    db.SaveChanges();
                    TempData["mensaje"] = "Se registró el título satisfactoriamente";
                    return RedirectToAction("Index");
                }
                catch (DbEntityValidationException mex)
                {
                    TempData["mensajeError"] = "No se pudo realizar la acción. Trate nuevamente, si el problema persiste contacte al administrador del sistema.";
                }
                catch (DbUpdateException e)
                {
                    TempData["mensajeError"] = "No se pudo realizar la acción. Compruebe si ya existe un titulo registrado con el mismo código , si el problema persiste contacte al administrador del sistema.";
                }
                catch (Exception e)
                {
                    TempData["mensajeError"] = "No se pudo realizar la acción. Trate nuevamente, si el problema persiste contacte al administrador del sistema.";
                }
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
        public ActionResult Edit([Bind(Include ="Id,Nombre,Precio,Foto")] Titulo tituloActualizar, HttpPostedFileBase Foto, string FotoActual)
        {
            if (tituloActualizar.Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
          

            if (!FotoActual.Equals("noPortada.jpg") && Foto == null)
                tituloActualizar.Foto = FotoActual;
            else
                tituloActualizar.Foto = ClaseSelect.GetInstancia().guardarArchivo(tituloActualizar.Id, Foto, "~/Imagenes/Portada/");

            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(tituloActualizar).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["mensaje"] = "Se registraron los cambios del título satisfactoriamente";
                    return RedirectToAction("Index");
                }
            }
            catch (DbEntityValidationException mex)
            {
                TempData["mensajeError"] = "No se pudo realizar la acción. Trate nuevamente, si el problema persiste contacte al administrador del sistema.";
            }
            catch (DbUpdateException e)
            {
                TempData["mensajeError"] = "No se pudo realizar la acción. Compruebe si ya existe un administrador registrado con el mismo código , si el problema persiste contacte al administrador del sistema.";
            }
            catch (Exception e)
            {
                TempData["mensajeError"] = "No se pudo realizar la acción. Trate nuevamente, si el problema persiste contacte al administrador del sistema.";
            }

            return View(tituloActualizar);
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
