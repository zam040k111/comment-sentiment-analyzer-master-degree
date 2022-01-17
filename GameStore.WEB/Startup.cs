using System;
using AutoMapper;
using GameStore.BLL.ServiceRegister;
using GameStore.BLL.Services;
using GameStore.WEB.Interfaces;
using GameStore.WEB.Middleware;
using GameStore.WEB.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GameStore.WEB
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostEnvironment env)
        {
            Configuration = configuration;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddSession(s => s.IdleTimeout = TimeSpan.FromMinutes(30));
            services.AddAutoMapper(
                typeof(Startup),
                typeof(GameService),
                typeof(CommentService),
                typeof(GenreService),
                typeof(PlatformTypeService));

            services.AddScoped<IFileService, FileService>();
            services.AddScoped<ICartService, CartService>();

            ServiceRegister.Register(services,
                Configuration.GetConnectionString("DefaultConnection"),
                Configuration.GetConnectionString("MongoConnection"));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/Home/HandleError");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.ConfigureCustomExceptionMiddleware();
            app.UseRouting();
            app.UseSession();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
