using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;


namespace selcukunikonutlari.Controllers.Araclar
{
    public class MuhasebeciGirisKontrol: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            base.OnActionExecuting(filterContext);

            if (HttpContext.Current.Session["muhasebegiris"] == null)
            {
                filterContext.Result = new RedirectToRouteResult("yaction", new RouteValueDictionary(new
                {
                    action = "Giris"
                }));
                return;
            }
        }
    }
}