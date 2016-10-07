using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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


        /*        public string guardarArchivo(string codigo ,HttpPostedFileBase archivo, string ruta)
                {
                    if (archivo == null)
                    {
                        if (ruta.Equals("~/Imagenes/Perfil/"))
                            return "noperfil.jpg";
                        else
                            return "nodocumento.png";
                    }
                    if (archivo.FileName.Equals("noperfil.jpg") || archivo.FileName.Equals("nodocumento.png"))
                        return archivo.FileName;

                    if (!Regex.IsMatch(archivo.FileName, @"(?i).*\.(gif|jpe?g|png|bmp)$") && archivo.ContentLength <= 5000000)
                        return "algo.doc";
                    if (archivo.ContentLength > 5000000)
                        return new string(Enumerable.Repeat("o", 300).Select(s=>s[new Random().Next(s.Length)]).ToArray());//Para que genere el error predefinido en la clase por tamaño de string aunque el tamaño que excede es el del archivo

                    archivo.SaveAs(HttpContext.Current.Server.MapPath(ruta)
                                                          + codigo + archivo.FileName);
                    return codigo + archivo.FileName;
                    //img.ImagePath = archivo.FileName;

               // db.Image.Add(img);
                }*/

        public string guardarArchivo(string codigo, HttpPostedFileBase archivo, string ruta)
        {
            if (archivo == null)
            {
                if (ruta.Equals("~/Imagenes/Perfil/"))
                    return "noperfil.jpg";
                else
                if (ruta.Equals("~/Imagenes/Documento/"))
                    return "nodocumento.png";
                else
                    return "noPortada.jpg";
            }
            if (archivo.FileName.Equals("noperfil.jpg") || archivo.FileName.Equals("nodocumento.png") || archivo.FileName.Equals("noPortada.jpg"))
                return archivo.FileName;

            if (!Regex.IsMatch(archivo.FileName, @"(?i).*\.(gif|jpe?g|png|bmp)$") && archivo.ContentLength <= 5000000)
                return "algo.doc";
            if (archivo.ContentLength > 5000000)
                return new string(Enumerable.Repeat("o", 300).Select(s => s[new Random().Next(s.Length)]).ToArray());//Para que genere el error predefinido en la clase por tamaño de string aunque el tamaño que excede es el del archivo

            archivo.SaveAs(HttpContext.Current.Server.MapPath(ruta)
                                                  + codigo + archivo.FileName);
            return codigo + archivo.FileName;
            //img.ImagePath = archivo.FileName;

            // db.Image.Add(img);
        }

    }
}