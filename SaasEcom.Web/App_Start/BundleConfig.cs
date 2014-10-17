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
            bundles.Add(new ScriptBundle("~/bundles/jquery", "http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.11.0.min.js")
                .Include("~/Scripts/jquery-{version}.js", "~/Scripts/jquery.confirm.js"));

            // jQuery confirm
            bundles.Add(new ScriptBundle("~/bundles/jquery.confirm")
                .Include("~/Scripts/jquery.confirm.js"));

            // Angular JS
            bundles.Add(new ScriptBundle("~/bundles/angularjs", "http://ajax.googleapis.com/ajax/libs/angularjs/1.2.16/angular.min.js")
                .Include("~/Scripts/angular.js"));

            // Angular UI Bootstrap
            bundles.Add(new ScriptBundle("~/bundles/angular-ui-bootstrap").Include("~/Scripts/angular-ui/ui-bootstrap-tpls.js"));

            // Angular ng-table
            bundles.Add(new ScriptBundle("~/bundles/angular-ng-table")
                .Include("~/Scripts/angular-ng-table.min.js"));

            // Modernizr
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr", "http://ajax.aspnetcdn.com/ajax/modernizr/modernizr-2.6.2.js")
                .Include("~/Scripts/modernizr-2.6.2.js"));
            
            // POST: Validation
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate.min.js",
                "~/Scripts/jquery.validate.unobtrusive.min.js"));

            // Site Scripts
            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                "~/Scripts/bootstrap.min.js",
                "~/Scripts/respond.js",
                "~/Scripts/site.js"));

            // Billing Scripts
            bundles.Add(new ScriptBundle("~/bundles/billing/js").Include(
                "~/Scripts/bootstrap.min.js",
                "~/Scripts/chosen.jquery.min.js",
                "~/Scripts/respond.js",
                "~/Scripts/billing.js"));

            // Dashboard Scripts
            bundles.Add(new ScriptBundle("~/bundles/dashboard/js").Include(
                "~/Scripts/bootstrap.min.js",
                "~/Scripts/respond.js"));

            // Dashboard Stripe card form
            bundles.Add(new ScriptBundle("~/bundles/dashboard/stripe-card").Include(
                "~/Scripts/dashboard/card-form.js"));

            // Site CSS
            bundles.Add(new LessBundle("~/bundles/css").Include(
                "~/Content/site.less"));

            // Billing CSS
            bundles.Add(new LessBundle("~/bundles/billing/css").Include(
                "~/Content/billing/billing.less", 
                "~/Content/billing/ng-table.less",
                "~/Content/billing/chosen.css"));
            
            // Dashboard CSS
            bundles.Add(new LessBundle("~/bundles/dashboard/css").Include(
                "~/Content/dashboard/dashboard.less"));

            // Font awesome
            bundles.Add(new LessBundle("~/bundles/fontawesome").Include("~/Content/fontawesome/font-awesome.less"));
        }
    }
}
