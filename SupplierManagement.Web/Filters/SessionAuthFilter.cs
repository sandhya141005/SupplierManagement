using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SupplierManagement.Web.Filters;

public class SessionAuthFilter : IAuthorizationFilter
{
    private static readonly HashSet<string> AnonymousControllers = new(StringComparer.OrdinalIgnoreCase)
    {
        "Auth", "Home"
    };
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var controllerName = context.RouteData.Values["controller"]?.ToString() ?? "";

        if (AnonymousControllers.Contains(controllerName))
            return;

        var role = context.HttpContext.Session.GetString("Role");

        if (string.IsNullOrEmpty(role))
        {
            context.Result = new RedirectToActionResult("AccessDenied", "Auth", null);
            return;
        }

        if (controllerName.Equals("Supplier", StringComparison.OrdinalIgnoreCase))
        {
            var actionName = context.RouteData.Values["action"]?.ToString() ?? "";
            var adminOnlyActions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            { "Add", "Edit", "Delete", "Analytics" };

            if (role != "Admin" && adminOnlyActions.Contains(actionName))
            {
                context.Result = new RedirectToActionResult("AccessDenied", "Auth", null);
            }
        }
    }
}