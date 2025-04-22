using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Cantina.Web.Abstration
{
    public class CantinaController : ControllerBase, IActionFilter
    {
        protected string CurrentUserId { get; private set; }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            CurrentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        public void OnActionExecuted(ActionExecutedContext context) {}
    }
}
