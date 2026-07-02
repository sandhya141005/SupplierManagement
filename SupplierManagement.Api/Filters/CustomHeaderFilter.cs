using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SupplierManagement.Api.Filters;

public class CustomHeaderFilter : IActionFilter
{
    private readonly ILogger<CustomHeaderFilter> _logger;
    private const string RequiredHeader = "X-Api-Key";
    private const string ExpectedValue = "SupplierHub'26";
    public CustomHeaderFilter(ILogger<CustomHeaderFilter> logger)
    {
        _logger = logger;
    }
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var controller = context.RouteData.Values["controller"]?.ToString();

        if (controller == "Ai" || controller == "Test")
        {
            return;
        }
        context.HttpContext.Request.Headers.TryGetValue(RequiredHeader, out var value);

        _logger.LogInformation(
            "Received Header: '{Value}', Expected: '{Expected}'",
            value.ToString(),
            ExpectedValue);

        if (string.IsNullOrEmpty(value) || value != ExpectedValue)
        {

            _logger.LogWarning(
                "HEADER FILTERRRRRRRRR: missing/invalid {Header} on {Method} {Path}",
                RequiredHeader, context.HttpContext.Request.Method, context.HttpContext.Request.Path);
            //tis for for returning 401
            context.Result = new UnauthorizedObjectResult(new
            {
                error = "Invalid or missing API key",
                header = RequiredHeader,
                path = context.HttpContext.Request.Path.ToString()
            });
        }
        else
        {
            _logger.LogInformation(
                "HEADER FILTERRRRRR: Accepted — valid {Header} on {Method} {Path}",
                RequiredHeader, context.HttpContext.Request.Method, context.HttpContext.Request.Path);
        }
    }
    public void OnActionExecuted(ActionExecutedContext context) { }
}