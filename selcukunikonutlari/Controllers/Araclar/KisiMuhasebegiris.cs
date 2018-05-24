using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace selcukunikonutlari.Controllers.Araclar
{
    public class KisiMuhasebegiris: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            base.OnActionExecuting(filterContext);

            if (HttpContext.Current.Session["kisimuhasebegiris"] == null)
            {
                filterContext.Result = new RedirectToRouteResult("kisimuhasebegiris", new RouteValueDictionary(
                    new {
                        action = "KisiMuhasebeGiris"
                    }
                    ));
                
            }
        }
    }
}