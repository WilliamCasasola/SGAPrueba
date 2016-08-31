using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SGA.ViewModels;
using SGA.DAL;

namespace SGA.Controllers
{
    public class HomeController : Controller
    {
        private SGAContext db = new SGAContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
             IQueryable<GrupoMatriculaDia> datos = from estudiante in db.Estudiantes
                                                   group estudiante by estudiante.Fechacontratacion into grupoFecha
                                                   select new GrupoMatriculaDia()
                                                   {
                                                       diaMatricula = grupoFecha.Key,
                                                       contadorEstudiantes = grupoFecha.Count()
                                                   };

            return View(datos.ToList());
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}