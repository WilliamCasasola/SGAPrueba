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
using System.Text.RegularExpressions;
using System.Web.Routing;

namespace SGA.Controllers
{
    public class EstudianteParaFacturaController : Controller
    {
        private SGAContext db = new SGAContext();
        private static System.Uri antiguauri;
        private static Factura factura;

        // GET: EstudianteParaFactura
        public ActionResult Index()
        {
            var estudianteParaFacturas = db.EstudianteParaFacturas.Include(e => e.Estudiante);
            return View(estudianteParaFacturas.ToList());
        }

        // GET: EstudianteParaFactura/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EstudianteParaFactura estudianteParaFactura = db.EstudianteParaFacturas.Find(id);
            if (estudianteParaFactura == null)
            {
                return HttpNotFound();
            }
            return View(estudianteParaFactura);
        }

        // GET: EstudianteParaFactura/Create
        public ActionResult Create(string date, string total,
          string state, string description,string client)
        {
            Double d = 0;
            bool esDouble = total ==null? false : Double.TryParse(total,out d);

            factura = new Factura//Se inicializa la factura para noperder los datos
            {
                Fecha = date == "" ? DateTime.Now : DateTime.Parse(date),
                TotalCancelado = esDouble == false ? 0 : Double.Parse(total),
                estado = state == null ? EstadoFactura.Cancelado : (EstadoFactura)Enum.Parse(typeof(EstadoFactura), state),//Es necesario parsearlo usando typeof y luego el cast
                Descripcion = description == null ? "" : description,
                ClienteId = client == null ? null : client
            };
            antiguauri = System.Web.HttpContext.Current.Request.UrlReferrer;//Se guarda de donde viene la consulta
            if (!new Regex("/Factura/Create.+$").IsMatch(antiguauri.AbsolutePath))//Si no viene de la página factura
            {
                var estudianteFactura = new EstudianteParaFactura();
                estudianteFactura.Titulos = new List<Titulo>();
                ViewBag.EstudianteId = new SelectList(db.Estudiantes, "Id", "Apellidos");
                populateTituloAsignadoEstudiante(estudianteFactura);
                return View();
            }
            else return RedirectToAction("Index", "Factura");


        }

        // POST: EstudianteParaFactura/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,EstudianteId")] EstudianteParaFactura estudianteParaFactura, string[] titulosSeleccionados)
        {

            estudianteParaFactura.Titulos = new List<Titulo>();
            foreach (var titulo in titulosSeleccionados) {
                estudianteParaFactura.Titulos.Add(db.Titulos.Find(titulo));
            }
            if (ModelState.IsValid)
            {

                db.EstudianteParaFacturas.Add(estudianteParaFactura);
                db.SaveChanges();
                db.Entry(estudianteParaFactura).GetDatabaseValues();
             return RedirectToAction("Create", "Factura", new { Clienteid=factura.ClienteId, factura= new RouteValueDictionary(factura) });
                
            }
            populateTituloAsignadoEstudiante(estudianteParaFactura);
            ViewBag.EstudianteId = new SelectList(db.Estudiantes, "Id", "Apellidos", estudianteParaFactura.EstudianteId);
            return View(estudianteParaFactura);
        }


    // GET: EstudianteParaFactura/Edit/5
    public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EstudianteParaFactura estudianteParaFactura = db.EstudianteParaFacturas.Find(id);
            if (estudianteParaFactura == null)
            {
                return HttpNotFound();
            }
            ViewBag.EstudianteId = new SelectList(db.Estudiantes, "Id", "Apellidos", estudianteParaFactura.EstudianteId);
            return View(estudianteParaFactura);
        }

        // POST: EstudianteParaFactura/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,EstudianteId")] EstudianteParaFactura estudianteParaFactura)
        {
            if (ModelState.IsValid)
            {
                db.Entry(estudianteParaFactura).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EstudianteId = new SelectList(db.Estudiantes, "Id", "Apellidos", estudianteParaFactura.EstudianteId);
            return View(estudianteParaFactura);
        }

        // GET: EstudianteParaFactura/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EstudianteParaFactura estudianteParaFactura = db.EstudianteParaFacturas.Find(id);
            if (estudianteParaFactura == null)
            {
                return HttpNotFound();
            }
            return View(estudianteParaFactura);
        }

        // POST: EstudianteParaFactura/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EstudianteParaFactura estudianteParaFactura = db.EstudianteParaFacturas.Find(id);
            db.EstudianteParaFacturas.Remove(estudianteParaFactura);
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
                    Nombre=titulo.Nombre,
                    Asignado = TitulosEstudiantes.Contains(titulo.Id)
            };

                viewModel.Add(act);
            }
            ViewBag.Titulos = viewModel;
        }



    }
}
