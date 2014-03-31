using System.Web.Optimization;
using BundleTransformer.Core.Builders;
using BundleTransformer.Core.Orderers;
using BundleTransformer.Core.Transformers;

namespace SaasEcom.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // TODO: Set to true for production
            BundleTable.EnableOptimizations = false;

            // Variables
            bundles.UseCdn = true;
            var nullBuilder = new NullBuilder();
            var cssTransformer = new CssTransformer();
            var jsTransformer = new JsTransformer();
            var nullOrderer = new NullOrderer();

            #region JavaScript

            // jQuery
            var jquery = new Bundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js");
            jquery.Builder = nullBuilder;
            jquery.Transforms.Add(jsTransformer);
            jquery.Orderer = nullOrderer;
            bundles.Add(jquery);

            // Modernizr
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            var modernizr = new Bundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-2.6.2.js");
            modernizr.Builder = nullBuilder;
            modernizr.Transforms.Add(jsTransformer);
            modernizr.Orderer = nullOrderer;
            bundles.Add(modernizr);
            
            // POST: Validation
            var jqueryVal = new Bundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate.unobtrusive.min.js",
                "~/Scripts/jquery.validate.min.js");
            jqueryVal.Builder = nullBuilder;
            jqueryVal.Transforms.Add(jsTransformer);
            jqueryVal.Orderer = nullOrderer;
            bundles.Add(jqueryVal);

            // Site Scripts
            var js = new Bundle("~/bundles/js").Include(
                "~/Scripts/bootstrap.min.js",
                "~/Scripts/respond.js",
                "~/Scripts/site.js");
            js.Builder = nullBuilder;
            js.Transforms.Add(jsTransformer);
            js.Orderer = nullOrderer;
            bundles.Add(js);

            // Billing Scripts
            var billingJs = new Bundle("~/bundles/js").Include(
                "~/Scripts/bootstrap.min.js",
                "~/Scripts/respond.js",
                "~/Scripts/billing.js");
            billingJs.Builder = nullBuilder;
            billingJs.Transforms.Add(jsTransformer);
            billingJs.Orderer = nullOrderer;
            bundles.Add(billingJs);

            #endregion

            #region CSS Styles

            // Site CSS
            var css = new Bundle("~/bundles/css").Include(
                "~/Content/bootstrap/bootstrap.less",
                "~/Content/site.less");
            css.Builder = nullBuilder;
            css.Transforms.Add(cssTransformer);
            css.Transforms.Add(new CssMinify());
            css.Orderer = nullOrderer;
            bundles.Add(css);

            // Billing CSS
            var billingCss = new Bundle("~/bundles/billing/css").Include(
                "~/Content/bootstrap/bootstrap.less",
                "~/Content/billing/billing.less");
            billingCss.Builder = nullBuilder;
            billingCss.Transforms.Add(cssTransformer);
            billingCss.Transforms.Add(new CssMinify());
            billingCss.Orderer = nullOrderer;
            bundles.Add(billingCss);

            // Font awesome
            var fa = new Bundle("~/bundles/fontawesome").Include(
                "~/Content/fontawesome/font-awesome.less");
            fa.Builder = nullBuilder;
            fa.Transforms.Add(cssTransformer);
            fa.Transforms.Add(new CssMinify());
            fa.Orderer = nullOrderer;
            bundles.Add(fa);

            #endregion
        }
    }
}
