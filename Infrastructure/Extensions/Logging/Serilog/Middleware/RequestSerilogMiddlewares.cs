using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Serilog.Context;

namespace SuperMarket.Infrastructure.Extensions.Logging;

public class RequestSerilogMiddleware
{
    private readonly RequestDelegate _next;

    public RequestSerilogMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task Invoke(HttpContext context)
    {
        using (LogContext.PushProperty("UserName", context?.User?.Identity?.Name ?? "(annonimous"))
        using (LogContext.PushProperty("CorrelationId", GetCorrelationId(context)))
        {
            return _next.Invoke(context);
        }
    }

    private string GetCorrelationId(HttpContext httpContext)
    {
        httpContext.Request.Headers.TryGetValue("Cko-Correlation-Id", out StringValues correlationId);
        return correlationId.FirstOrDefault() ?? httpContext.TraceIdentifier;
    }
}