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
using SGA.ViewModels;
using System.Data.Entity.Validation;

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
            Generacion generacion = db.Generacions.Include(g=>g.TitulosRequisito).Single(g=>g.Id==id);
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
                try
                {
                    db.Generacions.Add(generacion);
                generacion.TitulosRequisito = new List<Titulo>();//Para no inicializar aquí se puede inicializar en el modelo en el get y el set
                foreach (var titulo in titulosSeleccionados)
                {
                    var incluirtitulo = db.Titulos.Find(titulo);
                    generacion.TitulosRequisito.Add(incluirtitulo);
                }

                    db.SaveChanges();
                    TempData["mensaje"] = "Se registró la generación satisfactoriamente";
                    return RedirectToAction("Index");
                }
                catch (DbEntityValidationException mex)
                {
                    TempData["mensajeError"] = "No se pudo realizar la acción. Trate nuevamente, si el problema persiste contacte al administrador del sistema.";
                }
                catch (DbUpdateException e)
                {
                    TempData["mensajeError"] = "No se pudo realizar la acción. Compruebe si ya existe una generación registrada con el mismo nombre o que escogió al menos un requisito para graduarse, si el problema persiste contacte al administrador del sistema.";
                }
                catch (Exception e)
                {
                    TempData["mensajeError"] = "No se pudo realizar la acción. Trate nuevamente, si el problema persiste contacte al administrador del sistema.";
                }
            }

            return View(generacion);
        }

        private void ActualizarRequisitosGeneracion(string[] titulosSeleccionados, Generacion generacionActualizar)
        {
            if (titulosSeleccionados == null)
            {
                generacionActualizar.TitulosRequisito = new List<Titulo>();
                return;
            }

            var titulosSeleccionadosHS = new HashSet<string>(titulosSeleccionados);
            var titulosGeneracion = new HashSet<string>
                (generacionActualizar.TitulosRequisito.Select(t => t.Id));
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

        private void populateTituloRequeridoGeneracion(Generacion generacion)
        {
            var todosLosTitulos = db.Titulos.ToList();
            var titulosgeneracion = new HashSet<string>(generacion.TitulosRequisito.Select(t => t.Id));
            var viewModel = new List<AsignarRequistoGeneracion>();
            foreach (var titulo in todosLosTitulos)
            {
                AsignarRequistoGeneracion act = new AsignarRequistoGeneracion
                {
                    TituloID = titulo.Id,
                    TituloNombre = titulo.Nombre,
                    Asignado = titulosgeneracion.Contains(titulo.Id)

                };
                viewModel.Add(act);
            }
            ViewBag.Titulos = viewModel;
        }

        // GET: Generacion/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Generacion generacion = db.Generacions.Include(g=>g.TitulosRequisito).Single(g=>g.Id==id);
            if (generacion == null)
            {
                return HttpNotFound();
            }
            populateTituloRequeridoGeneracion(generacion);
            return View(generacion);
        }

        // POST: Generacion/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Fecha,Foto")] Generacion generacionActualizar, string[] titulosSeleccionados,string id, HttpPostedFileBase Foto, string FotoActual)
        {
            if (generacionActualizar.Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }            

            if (!FotoActual.Equals("noPortada.jpg.png") && Foto == null)
                generacionActualizar.Foto = FotoActual;
            else
            generacionActualizar.Foto = ClaseSelect.GetInstancia().guardarArchivo(generacionActualizar.Id, Foto, "~/Imagenes/Portada/");
                        
                try
                {
                    ActualizarRequisitosGeneracion(titulosSeleccionados, generacionActualizar);
                if (ModelState.IsValid)
                {
                    db.Entry(generacionActualizar).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["mensaje"] = "Se registraron los cambios de la generación satisfactoriamente";
                    return RedirectToAction("Index");
                }
                }
            catch (DbEntityValidationException mex)
            {
                TempData["mensajeError"] = "No se pudo realizar la acción. Trate nuevamente, si el problema persiste contacte al administrador del sistema.";
            }
            catch (DbUpdateException e)
            {
                TempData["mensajeError"] = "No se pudo realizar la acción. Compruebe si ya existe una generación registrada con el mismo nombre o que escogió al menos un requisito para graduarse, si el problema persiste contacte al administrador del sistema.";
            }
            catch (Exception e)
            {
                TempData["mensajeError"] = "No se pudo realizar la acción. Trate nuevamente, si el problema persiste contacte al administrador del sistema.";
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
