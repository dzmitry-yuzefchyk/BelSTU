using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace CommonLogic.Configuration
{
    public static class ControllerBaseExtensions
    {
        public static void SetCookieSecurityToken(this ControllerBase controller, string token)
        {
            controller.HttpContext.Response.Cookies.Append(".AspNetCore.Application.Id", token,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    MaxAge = TimeSpan.FromMinutes(60)
                });
        }

        public static void ResetCookieSecurityToken(this ControllerBase controller)
        {
            controller.HttpContext.Response.Cookies.Delete(".AspNetCore.Application.Id");
        }

        public static Guid UserId(this ControllerBase controller)
        {
            return Guid.Parse(controller.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
    }
}
