using DataProvider;
using DataProvider.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CommonLogic.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDbContext(this IServiceCollection services, IConfiguration configuration, string connectionString)
        {
            services.AddDbContext<TaskboardContext>(
                options => options.UseSqlServer(
                    configuration.GetConnectionString(connectionString)
                ));
        }

        public static void AddIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, Role>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                    options.Password.RequireDigit = false;
                    options.Password.RequiredUniqueChars = 0;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 6;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                    options.Lockout.MaxFailedAccessAttempts = 5;
                    options.SignIn.RequireConfirmedEmail = true;
                })
                .AddEntityFrameworkStores<TaskboardContext>()
                .AddDefaultTokenProviders();
        }
    }
}
