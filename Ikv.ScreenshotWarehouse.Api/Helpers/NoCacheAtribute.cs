using Microsoft.AspNetCore.Mvc.Filters;

namespace Ikv.ScreenshotWarehouse.Api.Helpers
{
    public class NoCacheAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            context.HttpContext.Response.Headers["Cache-Control"] = "no-cache";
            context.HttpContext.Response.Headers["Pragma"] = "no-cache";
            context.HttpContext.Response.Headers["Expires"] = "0";
            base.OnResultExecuting(context);
        }
    }
}