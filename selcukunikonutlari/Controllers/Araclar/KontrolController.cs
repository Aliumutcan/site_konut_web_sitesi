using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace selcukunikonutlari.Controllers.Araclar
{
    public class KontrolController : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            base.OnActionExecuting(filterContext);

            if (HttpContext.Current.Session["giris"] == null)
            {
                filterContext.Result = new RedirectToRouteResult("ysayfaaction", new RouteValueDictionary(new
                {
                    action = "Giris"
                }));
                return;
            }
        }
    }
}
