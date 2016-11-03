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
using System.Data.Entity.Infrastructure;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;


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
            Estudiante estudiante = db.Estudiantes.Include(e=>e.Generacion).Single(e=>e.Id==id);
            estudiante.matriculas = db.Matriculas.Include(m=>m.Curso.Titulo).Include(m=>m.Calificaciones).Where(m => m.EstudianteID == id).ToList();

            if (estudiante == null)
            {
                return HttpNotFound();
            }

            var requisitos = db.Database.SqlQuery<string>("SELECT Titulo_Id FROM titulogeneracion WHERE Generacion_Id = @gen ", new MySqlParameter("@gen", estudiante.GeneracionId));
            int contador = 0;
            foreach (var item in estudiante.matriculas)//.Zip(curosNota.Notas, (a, b) => new { matricula = a, nota = b }))
            {

                if (requisitos.Contains(item.Curso.TituloId))
                {
                    if (item.NotaFinal >= 70)
                        contador++;
                }
            }
            int cantidadRequisitos = requisitos.Count();
            if (contador == cantidadRequisitos)
                ViewBag.PuedeGraduarse = "Si";
            else
                ViewBag.PuedeGraduarse = "No puede, necesita pasar " + (cantidadRequisitos - contador) + " curso(s) más";

            if (contador >= (cantidadRequisitos - 2))
                ViewBag.PuedeProyecto = "Si";
            else
                ViewBag.PuedeProyecto = "No puede, necesita pasar " + (cantidadRequisitos - contador - 2) + " curso(s) más";

            ViewBag.sexo = estudiante.Sexo ? "Masculino" : "Femenino";
            ViewBag.activo = estudiante.Estado ? "Si" : "No";
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
                try { 
                db.Estudiantes.Add(estudiante);
                db.SaveChanges();
                TempData["mensaje"] = "Se registraró el estudiante satisfactoriamente";
                    return RedirectToAction("Index");
            }
              catch (DbEntityValidationException mex)
            {
                TempData["mensajeError"] = "No se pudo realizar la acción. Trate nuevamente, si el problema persiste contacte al administrador del sistema.";
            }
            catch (DbUpdateException e)
            {                
                TempData["mensajeError"] = "No se pudo realizar la acción. Compruebe si ya existe un estudiante con el mismo carnet registrado o si la generación existen, si el problema persiste contacte al administrador del sistema.";
            }
            catch (Exception e)
            {
                TempData["mensajeError"] = "No se pudo realizar la acción. Trate nuevamente, si el problema persiste contacte al administrador del sistema.";
            }
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
        public ActionResult Edit([Bind(Include ="Id,Nombre,Pais,Identificacion,Telefono,Correo,CorreoAlternativo,Direccion,Apellidos,Fotografia,Clave,Sexo,Profesion,Institucion,Estado,GeneracionId")] Estudiante estudianteActualizar, HttpPostedFileBase Fotografia, HttpPostedFileBase Identificacion, string FotoActual, string IdentificacionActual)
        {
            if (estudianteActualizar.Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            if (!FotoActual.Equals("noperfil.jpg") && Fotografia == null)
                estudianteActualizar.Fotografia = FotoActual;
            else
                estudianteActualizar.Fotografia = ClaseSelect.GetInstancia().guardarArchivo(estudianteActualizar.Id, Fotografia, "~/Imagenes/Perfil/");


            if (!IdentificacionActual.Equals("nodocumento.png") && Identificacion == null)
                estudianteActualizar.Identificacion = IdentificacionActual;
            else
                estudianteActualizar.Identificacion = ClaseSelect.GetInstancia().guardarArchivo(estudianteActualizar.Id, Identificacion, "~/Imagenes/Documento/");          
                        try
                    {
                if (ModelState.IsValid)
                {
                    db.Entry(estudianteActualizar).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["mensaje"] = "Se registraron los cambios del estudiante satisfactoriamente";
                    return RedirectToAction("Index");
                }
                    }
            catch (DbEntityValidationException mex)
            {
                TempData["mensajeError"] = "No se pudo realizar la acción. Trate nuevamente, si el problema persiste contacte al administrador del sistema.";
            }
            catch (DbUpdateException e)
            {                
                TempData["mensajeError"] = "No se pudo realizar la acción. Compruebe si ya existe un estudiante con el mismo carnet registrado o si la generación existen, si el problema persiste contacte al administrador del sistema.";
            }
            catch (Exception e)
            {
                TempData["mensajeError"] = "No se pudo realizar la acción. Trate nuevamente, si el problema persiste contacte al administrador del sistema.";
            }
            ViewBag.Paises = ClaseSelect.GetInstancia().GetCountries();
            ViewBag.GeneracionId = new SelectList(db.Generacions, "Id", "Id", estudianteActualizar.GeneracionId);
            return View(estudianteActualizar);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult insertarEstudiantescsv(HttpPostedFileBase upload)
        {

            if (upload != null && upload.ContentLength > 0)
            {

                if (upload.FileName.EndsWith(".csv"))
                {
                    using (StreamReader CsvReader = new StreamReader(upload.InputStream))
                    {
                        Regex [] patrones = new Regex[18];
                        patrones[0] = new Regex(@"^Id$");
                        patrones[1] = new Regex(@"Nombre");
                        patrones[2] = new Regex(@"Pais");
                        patrones[3] = new Regex(@"Telefono");
                        patrones[4] = new Regex(@"^Correo$");
                        patrones[5] = new Regex(@"CorreoAlternativo");
                        patrones[6] = new Regex(@"Direccion");
                        patrones[7] = new Regex(@"FechaRegistro");
                        patrones[8] = new Regex(@"Apellidos");
                        patrones[9] = new Regex(@"Clave");
                        patrones[10] = new Regex(@"Sexo");
                        patrones[11] = new Regex(@"Identificacion");
                        patrones[12] = new Regex(@"Profesion");
                        patrones[13] = new Regex(@"Institucion");
                        patrones[14] = new Regex(@"Fotografia");
                        patrones[15] = new Regex(@"Estado");
                        patrones[16] = new Regex(@"GeneracionId");
                        patrones[17] = new Regex(@"Discriminator");
                        var numeroHeader = new Dictionary<int, int>(); ;
                        string inputLine = "";
                        string[] persona;
                        inputLine = CsvReader.ReadLine();
                        string[] headers = inputLine.Split(new char[] { ',' });
                        foreach (var header in headers.Select((valor, i) => new { i, valor }))
                        {
                            for (int i = 0; i < patrones.Length; i++)
                            {

                                if (patrones[i].IsMatch(header.valor))
                                {
                                    numeroHeader.Add(i, header.i);
                                    break;
                                }
                            }
                        }
                            var indicesOrdenados = numeroHeader.OrderBy(n => n.Key).Select(n => n.Value).ToList();
                        int p = 0;
                            while ((inputLine = CsvReader.ReadLine()) != null)
                            {
                                persona = inputLine.Split(new char[] { ',' });
                            
                            db.Estudiantes.Add(new Estudiante
                            {
                                Id = p.ToString(),//Cambiar
                                Nombre = persona[1],
                                Pais = persona[2],
                                Telefono = persona[3],
                                Correo = persona[4],
                                CorreoAlternativo = persona[4],//Cambiar
                                Direccion = persona[6],
                                FechaRegistro  = persona[7].Equals("nadanada")  ? DateTime.Now : DateTime.Parse(persona[7]),
                                Apellidos = persona[8],
                                Clave = persona[9],
                                Sexo = Boolean.Parse(persona[10]),
                                Identificacion = persona[11],
                                Profesion = persona[12],
                                Institucion = persona[13],
                                Fotografia = persona[14],
                                Estado = Boolean.Parse(persona[15]),
                                GeneracionId = int.Parse(persona[16])
                            }
                           );
                            p++;            
                        }
                        try
                        {
                            db.SaveChanges();
                        }
                        catch (DbEntityValidationException mex)
                        {
                            TempData["mensajeError"] = "No se pudo realizar la acción. Trate nuevamente, si el problema persiste contacte al administrador del sistema.";
                        }
                        catch (DbUpdateException e) {                            
                            TempData["mensajeError"] = "No se pudo realizar la acción. Compruebe si algún estudiante ya está registrado o si las generaciones existen, si el problema persiste contacte al administrador del sistema.";
                        }
                        catch (Exception e)
                        {
                            TempData["mensajeError"] = "No se pudo realizar la acción. Trate nuevamente, si el problema persiste contacte al administrador del sistema.";
                        }
                        CsvReader.Close();   
                    }
                }
                else
                {
                    TempData["mensajeError"] = "El archivo no tiene el formato requerido";
                }
                }
                else
                {
                TempData["mensajeError"] = "Por favor suba un archivo csv";
                }

            if(TempData["mensajeError"]==null)
                TempData["mensaje"] = "Se registraron los estudiantes satisfactoriamente";

            return RedirectToAction("Index");
        }
    
 




    }

}
