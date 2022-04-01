using System.Net;
using System.Text.Json;
using Application.Core;
using Microsoft.AspNetCore.Http;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;
        public ExceptionMiddleware(RequestDelegate next, 
            ILogger<ExceptionMiddleware> logger,
            IHostEnvironment env) {
            _env = env;
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context) {
            try {
                // simply pass a request from this middleware to the next middleware
                await _next(context); 
            } catch (Exception ex) {
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                // set the status code to 500
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

                // check if we are in Development or Production mode
                var response = _env.IsDevelopment() 
                    ? new AppException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
                    : new AppException(context.Response.StatusCode, "Server Error");

                // here we want to transform our response to JSON format
                var options = new JsonSerializerOptions{ PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(response, options);
            
                await context.Response.WriteAsync(json);
            }
        }
    }
}