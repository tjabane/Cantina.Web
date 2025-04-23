using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Cantina.Web.Abstration
{
    public class CantinaController : ControllerBase, IActionFilter
    {
        protected string CurrentUserId { get; private set; }

        [ApiExplorerSettings(IgnoreApi = true)]
        public void OnActionExecuting(ActionExecutingContext context)
        {
            CurrentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public void OnActionExecuted(ActionExecutedContext context) {}
    }
}
