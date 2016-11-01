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
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;

namespace SGA.Controllers
{
    public class ClienteController : Controller
    {
        private SGAContext db = new SGAContext();

        // GET: Cliente
        public ActionResult Index()
        {
            return View(db.Clientes.SqlQuery("SELECT * FROM Cliente WHERE Discriminator = 'Cliente'").ToList());
        }

        // GET: Cliente/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Clientes.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // GET: Cliente/Create
        public ActionResult Create()
        {
            ViewBag.Paises = ClaseSelect.GetInstancia().GetCountries();

            return View();
        }

        // POST: Cliente/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,Pais,Telefono,Correo,CorreoAlternativo,Direccion")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    cliente.FechaRegistro = DateTime.Now;
                    db.Clientes.Add(cliente);
                    db.SaveChanges();
                    TempData["mensaje"] = "Se registró el cliente satisfactoriamente";
                    return RedirectToAction("Index");
                }
                catch (DbEntityValidationException mex)
                {
                    TempData["mensajeError"] = "No se pudo realizar la acción. Trate nuevamente, si el problema persiste contacte al administrador del sistema.";
                }
                catch (DbUpdateException e)
                {
                    TempData["mensajeError"] = "No se pudo realizar la acción. Compruebe si ya existe un cliente registrado con el mismo carnet , si el problema persiste contacte al administrador del sistema.";
                }
                catch (Exception e)
                {
                    TempData["mensajeError"] = "No se pudo realizar la acción. Trate nuevamente, si el problema persiste contacte al administrador del sistema.";
                }
            }

            return View(cliente);
        }

        // GET: Cliente/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Clientes.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            ViewBag.Paises = ClaseSelect.GetInstancia().GetCountries();

            return View(cliente);
        }

        // POST: Cliente/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,Pais,Telefono,Correo,CorreoAlternativo,Direccion")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(cliente).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["mensaje"] = "Se registraron los cambios en el cliente satisfactoriamente";
                    return RedirectToAction("Index");
                }
                catch (DbEntityValidationException mex)
                {
                    TempData["mensajeError"] = "No se pudo realizar la acción. Trate nuevamente, si el problema persiste contacte al administrador del sistema.";
                }
                catch (DbUpdateException e)
                {
                    TempData["mensajeError"] = "No se pudo realizar la acción. Compruebe si ya existe un cliente registrado con el mismo carnet , si el problema persiste contacte al administrador del sistema.";
                }
                catch (Exception e)
                {
                    TempData["mensajeError"] = "No se pudo realizar la acción. Trate nuevamente, si el problema persiste contacte al administrador del sistema.";
                }
            }
            return View(cliente);
        }

        // GET: Cliente/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Clientes.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // POST: Cliente/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Cliente cliente = db.Clientes.Find(id);
            db.Clientes.Remove(cliente);
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
