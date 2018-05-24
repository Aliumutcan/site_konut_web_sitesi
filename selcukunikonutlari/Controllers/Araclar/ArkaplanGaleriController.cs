using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace selcukunikonutlari.Controllers.Araclar
{
    public class ArkaplanGaleriController : ActionFilterAttribute
    {


        Models.Model1 context = new Models.Model1();
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (context.Database.Exists())
            {
                HttpContext.Current.Session["arkaplan"] = context.Galeri.Where(x => x.arkaplan == true).ToList();
            }
            
        }
    }
}
