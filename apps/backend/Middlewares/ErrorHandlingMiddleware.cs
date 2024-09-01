using System.Net;
using System.Text.Json;
using ChefizaApi.ApiModels;

namespace ChefizaApi.Middlewares {
    public class ErrorHandlingMiddleware {
        public readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger, RequestDelegate request) {
            _logger = logger;
            _next = request;
        }

        public async Task InvokeAsync(HttpContext context) {
            try {
                await _next(context);
            } catch (Exception ex) {
                await HandleExceptionAsync(context, ex);
            }
        }

        public Task HandleExceptionAsync(HttpContext context, Exception ex) {
            _logger.LogError(ex, "An error occurred");

            var response = new ApiResponse<string> {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = "An error ocurred",
                Success = false
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var jsonResponse = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(jsonResponse);
        }
    }
}