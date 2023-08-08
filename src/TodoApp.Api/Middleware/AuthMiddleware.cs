using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Json;
using TodoApp.Core.Models.Response;

namespace TodoApp.Api.Middleware
{
    public class AuthMiddleware : IAuthorizationMiddlewareResultHandler
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public AuthMiddleware(IOptions<JsonOptions> jsonOptions)
        {
            _jsonSerializerOptions = jsonOptions.Value.JsonSerializerOptions;
        }

        public async Task HandleAsync(
            RequestDelegate requestDelegate,
            HttpContext httpContext,
            AuthorizationPolicy authorizationPolicy,
            PolicyAuthorizationResult policyAuthorizationResult)
        {
            if (!policyAuthorizationResult.Succeeded)
            {
                var res = new ApiResponse<object>
                {
                    Code = "UNAUTHORIZED",
                    Message = "Invalid Credentials"
                };

                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                httpContext.Response.ContentType = "application/json";

                await httpContext.Response.WriteAsync(JsonSerializer.Serialize(res, _jsonSerializerOptions));
            }
            else
            {
                await requestDelegate(httpContext);
            }
        }

    }
}