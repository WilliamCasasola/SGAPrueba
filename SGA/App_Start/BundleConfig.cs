using System.Web;
using System.Web.Optimization;

namespace SGA
{
    public class BundleConfig
    {
        // Para obtener más información sobre Bundles, visite http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {



            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
              "~/Content/themes/base/core.css",
              "~/Content/themes/base/resizable.css",
              "~/Content/themes/base/selectable.css",
              "~/Content/themes/base/accordion.css",
              "~/Content/themes/base/autocomplete.css",
              "~/Content/themes/base/button.css",
              "~/Content/themes/base/dialog.css",
              "~/Content/themes/base/slider.css",
              "~/Content/themes/base/tabs.css",
              "~/Content/themes/base/datepicker.css",
              "~/Content/themes/base/progressbar.css",
              "~/Content/themes/base/theme.css"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
            "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
            // preparado para la producción y podrá utilizar la herramienta de compilación disponible en http://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
        }
    }
}
