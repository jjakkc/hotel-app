using HotelAppLibrary.Data;
using HotelAppLibrary.Databases;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelApp.Web
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
            services.AddRazorPages();
            
            string dbChoice = Configuration.GetValue<string>("DatabaseChoice").ToLower();

            if (dbChoice == "sql")
                services.AddTransient<IDatabaseData, SqlData>();
            else if (dbChoice == "sqlite")
                services.AddTransient<IDatabaseData, SqliteData>();
            else
                services.AddTransient<IDatabaseData, SqlData>();

            // dependency injection wherever parameter asks for this type
            // addtransient will map an interface to a type and create a new instance of 
            // that type each time its requested
            services.AddTransient<ISQLDataAccess, SQLDataAccess> ();
            services.AddTransient<ISqliteDataAccess, SqliteDataAccess> ();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
