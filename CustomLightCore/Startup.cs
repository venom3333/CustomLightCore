using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using CustomLightCore.Models;

namespace CustomLightCore
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    public class Startup
    {
        //private IConfiguration config;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<CustomLightContext>();
            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<CustomLightContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();

            // Для сессий
            services.AddDistributedMemoryCache();
            services.AddMemoryCache();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddSession(options =>
                {
                    options.CookieName = ".CustomLight.Session";
                    options.IdleTimeout = TimeSpan.FromSeconds(3600);
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            // Сессии
            app.UseSession();

            //app.Run(async (context) =>
            //    {
            //        if (context.Session.Keys.Contains("Cart"))
            //        {
            //            //await context.Response.WriteAsync($"Hello {context.Session.GetString("name")}!");
            //        }
            //        else
            //        {
            //            context.Session.SetString("Cart", "");
            //            //await context.Response.WriteAsync("Hello World!");
            //        }
            //    });

            // Аутентификация
            app.UseIdentity();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Categories}/{action=Index}/{id?}");
            });
        }
    }
}
