using System.Web;
using System.Web.Optimization;

namespace BARTPerks
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/javascript").Include(
                      "~/Scripts/drupal-jquery-1.10.2.js",
                      "~/Scripts/bart-perk.js",
                      "~/Scripts/site.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/css-1.css",
                      "~/Content/css-2.css",
                      "~/Content/css-3.css",
                      "~/Content/css-4.css",
                      "~/Content/css-5.css",
                      "~/Content/site.css"));
        }
    }
}
