using System.Web.Optimization;

namespace SaasEcom.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // TODO: Set to true for production
            BundleTable.EnableOptimizations = true;
            bundles.UseCdn = true;

            // jQuery
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-{version}.js"));

            // Modernizr
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include("~/Scripts/modernizr-2.6.2.js"));
            
            // POST: Validation
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate.unobtrusive.min.js",
                "~/Scripts/jquery.validate.min.js"));

            // Site Scripts
            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                "~/Scripts/bootstrap.min.js",
                "~/Scripts/respond.js",
                "~/Scripts/site.js"));

            // Billing Scripts
            bundles.Add(new ScriptBundle("~/bundles/billing/js").Include(
                "~/Scripts/bootstrap.min.js",
                "~/Scripts/respond.js",
                "~/Scripts/billing.js"));

            // Dashboard Scripts
            bundles.Add(new ScriptBundle("~/bundles/dashboard/js").Include(
                "~/Scripts/bootstrap.min.js",
                "~/Scripts/respond.js",
                "~/Scripts/dashboard.js"));


            // Site CSS
            bundles.Add(new LessBundle("~/bundles/css").Include(
                "~/Content/bootstrap/bootstrap.less",
                "~/Content/site.less"));

            // Billing CSS
            bundles.Add(new LessBundle("~/bundles/billing/css").Include(
                "~/Content/bootstrap/bootstrap.less",
                "~/Content/billing/billing.less"));
            
            // Dashboard CSS
            bundles.Add(new LessBundle("~/bundles/dashboard/css").Include(
                "~/Content/bootstrap/bootstrap.less",
                "~/Content/dashboard/dashboard.less"));

            // Font awesome
            bundles.Add(new LessBundle("~/bundles/fontawesome").Include("~/Content/fontawesome/font-awesome.less"));
        }
    }
}
