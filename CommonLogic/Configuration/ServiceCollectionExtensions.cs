using DataProvider;
using DataProvider.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

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

        public static void AddServices(this IServiceCollection services)
        {

        }

        public static void AddIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<TaskboardContext>();
        }

        public static void AddJWT(this IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
           {
               options.RequireHttpsMetadata = true;
               options.SaveToken = true;
               options.TokenValidationParameters = new TokenValidationParameters
               {

               };
           });
        }
    }
}
