using System.Web;
using System.Web.Optimization;

namespace selcukunikonutlari
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/Content/css/ie7.css",
                        "~/Content/css/ie8.css",
                        "~/Content/css/ie9.css",
                        "~/Content/css/jquery.slider.css",
                        "~/Content/css/prettyphoto.css",
                        "~/Content/css/style.css"));

            bundles.Add(new StyleBundle("~/Content/js").Include(
                        "~/Content/js/carousel.js",
                        "~/Content/js/ddsmoothmenu.js",
                        "~/Content/js/jquery-1.6.2.min.js",
                        "~/Content/js/jquery-1.6.2.min.js",
                        "~/Content/js/jquery.jcarousel.js",
                        "~/Content/js/jquery.masonry.min.js",
                        "~/Content/js/jquery.prettyPhoto.js",
                        "~/Content/js/jquery.slickforms.js",
                        "~/Content/js/jquery.superbgimage.min.js",
                        "~/Content/js/script.js",
                        "~/Content/js/transify.css"));
        }
    }
}