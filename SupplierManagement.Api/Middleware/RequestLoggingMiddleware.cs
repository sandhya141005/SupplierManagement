using System.Diagnostics;
namespace SupplierManagement.Api.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next,
        ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var method = context.Request.Method;
        var path = context.Request.Path;
        var queryString = context.Request.QueryString;
        _logger.LogInformation(
            "REQUEST INNNNN {Method} {Path}{Query}",
            method, path, queryString);
        await _next(context);
        stopwatch.Stop();
        var statusCode = context.Response.StatusCode;
        var elapsed = stopwatch.ElapsedMilliseconds;
        var level = statusCode >= 500 ? LogLevel.Error
                  : statusCode >= 400 ? LogLevel.Warning
                  : LogLevel.Information;
        _logger.Log(level,
            "REQUEST OUTTTTTT {Method} {Path} | Status: {StatusCode} | Time: {Elapsed}ms",
            method, path, statusCode, elapsed);
    }
}