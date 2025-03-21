using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HelpdeskApp.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
            {
                context.Result = RedirectToAction("Login", "Account");
            }
            base.OnActionExecuting(context);
        }
    }
}
