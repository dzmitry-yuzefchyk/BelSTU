using BusinessLogic.Services.Implementation;
using BusinessLogic.Services.Interfaces;
using CommonLogic.Configuration;
using CommonLogic.EmailSender;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddJWT();
            services.AddDbContext(Configuration, "Default");
            services.AddIdentity();
            services.AddCors();

            services.AddTransient<IEmailSender, EmailSender>(x =>
                new EmailSender(
                    Configuration.GetValue<string>(Env, "EmailHostName", "EmailSender:HostName"),
                    Configuration.GetValue<string>(Env, "EmailUserName", "EmailSender:UserName"),
                    Configuration.GetValue<string>(Env, "EmailPassword", "EmailSender:Password"),
                    Configuration.GetValue<int>(Env, "EmailPort", "EmailSender:Port"),
                    Configuration.GetValue<bool>(Env, "EmailSslEnabled", "EmailSender:IsSSLEnabled")
                )
            );

            services.AddControllersWithViews();

            services.AddSpaStaticFiles(configuration =>
             {
                 configuration.RootPath = "ClientApp/build";
             });

            services.AddTransient<IAccountService, AccountService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseJWTAuth();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseFileLogger(loggerFactory);

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
