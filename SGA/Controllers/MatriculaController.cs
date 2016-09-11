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

namespace SGA.Controllers
{
    public class MatriculaController : Controller
    {
        private SGAContext db = new SGAContext();
        private static ICollection<Nota> notas;

        // GET: Matricula
        public ActionResult Index()
        {
            CursoNota curosNota = new CursoNota();
            curosNota.Matriculas=  db.Matriculas
                .Include(m => m.Curso.Titulo)
                .Include(m => m.Estudiante).ToList();
            curosNota.Notas=db.Matriculas.Select(m => m.Calificaciones.Select(c => c.Valor).Sum() / m.Curso.CantidadEvaluaciones).ToList();
            return View(curosNota);
        }

        // GET: Matricula/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Matricula matricula = db.Matriculas.Find(id);
            if (matricula == null)
            {
                return HttpNotFound();
            }
            return View(matricula);
        }

        private void crearSelect(Matricula matricula) {
            ViewBag.CursoID = new SelectList(db.Cursos, "Id", "TituloId", matricula.CursoID);
            ViewBag.EstudianteID = new SelectList(db.Clientes.SqlQuery("SELECT * FROM Cliente WHERE Discriminator = 'Estudiante'"), "Id", "Nombre", matricula.EstudianteID);
        }

        // GET: Matricula/Create
        public ActionResult Create()
        {
            ViewBag.CursoID = new SelectList(db.Cursos, "Id", "TituloId");
            ViewBag.EstudianteID = new SelectList(db.Clientes.SqlQuery("SELECT * FROM Cliente WHERE Discriminator = 'Estudiante'"), "Id", "Nombre");
            return View();
        }

        // POST: Matricula/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CursoID,EstudianteID")] Matricula matricula)
        {
            if (ModelState.IsValid)
            {
                db.Matriculas.Add(inicializar(matricula));
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            crearSelect(matricula);
            return View(matricula);
        }

        // GET: Matricula/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Matricula matricula = db.Matriculas.Include(m=>m.Calificaciones).Where(m=>m.ID==id).Single();
            notas = matricula.Calificaciones;
            if (matricula == null)
            {
                return HttpNotFound();
            }
            crearSelect(matricula);
            return View(matricula);
        }

        private Matricula inicializar(Matricula matricula) {
            int c = db.Cursos.Find(matricula.CursoID).CantidadEvaluaciones;
            matricula.Calificaciones = new List<Nota>();
            if (c == 1)
            {
                matricula.Calificaciones.Add(new Nota { Valor = 0, Tipo = "Trabajo Final" });
            }
            else
            {
                for (int i = 0; i < c; i++)
                {
                    matricula.Calificaciones.Add(new Nota { Valor = 0, Tipo = "Tarea "+ (i+1) });
                }
            }
            matricula.DiaMatricula = DateTime.Now;
            return matricula;
        }

        // POST: Matricula/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CursoID,EstudianteID,DiaMatricula")] Matricula matricula,  string[] valores, string[] tipos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(matricula).State = EntityState.Modified;
                db.SaveChanges();
                Dispose(true);
                db = new SGAContext();
                foreach (var item in notas.Select((nota,i)=>new {Indice=i, Nota=nota })) {
                    item.Nota.Tipo = tipos[item.Indice];
                    item.Nota.Valor = Convert.ToDouble(valores[item.Indice]);
                    db.Entry(item.Nota).State = EntityState.Modified;
                    
                }
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            crearSelect(matricula);
            return View(matricula);
        }

        // GET: Matricula/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Matricula matricula = db.Matriculas.Find(id);
            if (matricula == null)
            {
                return HttpNotFound();
            }
            return View(matricula);
        }

        // POST: Matricula/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Matricula matricula = db.Matriculas.Find(id);
            db.Matriculas.Remove(matricula);
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
