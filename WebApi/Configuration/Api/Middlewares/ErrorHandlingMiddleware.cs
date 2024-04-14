using Microsoft.AspNetCore.Mvc;
using Serilog;
using SuperMarket.Infrastructure.Framework.Responses;
using SuperMarket.Infrastructure.Framework.Security;
using SuperMarket.Infrastructure.Framework.Validations;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.Json;

namespace SuperMarket.WebApi.Configuration.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                var response = httpContext.Response;
                response.ContentType = "application/json";

                var responseWrapper = await ResponseWrapper.FailAsync(ex.Message);

                switch (ex)
                {
                    case CustomValidationException vex:
                        Log.Error(vex.InnerException ?? vex, vex.FriendlyErrorMessage);
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case CustomAuthException aex:
                        Log.Error(aex.InnerException ?? aex, aex.FriendlyErrorMessage);
                        response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        break;
                    default:
                        Log.Error(ex.InnerException ?? ex, ex.Message);
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                var result = JsonSerializer.Serialize(responseWrapper);
                await response.WriteAsync(result);
            }
        }
    }
}