using CommonLogic.Logger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IO;

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

        public static void UseFileLogger(this IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            string applicationDirectory = Directory.GetCurrentDirectory();
            string logsDirectory = "logs";
            loggerFactory.AddFile(Path.Combine(applicationDirectory, logsDirectory));
        }
    }
}
