using Microsoft.AspNetCore.Mvc.Filters;
namespace SupplierManagement.Api.Filters;
public class ResponseFilter : IResultFilter
{
    private readonly ILogger<ResponseFilter> _logger;

    public ResponseFilter(ILogger<ResponseFilter> logger)
    {
        _logger = logger;
    }
    public void OnResultExecuting(ResultExecutingContext context)
    {
      
        _logger.LogInformation(
            "RESPONSE FILTERRRRR;Sending response for {Method} {Path}",
            context.HttpContext.Request.Method,
            context.HttpContext.Request.Path);
    }
    public void OnResultExecuted(ResultExecutedContext context)
    { _logger.LogInformation(
            "RESPONSE FILTERRR:Response sent, Status:{StatusCode},Method: {Method},Path: {Path},Time: {Time}",
            context.HttpContext.Response.StatusCode,
            context.HttpContext.Request.Method,
            context.HttpContext.Request.Path,
            DateTime.Now);
    }
}