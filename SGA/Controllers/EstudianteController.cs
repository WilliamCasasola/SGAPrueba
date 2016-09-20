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
using SGA.ViewModels;
using System.IO;
using System.Globalization;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace SGA.Controllers
{
    public class EstudianteController : Controller
    {
        private SGAContext db = new SGAContext();

        // GET: Estudiante
        public ActionResult Index()
        {
            var estudiantes = db.Estudiantes.Include(e => e.Generacion);
            return View(estudiantes.ToList());
        }

        // GET: Estudiante/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estudiante estudiante = db.Estudiantes.Find(id);
            if (estudiante == null)
            {
                return HttpNotFound();
            }
            return View(estudiante);
        }

       

        // GET: Estudiante/Create
        public ActionResult Create()
        {
            ViewBag.Paises = ClaseSelect.GetInstancia().GetCountries();
            ViewBag.GeneracionId = new SelectList(db.Generacions, "Id", "Id");
            return View();
        }

        

        // POST: Estudiante/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Nombre,Pais,Telefono,Correo,CorreoAlternativo,Direccion,Apellidos,Clave,Sexo,Profesion,Institucion,Estado,GeneracionId")] Estudiante estudiante, HttpPostedFileBase Fotografia, HttpPostedFileBase Identificacion )
        {
            estudiante = inicializarCodigo(estudiante);
            estudiante.Fotografia = ClaseSelect.GetInstancia().guardarArchivo(estudiante.Id, Fotografia, "~/Imagenes/Perfil/");
            estudiante.Identificacion = ClaseSelect.GetInstancia().guardarArchivo(estudiante.Id, Identificacion, "~/Imagenes/Documento/");


            if (ModelState.IsValid)
            {
                db.Estudiantes.Add(estudiante);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.GeneracionId = new SelectList(db.Generacions, "Id", "Id", estudiante.GeneracionId);
            return View(estudiante);
        }

        private Estudiante inicializarCodigo(Estudiante estudiante) {
            string letra = "A";
            string a = DateTime.Now.Year.ToString().Substring(2);//En el 2100 se debe de cambiar a 3 para que coja los últimos 3 dígitos
            int cantidadEst = 0;
            if (DateTime.Now.Month < 7)
                cantidadEst = db.Estudiantes.Where(e => e.FechaRegistro.Year == DateTime.Now.Year && e.FechaRegistro.Month < 7).Count();
            else
            {
                cantidadEst = db.Estudiantes.Where(e => e.FechaRegistro.Year == DateTime.Now.Year && e.FechaRegistro.Month >= 7).Count();
                letra = "B";
            }
            int cantidaNum = cantidadEst==0? 0 : (int)Math.Floor(Math.Log10(cantidadEst) + 1);//Por que logarítmo de 0 es infinito

            string ceros = cantidaNum==0 ? new String('0', 3) : new String('0',4-cantidaNum);//Aquí crea la cantidad de ceros a la izquierda, con log de 10 saca la cantidad de números a la derecha
            
            estudiante.Id = String.Concat(a, String.Concat(letra, String.Concat(ceros, cantidadEst)));
            estudiante.FechaRegistro = DateTime.Now;
            return estudiante;
        }

        // GET: Estudiante/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estudiante estudiante = db.Estudiantes.Find(id);
            if (estudiante == null)
            {
                return HttpNotFound();
            }
            ViewBag.Paises = ClaseSelect.GetInstancia().GetCountries();
            ViewBag.GeneracionId = new SelectList(db.Generacions, "Id", "Id", estudiante.GeneracionId);
            return View(estudiante);
        }

        // POST: Estudiante/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,Pais,Telefono,Correo,CorreoAlternativo,Direccion,Apellidos,Clave,Sexo,Profesion,Institucion,Estado,GeneracionId")] Estudiante estudiante, HttpPostedFileBase Fotografia, HttpPostedFileBase Identificacion, string FotoActual, string IdentificacionActual)
        {
            if (!FotoActual.Equals("noperfil.jpg") && Fotografia == null)
                estudiante.Fotografia = FotoActual;
            else
                estudiante.Fotografia = ClaseSelect.GetInstancia().guardarArchivo(estudiante.Id, Fotografia, "~/Imagenes/Perfil/");


            if (!IdentificacionActual.Equals("nodocumento.png") && Identificacion == null)
                estudiante.Identificacion = IdentificacionActual;
            else
                estudiante.Identificacion = ClaseSelect.GetInstancia().guardarArchivo(estudiante.Id, Identificacion, "~/Imagenes/Documento/");
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(estudiante).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }
            }
            ViewBag.Paises = ClaseSelect.GetInstancia().GetCountries();
            ViewBag.GeneracionId = new SelectList(db.Generacions, "Id", "Foto", estudiante.GeneracionId);
            return View(estudiante);
        }

        // GET: Estudiante/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estudiante estudiante = db.Estudiantes.Find(id);
            if (estudiante == null)
            {
                return HttpNotFound();
            }
            return View(estudiante);
        }

        // POST: Estudiante/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Estudiante estudiante = db.Estudiantes.Find(id);
            db.Estudiantes.Remove(estudiante);
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
