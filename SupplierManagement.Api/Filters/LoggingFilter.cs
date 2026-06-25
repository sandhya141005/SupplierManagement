using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
namespace SupplierManagement.Api.Filters; 
public class LoggingFilter : IExceptionFilter
{
    private readonly ILogger<LoggingFilter> _logger;
    public LoggingFilter(ILogger<LoggingFilter> logger)
    {
        _logger=logger;
    }

    public void OnException(ExceptionContext context)
    {
        _logger.LogError(context.Exception,"LOGGING FILTERRRR:Exception not handled; Method:{Method}, Path:{Path}, Time:{Time}",
        context.HttpContext.Request.Method,context.HttpContext.Request.Path,DateTime.Now);
        
        context.Result=new ObjectResult(new
        {
            error="Log-Exception result-error occured",
            detail=context.Exception.Message,
            path=context.HttpContext.Request.Path.ToString(),
            timestamp=DateTime.Now
        })
        {
            StatusCode=500
        };
        context.ExceptionHandled=true;
    }
}