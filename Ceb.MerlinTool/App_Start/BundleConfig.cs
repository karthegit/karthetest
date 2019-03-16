using System.Web;
using System.Web.Optimization;

namespace Ceb.MerlinTool
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
                      "~/Scripts/jquery.min.js",
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/bootstrap-select.min.js",
                       "~/Scripts/kendo.all.min.js",
                       "~/Scripts/jszip.js",
                       "~/Scripts/Merlin/common.js",
                       "~/Scripts/Merlin/utilityJS.js",
                       "~/Scripts/Merlin/sharedHighchartObj.js",
                       "~/Scripts/highstock.js",
                       "~/Scripts/highcharts-more.js",
                       "~/Scripts/solid-gauge.src.js",
                       "~/Scripts/no-data-to-display.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/bootstrap-select.min.css",
                      "~/Content/font-awesome.css",
                      "~/Content/gotham.css",
                "~/Content/kendo.common.min.css",
               "~/Content/kendo.silver.min.css",
                      "~/Content/site.css"));

            BundleTable.EnableOptimizations = true;
        }
    }
}
