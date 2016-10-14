﻿using System;
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
using System.Text.RegularExpressions;

namespace SGA.Controllers
{
    public class FacturaController : Controller
    {
        private SGAContext db = new SGAContext();
        private static List<EstudianteParaFactura> estudiantesFactura=new List<EstudianteParaFactura>();

        // GET: Factura
        public ActionResult Index()
        {
            var facturas = db.Facturas.Include(f => f.Cliente);
            var fact = facturas.ToList();
            return View(fact);
        }

        // GET: Factura/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Factura factura = db.Facturas.Find(id);
            if (factura == null)
            {
                return HttpNotFound();
            }
            return View(factura);
        }

        // GET: Factura/Create
        public ActionResult Create(string Clienteid,
            string Selectedtitles, string studentID, string date, string total,
          string state, string description, string client)
        {
            
            if (studentID != null) {
                string[] titulosSeleccionados = Regex.Split(Selectedtitles,",");
                ViewBag.Detalles="Si";
                var titulos = db.Titulos.Where(t => titulosSeleccionados.Contains(t.Id));
                var estudiante = db.Estudiantes.Find(studentID);
                estudiantesFactura.Add(new EstudianteParaFactura
                {
                    EstudianteId = studentID,
                    Titulos = titulos.ToList(),
                    Estudiante=estudiante
                }
                );
            }

            if (Clienteid != null)
            {
                ViewBag.Estudiantes = estudiantesFactura;
                ViewBag.ClienteNombre = db.Clientes.Where(c => c.Id == Clienteid).Select(c => c.Nombre).Single();
                var facturas = db.Facturas.Include(f => f.Cliente).Where(f => f.ClienteId == Clienteid);
                ViewBag.Facturas = facturas.ToList();
                ViewBag.ClienteId = new SelectList(db.Clientes.SqlQuery("SELECT * FROM Cliente WHERE Discriminator = 'Cliente'"), "Id", "Nombre", Clienteid);
            }
            else
            {
                ViewBag.Facturas = new List<Factura>();
                ViewBag.ClienteId = new SelectList(db.Clientes.SqlQuery("SELECT * FROM Cliente WHERE Discriminator = 'Cliente'"), "Id", "Nombre");
            }
            if (state != null)
            {
                Double d = 0;
                bool esDouble = total == null ? false : Double.TryParse(total, out d);

                Factura factura = new Factura//Se inicializa la factura para noperder los datos
                {
                    Fecha = date == "" ? DateTime.Now : DateTime.Parse(date),
                    TotalCancelado = esDouble == false ? 0 : Double.Parse(total),
                    estado = state == null ? EstadoFactura.Cancelado : (EstadoFactura)Enum.Parse(typeof(EstadoFactura), state),//Es necesario parsearlo usando typeof y luego el cast
                    Descripcion = description == null ? "" : description,
                    ClienteId = client == null ? null : client,
                    Detalles=estudiantesFactura
                };
                ViewBag.EstudianteId = new SelectList(db.Estudiantes, "Id", "Nombre");
                ViewBag.Titulos = db.Titulos.ToList();
                return View(factura);
            }
            else
                return View();
        }

        // POST: Factura/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FacturaId,Fecha,ClienteId,Descripcion,estado,TotalCancelado")] Factura factura)
        {
            if (ModelState.IsValid)
            {
                db.Facturas.Add(factura);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClienteId = new SelectList(db.Clientes, "Id", "Nombre", factura.ClienteId);
            return View(factura);
        }

        // GET: Factura/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Factura factura = db.Facturas.Find(id);
            if (factura == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClienteId = new SelectList(db.Clientes, "Id", "Nombre", factura.ClienteId);
            return View(factura);
        }

        // POST: Factura/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FacturaId,Fecha,ClienteId,Descripcion,estado,TotalCancelado")] Factura factura)
        {
            if (ModelState.IsValid)
            {
                db.Entry(factura).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClienteId = new SelectList(db.Clientes, "Id", "Nombre", factura.ClienteId);
            return View(factura);
        }

        // GET: Factura/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Factura factura = db.Facturas.Find(id);
            if (factura == null)
            {
                return HttpNotFound();
            }
            return View(factura);
        }

        // POST: Factura/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Factura factura = db.Facturas.Find(id);
            db.Facturas.Remove(factura);
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

        private void populateTituloAsignadoEstudiante(EstudianteParaFactura estudiante)
        {
            var todosLosTitulos = db.Titulos;
            var TitulosEstudiantes = new HashSet<string>(estudiante.Titulos.Select(c => c.Id));
            var viewModel = new List<AsignarTituloEstudiante>();
            foreach (var titulo in todosLosTitulos)
            {
                AsignarTituloEstudiante act = new AsignarTituloEstudiante
                {

                    TituloId = titulo.Id,
                    Precio = titulo.Precio,
                    Nombre = titulo.Nombre,
                    Asignado = TitulosEstudiantes.Contains(titulo.Id)
                };

                viewModel.Add(act);
            }
            ViewBag.Titulos = viewModel;
        }
    }
}
