using Microsoft.AspNetCore.Mvc.Filters;

namespace EmployeeManagementSystem.Action_Atributes
{
    public class NoCacheAttribute: ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var response = context.HttpContext.Response;
            response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            response.Headers["Expires"] = "0";
            response.Headers["Pragma"] = "no-cache";
            
            base.OnResultExecuting(context);
        }
    }
}
