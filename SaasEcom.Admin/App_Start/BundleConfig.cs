using System.Web.Optimization;

namespace SaasEcom.Admin
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                "~/Scripts/angular.js",
                "~/Scripts/angular-*",
                "~/Scripts/ui-bootstrap-*",
                "~/Scripts/loading-bar.js",
                "~/Scripts/toaster.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                "~/Scripts/app/app.js",
                "~/Scripts/app/controllers/*.js",
                "~/Scripts/app/directives/*.js",
                "~/Scripts/app/filters/*.js",
                "~/Scripts/app/services/*.js",
                "~/Scripts/respond.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            // Admin styles
            bundles.Add(new LessBundle("~/Content/css").Include(
                "~/Content/fontawesome/font-awesome.less",
                "~/Content/admin.less"));
        }
    }
}
