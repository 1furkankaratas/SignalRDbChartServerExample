using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRDbChartServerExample.Hubs;
using SignalRDbChartServerExample.Models;
using SignalRDbChartServerExample.Subscription;
using SignalRDbChartServerExample.Subscription.Middleware;

namespace SignalRDbChartServerExample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyMethod().AllowAnyHeader().AllowCredentials().SetIsOriginAllowed(origin => true)));
            services.AddSignalR();
            

            services.AddSingleton<DatabaseSubscription<Sale>>();
            services.AddSingleton<DatabaseSubscription<Person>>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseCors();

            app.UseDatabaseSubcripton<DatabaseSubscription<Sale>>("Sales");
            app.UseDatabaseSubcripton<DatabaseSubscription<Person>>("Persons");

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<SaleHub>("/salehub");
            });
        }
    }
}
