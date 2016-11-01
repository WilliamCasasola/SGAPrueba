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
    public class AdministradorController : Controller
    {
        private SGAContext db = new SGAContext();

        // GET: Administrador
        public ActionResult Index()
        {
            return View(db.Administradors.ToList());
        }

        // GET: Administrador/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Administrador administrador = db.Administradors.Find(id);
            if (administrador == null)
            {
                return HttpNotFound();
            }
            ViewBag.sexo = administrador.Sexo ? "Masculino" : "Femenino";
            ViewBag.activo = administrador.Estado ? "Si" : "No";
            return View(administrador);
        }

        // GET: Administrador/Create
        public ActionResult Create()
        {
            ViewBag.Paises = ClaseSelect.GetInstancia().GetCountries();
            return View();
        }

        // POST: Administrador/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Apellidos,Clave,Sexo,Profesion,Institucion,Estado,Nombre,Pais,Telefono,Correo,CorreoAlternativo,Direccion")] Administrador administrador, HttpPostedFileBase Fotografia, HttpPostedFileBase Identificacion)
        {
            administrador.Fotografia = ClaseSelect.GetInstancia().guardarArchivo(administrador.Id, Fotografia, "~/Imagenes/Perfil/");
            administrador.Identificacion = ClaseSelect.GetInstancia().guardarArchivo(administrador.Id, Identificacion, "~/Imagenes/Documento/");
            if (ModelState.IsValid)
            {
                try
                {
                    administrador.FechaRegistro = DateTime.Now;
                    db.Administradors.Add(administrador);
                    db.SaveChanges();
                    TempData["mensaje"] = "Se registró el administrador satisfactoriamente";
                    return RedirectToAction("Index");
                }
                catch (DbEntityValidationException mex)
                {
                    TempData["mensajeError"] = "No se pudo realizar la acción. Trate nuevamente, si el problema persiste contacte al administrador del sistema.";
                }
                catch (DbUpdateException e)
                {             
                    TempData["mensajeError"] = "No se pudo realizar la acción. Compruebe si ya existe un administrador registrado con el mismo carnet , si el problema persiste contacte al administrador del sistema.";
                }
                catch (Exception e)
                {
                    TempData["mensajeError"] = "No se pudo realizar la acción. Trate nuevamente, si el problema persiste contacte al administrador del sistema.";
                }
            }
            ViewBag.Paises = ClaseSelect.GetInstancia().GetCountries();
            return View(administrador);
        }

        // GET: Administrador/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Administrador administrador = db.Administradors.Find(id);
            if (administrador == null)
            {
                return HttpNotFound();
            }
            ViewBag.Paises = ClaseSelect.GetInstancia().GetCountries();

            return View(administrador);
        }

        // POST: Administrador/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,Apellidos,Clave,Sexo,Identificacion,Profesion,Institucion,Fotografia,Estado,Nombre,Pais,Telefono,Correo,CorreoAlternativo,Direccion")] Administrador administradorActualizar, HttpPostedFileBase Fotografia, HttpPostedFileBase Identificacion, string FotoActual, string IdentificacionActual) {

                if (administradorActualizar.Id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if (!FotoActual.Equals("noperfil.jpg") && Fotografia == null)
                    administradorActualizar.Fotografia = FotoActual;
                else
                    administradorActualizar.Fotografia = ClaseSelect.GetInstancia().guardarArchivo(administradorActualizar.Id, Fotografia, "~/Imagenes/Perfil/");


                if (!IdentificacionActual.Equals("nodocumento.png") && Identificacion == null)
                    administradorActualizar.Identificacion = IdentificacionActual;
                else
                    administradorActualizar.Identificacion = ClaseSelect.GetInstancia().guardarArchivo(administradorActualizar.Id, Identificacion, "~/Imagenes/Documento/");
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(administradorActualizar).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["mensaje"] = "Se registraron los cambios en el administrador satisfactoriamente";
                    return RedirectToAction("Index");
                }
            }
            catch (DbEntityValidationException mex)
            {
                TempData["mensajeError"] = "No se pudo realizar la acción. Trate nuevamente, si el problema persiste contacte al administrador del sistema.";
            }
            catch (DbUpdateException e)
            {
                TempData["mensajeError"] = "No se pudo realizar la acción. Compruebe si ya existe un administrador registrado con el mismo carnet , si el problema persiste contacte al administrador del sistema.";
            }
            catch (Exception e)
            {
                TempData["mensajeError"] = "No se pudo realizar la acción. Trate nuevamente, si el problema persiste contacte al administrador del sistema.";
            }
            ViewBag.Paises = ClaseSelect.GetInstancia().GetCountries();
            return View(administradorActualizar);
            }
            
        

        // GET: Administrador/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Administrador administrador = db.Administradors.Find(id);
            if (administrador == null)
            {
                return HttpNotFound();
            }
            return View(administrador);
        }

        // POST: Administrador/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Administrador administrador = db.Administradors.Find(id);
            db.Administradors.Remove(administrador);
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
