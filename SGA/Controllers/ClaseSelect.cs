using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace SGA.Controllers
{//En esta clase se crea el select de los paises
    public class ClaseSelect
    {
        public static ClaseSelect instancia;

        public static ClaseSelect GetInstancia()
        {
            lock (typeof(ClaseSelect))
            {
                instancia = new ClaseSelect();
            }
            return instancia;
        }
        public IEnumerable<SelectListItem> GetCountries()
        {
            RegionInfo country = new RegionInfo(new CultureInfo("en-US", false).LCID);
            List<SelectListItem> countryNames = new List<SelectListItem>();

            //To get the Country Names from the CultureInfo installed in windows
            foreach (CultureInfo cul in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
            {
                country = new RegionInfo(new CultureInfo(cul.Name, false).LCID);
                countryNames.Add(new SelectListItem() { Text = country.DisplayName, Value = country.DisplayName });
            }

            //Assigning all Country names to IEnumerable
            IEnumerable<SelectListItem> nameAdded = countryNames.GroupBy(x => x.Text).Select(x => x.FirstOrDefault()).ToList<SelectListItem>().OrderBy(x => x.Text);
            return nameAdded;
        }


        public string guardarArchivo(string codigo ,HttpPostedFileBase archivo)
        {
            archivo.SaveAs(HttpContext.Current.Server.MapPath("~/Imagenes/Perfil/")
                                                  + codigo + archivo.FileName);
            return codigo + archivo.FileName;
            //img.ImagePath = archivo.FileName;
        
       // db.Image.Add(img);
        }
    }
}