using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace selcukunikonutlari
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });
            routes.IgnoreRoute("{*allaspx}", new { allaspx = @".*\.aspx(/.*)?" });
            routes.IgnoreRoute("{*robotstxt}", new { robotstxt = @"(.*/)?robots.txt(/.*)?" });




           
            //başı y iile başlayanlar yonetim sayfası için
            routes.MapRoute(
              name: "yidaction",
              url: "Yonetim/{id}/{action}.html",
              defaults: new { controller = "Yonetim", action = "", id = "" }
          );

            routes.MapRoute(
               name: "ysayfaaction",
               url: "Yonetim/sayfalar/{sayfa}/{action}.html",
               defaults: new { controller = "Yonetim", action = "", sayfa = 1 }
           );

            routes.MapRoute(
               name: "yaction",
               url: "Yonetim/{action}.html",
               defaults: new { controller = "Yonetim", action = "" }
           );
            routes.MapRoute(
               name: "yidislemaction",
               url: "Yonetim/{id}/islem/{islem}/{action}.html",
               defaults: new { controller = "Yonetim", action = "", id = "", islem = "" }
           );




            routes.MapRoute(
             name: "kisimuhasebegiris",
             url: "{action}/kisi-muhasebe-giris.html",
             defaults: new { controller = "index", action = "KisiMuhasebeGiris" }
            );
            routes.MapRoute(
                name: "idaction",
                url: "{id}/{action}.html",
                defaults: new { controller = "index", action = "", id = 0 }
            );
            routes.MapRoute(
                name: "action",
                url: "{action}.html",
                defaults: new { controller = "index", action = "" }
            );

            routes.MapRoute(
                name: "sayfaaction",
                url: "Anasayfa/{sayfa}/{action}.html",
                defaults: new { controller = "index", action = "", sayfa = 1 }
            );

            routes.MapRoute(
                name: "anasayfalar",
                url: "{action}.html",
                defaults: new { controller = "index", action = "", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "defaultsayfa",
                url: "{controller}/{action}/{sayfa}",
                defaults: new { controller = "index", action = "AnaSayfa", sayfa = UrlParameter.Optional }
            );
           
            routes.MapRoute(
               name: "Default",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "index", action = "AnaSayfa",id=UrlParameter.Optional }
           );

            
        }
    }
}