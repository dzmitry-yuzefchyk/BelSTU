using BusinessLogic.Hubs;
using BusinessLogic.Services.HostedServices;
using BusinessLogic.Services.Implementation;
using BusinessLogic.Services.Interfaces;
using CommonLogic.Configuration;
using CommonLogic.EmailSender;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
            services.AddHostedService<NotificationCleaner>();
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.AddDbContext(Configuration, "Default");
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<ISecurityService, SecurityService>();
            services.AddTransient<IProjectService, ProjectService>();
            services.AddTransient<IBoardService, BoardService>();
            services.AddTransient<ITaskService, TaskService>();
            services.AddTransient<IEmailSender, EmailSender>(x =>
                new EmailSender(
                    Configuration.GetValue<string>("EmailSender:HostName"),
                    Configuration.GetValue<string>("EmailSender:UserName"),
                    Configuration.GetValue<string>("EmailSender:Password"),
                    Configuration.GetValue<int>("EmailSender:Port"),
                    Configuration.GetValue<bool>("EmailSender:IsSSLEnabled")
                )
            );

            services.AddIdentity();
            services.AddAuthorization();
            services.AddCors();
            services.AddSignalR();
            services.AddControllersWithViews();
            services.AddSpaStaticFiles(configuration =>
             {
                 configuration.RootPath = "ClientApp/build";
             });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseAuthentication();
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
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<NotificationHub>("/hub/notification");
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

            app.UseCors(x => x
                .WithOrigins(Configuration.GetValue<string>("AppSettings:Host:Url"))
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
