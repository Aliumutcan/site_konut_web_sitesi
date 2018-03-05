using Elmah;
using System.Web;
using System.Web.Mvc;

namespace selcukunikonutlari
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new ElmahHandledErrorLoggerFilter());
        }
    }
    public class ElmahHandledErrorLoggerFilter : System.Web.Mvc.IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.ExceptionHandled)
                ErrorSignal.FromCurrentContext().Raise(context.Exception);
        }
    }
}