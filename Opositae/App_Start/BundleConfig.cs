using System.Web.Optimization;

namespace IdentitySample
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //BACKEND
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Vendor/jquery.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/UploadJs").Include(
            "~/Scripts/UpoloadJs/jquery-ui.min.js",
            "~/Scripts/UpoloadJs/tmpl.min.js",
             "~/Scripts/UpoloadJs/load-image.all.min.js",
            "~/Scripts/UpoloadJs/canvas-to-blob.min.js",
             "~/Scripts/UpoloadJs/jquery.blueimp-gallery.min.js",
             "~/Scripts/UpoloadJs/jquery.iframe-transport.js",
             "~/Scripts/UpoloadJs/jquery.fileupload.js",
            "~/Scripts/UpoloadJs/jquery.fileupload-process.js",
            "~/Scripts/UpoloadJs/jquery.fileupload-image.js",
            "~/Scripts/UpoloadJs/jquery.fileupload-audio.js",
            "~/Scripts/UpoloadJs/jquery.fileupload-video.js",
            "~/Scripts/UpoloadJs/jquery.fileupload-validate.js",
            "~/Scripts/UpoloadJs/jquery.fileupload-ui.js",
            "~/Scripts/UpoloadJs/jquery.fileupload-jquery-ui.js",
            "~/Scripts/UpoloadJs/main.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Vendor/tether.min.js",
                        "~/Vendor/bootstrap.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/mdk").Include(
                        "~/Vendor/dom-factory.js",
                        "~/Vendor/material-design-kit.js"));

            bundles.Add(new ScriptBundle("~/bundles/sidebarcollapse").Include(
                        "~/Vendor/sidebar-collapse.js"));

            bundles.Add(new ScriptBundle("~/bundles/appjs").Include(
                        "~/Scripts/main.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));


            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/style.css",
                      "~/Vendor/material-design-kit.css",
                      "~/Vendor/sidebar-collapse.min.css"
                      ).Include("~/Content/material-icons.css", new CssRewriteUrlTransform()).Include(
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/UploadCs").Include(
                      "~/Content/UploadCss/jquery-ui.css",
                      "~/Content/UploadCss/style.css",
                       "~/Content/UploadCss/blueimp-gallery.min.css",
                      "~/Content/UploadCss/jquery.fileupload.css",
                      "~/Content/UploadCss/jquery.fileupload-ui.css"
                      ));

            //FRONTEND
            bundles.Add(new StyleBundle("~/Content/css_font_Frontend").Include(
                    "~/Content/Frontend/font/font-icon/font-awesome-4.4.0/font-awesome.css", new CssRewriteUrlTransform()).Include(
                    "~/Content/Frontend/font/font-icon/font-svg/Glyphter.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/Content/css_Frontend").Include(
                    "~/Content/Frontend/libs/animate/animate.css",
                    "~/Content/Frontend/libs/bootstrap-3.3.5/css/bootstrap.css",
                   // "~/Content/Frontend/libs/owl-carousel-2.0/assets/owl.carousel.css",
                    "~/Content/Frontend/libs/selectbox/css/jquery.selectbox.css",
                    "~/Content/Frontend/libs/fancybox/css/jquery.fancybox.css",
                    "~/Content/Frontend/libs/fancybox/css/jquery.fancybox-buttons.css",
                    "~/Content/Frontend/libs/media-element/build/mediaelementplayer.min.css",
                    "~/Content/Frontend/css/main.css"));
            bundles.Add(new StyleBundle("~/Content/slider").Include(
                    "~/Content/Frontend/libs/animate/animate.css",
                    "~/Content/Frontend/libs/bootstrap-3.3.5/css/bootstrap.css"
                   // "~/Content/Frontend/libs/owl-carousel-2.0/assets/owl.carousel.css"
      ));

            bundles.Add(new ScriptBundle("~/Content/js_superior_Frontend").Include(
                        "~/Content/Frontend/libs/jquery/jquery-2.1.4.min.js",
                        "~/Content/Frontend/libs/js-cookie/js.cookie.js"));

            bundles.Add(new ScriptBundle("~/Content/js_Frontend").Include(
                        "~/Content/Frontend/libs/bootstrap-3.3.5/js/bootstrap.min.js",
                        "~/Content/Frontend/libs/smooth-scroll/jquery-smoothscroll.js",
                      //  "~/Content/Frontend/libs/owl-carousel-2.0/owl.carousel.min.js",
                        "~/Content/Frontend/libs/appear/jquery.appear.js",
                        "~/Content/Frontend/libs/count-to/jquery.countTo.js",
                        "~/Content/Frontend/libs/wow-js/wow.min.js",
                        "~/Content/Frontend/libs/selectbox/js/jquery.selectbox-0.2.min.js",
                        "~/Content/Frontend/libs/fancybox/js/jquery.fancybox.js",
                        "~/Content/Frontend/libs/fancybox/js/jquery.fancybox-buttons.js",
                        "~/Content/Frontend/js/main.js",
                        "~/Content/Frontend/libs/isotope/isotope.pkgd.min.js",
                        "~/Content/Frontend/libs/isotope/fit-columns.js",
                        "~/Content/Frontend/js/pages/homepage.js"));

           

               bundles.Add(new ScriptBundle("~/Content/js_fe_inicio").Include(
                        "~/Content/Frontend/js/pages/homepage.js"));

            bundles.Add(new ScriptBundle("~/Content/js_fe_contacto").Include(
                        "~/Content/Frontend/js/pages/contact.js"));

            bundles.Add(new ScriptBundle("~/Scripts/misScripts").Include(
                        "~/Scripts/misScripts.js"));
        
    

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));
        }
    }
}
