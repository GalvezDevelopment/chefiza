using ChefizaApi.ApiModels;
using Microsoft.Extensions.Caching.Memory;

namespace ChefizaApi.Middlewares
{
    public class TokenBlacklistMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMemoryCache _cache;
        public TokenBlacklistMiddleware(RequestDelegate next, IMemoryCache cache)
        {
            _next = next;
            _cache = cache;
        }

        public async Task Invoke(HttpContext context)
        {
            var token_string = context.Request.Headers["Authorization"].ToString();

            if (token_string != string.Empty)
            {
                var token = token_string.Replace("Bearer ", "");
                var token_blacklisted = _cache.Get(token);

                if (token_blacklisted != null && (bool)token_blacklisted == true)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsJsonAsync(new ApiResponse<string>
                    {
                        StatusCode = StatusCodes.Status401Unauthorized,
                        Success = false,
                        Message = "Invalid token."
                    });
                    return;
                }
            }

            await _next(context);
        }
    }
}