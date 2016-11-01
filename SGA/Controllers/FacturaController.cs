using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using SGA.DAL;
using SGA.Models;
using SGA.ViewModels;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
using System.Web;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;

namespace SGA.Controllers
{
    public class FacturaController : Controller
    {
        private SGAContext db = new SGAContext();
        private static List<EstudianteParaFactura> estudiantesFactura;

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
            Factura factura = db.Facturas.Include("Cliente").Single(f=>f.Id==id);
            var estudiantes = db.EstudianteParaFacturas.Where(e => e.FacturaID == id).Include("Titulos").Include("Estudiante");
            if (factura == null)
            {
                return HttpNotFound();
            }
            
            ViewBag.Detalles = "Si";
            factura.Detalles = estudiantes.ToList();
            double total=factura.Detalles.Select(e => e.Titulos.Select(t => t.Precio).Sum()).Sum();
            ViewBag.totalPagar = total;
            double deuda= total - factura.TotalCancelado;
            ViewBag.Deuda = deuda;
           
            return View(factura);
        }

       

        // GET: Factura/Create
        public ActionResult Create(string Clienteid,
            string Selectedtitles, string studentID, string date, string total,
          string state, string description, string client, string nuevo)
        {

            if (studentID != null)
            {
                string[] titulosSeleccionados = Regex.Split(Selectedtitles, ",");//Por que Selectedtitles viene en un solo string separado por comas
                if (!titulosSeleccionados[0].Equals(""))
                {
                    var titulos = db.Titulos.Where(t => titulosSeleccionados.Contains(t.Id));
                    var estudiante = db.Estudiantes.Find(studentID);
                    estudiantesFactura.Add(new EstudianteParaFactura
                    {
                        EstudianteId = studentID,
                        Titulos = titulos.ToList(),
                        Estudiante = estudiante,
                        
                    }
                    );
                }
                ViewBag.Detalles = "Si";
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
                {//Se valida la factura para que no se caiga
                    Fecha = date == "" ? DateTime.Now : DateTime.Parse(date),
                    TotalCancelado = esDouble == false ? 0 : Double.Parse(total),
                    estado = Boolean.Parse(state), //== null ? EstadoFactura.Cancelado : (EstadoFactura)Enum.Parse(typeof(EstadoFactura), state),//Es necesario parsearlo usando typeof y luego el cast
                    Descripcion = description == null ? "" : description,
                    ClienteId = client == null ? null : client,
                    Detalles = estudiantesFactura
                };
                ViewBag.totalPagar = estudiantesFactura.Select(e => e.Titulos.Select(t => t.Precio).Sum()).Sum();
                if (nuevo != null)
                {
                    ViewBag.EstudianteId = new SelectList(db.Estudiantes, "Id", "Nombre");
                    ViewBag.Titulos = db.Titulos.ToList();
                    if (estudiantesFactura.Count() > 0)
                        ViewBag.Detalles = "Si";
                    ViewBag.mostrarTitulos = "Si";
                }
                return View(factura);
            }
            else
            {
                estudiantesFactura = new List<EstudianteParaFactura>();
                return View();
            }
        }

        // POST: Factura/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FacturaId,Fecha,ClienteId,Descripcion,estado,TotalCancelado")] Factura factura, HttpPostedFileBase Comprobante)
        {
            factura.Comprobante= ClaseSelect.GetInstancia().guardarArchivo(factura.Id.ToString(), Comprobante, "~/Imagenes/Comprobante/");
            if (ModelState.IsValid)
            {
                try
                {
                    db.Facturas.Add(factura);
                    db.SaveChanges();
                    db.Entry(factura).GetDatabaseValues();//Le asigna al objeto que insertó su id creada dinámicamente
                    estudiantesFactura.All(e => { e.FacturaID = factura.Id; e.Estudiante = null; return true; }); ;
                    db.Dispose();
                    crearEstudiantesFactura();
                    TempData["mensaje"] = "Se registró la factura satisfactoriamente";
                    return RedirectToAction("Index");
                }
                catch (DbEntityValidationException mex)
                {
                    TempData["mensajeError"] = "No se pudo realizar la acción. Trate nuevamente, si el problema persiste contacte al administrador del sistema.";
                }
                catch (DbUpdateException e)
                {
                    TempData["mensajeError"] = "No se pudo realizar la acción. Compruebe si ya existe una factura registrada con el mismo número o que escogió o que todos los datos existan, si el problema persiste contacte al administrador del sistema.";
                }
                catch (Exception e)
                {
                    TempData["mensajeError"] = "No se pudo realizar la acción. Trate nuevamente, si el problema persiste contacte al administrador del sistema.";
                }
            }
            ViewBag.Detalles = "Si";
            factura.Detalles = estudiantesFactura;
            ViewBag.Facturas = new List<Factura>();
            ViewBag.ClienteId = new SelectList(db.Clientes, "Id", "Nombre", factura.ClienteId);
            return View(factura);
        }

        public void crearEstudiantesFactura(){
            db = new SGAContext();
            foreach (var estudianteFactura in estudiantesFactura)
            {
                var titulos = estudianteFactura.Titulos;
                estudianteFactura.Titulos = null;
                db.EstudianteParaFacturas.Add(estudianteFactura);
                db.SaveChanges();
                db.Entry(estudianteFactura).GetDatabaseValues();//Le asigna al objeto que insertó su id creada dinámicamente
                foreach (var titulo in titulos)
                {
                 db.Database.ExecuteSqlCommand("Insert INTO estudianteparafacturatitulo VALUES(@estudiante,@titulo)",
                        new MySqlParameter("@estudiante", estudianteFactura.Id),
                    new MySqlParameter("@titulo", titulo.Id));
                    db.SaveChanges();
                }
            }
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
        public ActionResult Edit([Bind(Include = "FacturaId,Fecha,ClienteId,Descripcion,estado,TotalCancelado")] Factura factura, HttpPostedFileBase Comprobante, string ComprobanteActual)
        {
            if (ModelState.IsValid)
            {
                var estudiantes = db.EstudianteParaFacturas.Where(e => e.FacturaID == factura.Id).Include("Titulos").Include("Estudiante").ToList();
                if (factura.TotalCancelado >= estudiantes.Select(e => e.Titulos.Select(t => t.Precio).Sum()).Sum())
                    factura.estado = true;// EstadoFactura.Cancelado;
                if (!ComprobanteActual.Equals("noComprobante.jpg") && Comprobante == null)
                    factura.Comprobante = ComprobanteActual;
                else
                    factura.Comprobante = ClaseSelect.GetInstancia().guardarArchivo(factura.Id.ToString(), Comprobante, "~/Imagenes/Comprobante/");
                db.Entry(factura).State = EntityState.Modified;
                try { 
                db.SaveChanges();
                    TempData["mensaje"] = "Se registraron los cambios de la factura satisfactoriamente";
                    return RedirectToAction("Index");
                }
                catch (DbEntityValidationException mex)
                {
                    TempData["mensajeError"] = "No se pudo realizar la acción. Trate nuevamente, si el problema persiste contacte al administrador del sistema.";
                }
                catch (DbUpdateException e)
                {
                    TempData["mensajeError"] = "No se pudo realizar la acción. Compruebe si ya existe una factura registrada con el mismo número o que escogió o que todos los datos existan, si el problema persiste contacte al administrador del sistema.";
                }
                catch (Exception e)
                {
                    TempData["mensajeError"] = "No se pudo realizar la acción. Trate nuevamente, si el problema persiste contacte al administrador del sistema.";
                }
              
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
