using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using PetDemo.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PetDemo.Repository;
using PetDemo.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PetDemo.Infrastructure;

namespace PetDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            //services.AddIdentity<ApplicationUser, IdentityRole>()
            //   .AddEntityFrameworkStores<ApplicationDbContext>()
            //   .AddDefaultTokenProviders();



            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseMySql(
                   Configuration.GetConnectionString("DefaultConnection")));

            services.AddTransient<IPetRepository, PetRepository>();
            services.AddTransient<IWatchListRepository, WatchListRepository>();
            services.AddTransient<INotificationRepository, NotificationRepository>();
            services.AddControllersWithViews();
            services.AddRazorPages();
            //clicent notification start
            services.AddMvc().AddNToastNotifyToastr();

            //services.AddToastNotification();

            //finished 

            services.AddMvc().AddNewtonsoftJson(options => {
                options.SerializerSettings.ReferenceLoopHandling =
                    Newtonsoft.Json.ReferenceLoopHandling.Ignore;   
            });

            services.AddSignalR();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            //clicent notification start

            app.UseNToastNotify();
            //finished 
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            //signalr
            //app.UseSignalR(route=>{
            //    route.MapHub<SignalServer>("signalServer");
            //});
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<SignalServer>("/signalServer");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Pet}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
