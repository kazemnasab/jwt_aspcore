using System;
using Microsoft.AspNetCore.Http;

namespace JwtApp
{
    public class AuthMiddleware
    {
        public AuthMiddleware()
        {

        }
        private readonly RequestDelegate _next;

        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {

            var role = context.User.Claims.FirstOrDefault(m=> m.Type == "Role").Value;
            context.Items["Role"] = role;
            await _next(context);
        }
    }
}

