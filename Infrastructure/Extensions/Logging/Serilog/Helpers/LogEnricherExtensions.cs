using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Routing;
using Serilog;

namespace SuperMarket.Infrastructure.Extensions.Logging;

public static class LogEnricherExtensions
{
    public static void EnrichFromRequest(IDiagnosticContext diagnosticContext, HttpContext httpContext)
    {
        diagnosticContext.Set("UserName", httpContext?.User?.Identity?.Name);
        diagnosticContext.Set("UserAgent", httpContext?.Request?.Headers["User-Agent"].FirstOrDefault());
        diagnosticContext.Set("Resource", httpContext?.GetMetricsCurrentResourceName());
    }

    public static string GetMetricsCurrentResourceName(this HttpContext httpContext)
    {
        if (httpContext == null)
            throw new ArgumentNullException(nameof(httpContext));

        var endpoint = httpContext.Features?.Get<IEndpointFeature>()?.Endpoint;

        if (endpoint != null && endpoint.Metadata != null)
        {
            var endpointNameMetadata = endpoint?.Metadata?.GetMetadata<EndpointNameMetadata>();
            if (endpointNameMetadata != null)
            {
                return endpointNameMetadata.EndpointName;
            }
        }

        return "(context metadata no found)";
    }
}