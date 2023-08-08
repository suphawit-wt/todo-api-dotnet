using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TodoApp.Core.Exceptions;
using TodoApp.Core.Models.Response;

namespace TodoApp.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public ExceptionMiddleware(RequestDelegate next, IOptions<JsonOptions> jsonOptions)
        {
            _next = next;
            _jsonSerializerOptions = jsonOptions.Value.JsonSerializerOptions;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (AppException ex)
            {
                var res = new ApiResponse<object>
                {
                    Code = ex.Code,
                    Message = ex.Message,
                    Errors = ex.Errors
                };

                context.Response.StatusCode = ex.StatusCode;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(JsonSerializer.Serialize(res, _jsonSerializerOptions));
            }
            catch (Exception)
            {
                var res = new ApiResponse<object>()
                {
                    Code = "UNEXPECTED",
                    Message = "Unexpected error",
                };

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(JsonSerializer.Serialize(res, _jsonSerializerOptions));
            }
        }

    }
}