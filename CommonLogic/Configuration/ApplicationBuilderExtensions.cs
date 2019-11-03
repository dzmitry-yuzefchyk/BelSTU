using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Http;

namespace CommonLogic.Configuration
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseJWTAuth(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                var token = context.Request.Cookies[".AspNetCore.Application.Id"];
                if (!string.IsNullOrEmpty(token))
                {
                    context.Request.Headers.Add("Authorization", "Bearer " + token);
                    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                    context.Response.Headers.Add("X-Xss-Protection", "1");
                    context.Response.Headers.Add("X-Frame-Options", "DENY");
                }
                await next();
            });
            app.UseAuthentication();

            app.UseCors(x => x
                .WithOrigins("https://localhost:3000")
                .AllowCredentials()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Strict,
                HttpOnly = HttpOnlyPolicy.Always,
                Secure = CookieSecurePolicy.Always
            });
        }
    }
}
