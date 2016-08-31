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
    public class EstudianteController : Controller
    {
        private SGAContext db = new SGAContext();

        // GET: Estudiante
        public ActionResult Index()
        {
            return View(db.Estudiantes.ToList());
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
            return View();
        }

        // POST: Estudiante/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Apellidos,Clave,Sexo,Identificacion,Profecion,Institucion,Fotografia,Estado,Nombre,Pais,Telefono,Correo,CorreoAlternativo,Direccion,Fechacontratacion")] Estudiante estudiante)
        {
            if (ModelState.IsValid)
            {
                db.Estudiantes.Add(estudiante);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(estudiante);
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
            return View(estudiante);
        }

        // POST: Estudiante/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Apellidos,Clave,Sexo,Identificacion,Profecion,Institucion,Fotografia,Estado,Nombre,Pais,Telefono,Correo,CorreoAlternativo,Direccion,Fechacontratacion")] Estudiante estudiante)
        {
            if (ModelState.IsValid)
            {
                db.Entry(estudiante).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
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

namespace SGA.Controllers
{
    public class EstudiantesController : Controller
    {
        private SGAContext db = new SGAContext();

        // GET: Estudiantes
        public ActionResult Index(string ordenarPor, string buscado)
        {
           /* ViewBag.NombreParametroOrdenamiento = String.IsNullOrEmpty(ordenarPor) ? "nombre_desc" : "Nombre";
            ViewBag.FechaParametroOrdenamiento = ordenarPor == "Fecha" ? "fecha_desc" : "Fecha";
            ViewBag.ApellidosParametroOrdenamiento =ordenarPor== "Apellidos" ? "apellidos_desc" : "Apellidos";
            var estudiantes = from e in db.Estudiantes
                              select e;
            if (!String.IsNullOrEmpty(buscado))
            {
                estudiantes = estudiantes.Where(s => s.apellidos.Contains(buscado) || s.nombre.Contains(buscado));
            }
            switch (ordenarPor)
            {
                case "Nombre":
                    estudiantes = estudiantes.OrderBy(s => s.nombre);
                    break;
                case "nombre_desc":
                    estudiantes = estudiantes.OrderByDescending(s => s.nombre);
                    break;
                case "Fecha":
                    estudiantes = estudiantes.OrderBy(s => s.diaMatricula);
                    break;
                case "fecha_desc":
                    estudiantes = estudiantes.OrderByDescending(s => s.diaMatricula);
                    break;
                case "Apellidos":
                    estudiantes = estudiantes.OrderBy(s => s.apellidos);
                    break;
                case "apellidos_desc":
                    estudiantes = estudiantes.OrderByDescending(s => s.apellidos);
                    break;

            }

            return View(estudiantes.ToList());
        }

        // GET: Estudiantes/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estudiante estudiante = db.Estudiantes.Include(e=>e.matriculas.Select(m=>m.Curso.Titulo)).Single(e=>e.ID==id);
            if (estudiante == null)
            {
                return HttpNotFound();
            }
            return View(estudiante);
        }

        // GET: Estudiantes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Estudiantes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,nombre,apellidos,diaMatricula")] Estudiante estudiante)
        {
            try {
                if (ModelState.IsValid)
                {
                    db.Estudiantes.Add(estudiante);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException dex) {
                ModelState.AddModelError("", "No se pudo guardar, vuelva a insertar si el problema persiste consulte al administrador del sistema");
            }
            return View(estudiante);
            }

        // GET: Estudiantes/Edit/5
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
            return View(estudiante);
        }

        // POST: Estudiantes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(string id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var estudianteActualizar = db.Estudiantes.Find(id);
            if(TryUpdateModel(estudianteActualizar,"",new string[] {"nombre", "apellidos", "diaMatricula" }))
            if (ModelState.IsValid)
            {
                db.Entry(estudianteActualizar).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(estudianteActualizar);
        }

        // GET: Estudiantes/Delete/5
        public ActionResult Delete(string id, bool? guardarCambioErrores=false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (guardarCambioErrores.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "No se pudo borrar, vuelva a insertar si el problema persiste consulte al administrador del sistema";
            }
            Estudiante estudiante = db.Estudiantes.Find(id);
            if (estudiante == null)
            {
                return HttpNotFound();
            }
            return View(estudiante);
        }

        // POST: Estudiantes/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id)
        {
            try {
                Estudiante estudianteABorrar = new Estudiante() { ID = id };
                db.Entry(estudianteABorrar).State = EntityState.Deleted;
                db.SaveChanges();

            } catch (DataException dex) {
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });

            }

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
