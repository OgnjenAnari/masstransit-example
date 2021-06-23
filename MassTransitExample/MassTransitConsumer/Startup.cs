using MassTransit;
using MassTransitMessages;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MassTransitConsumer
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

            services.AddControllers();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<MassTransitTestConsumer>()
                    .Endpoint(e =>
                    {
                        e.Name = "masstransit-test-queue";
                        e.PrefetchCount = 20;
                    });

                x.UsingAmazonSqs((context, configurator) =>
                {
                    configurator.Host("eu-central-1", h => { });
                    configurator.PrefetchCount = 20;
                    configurator.WaitTimeSeconds = 10;
                    configurator.Message<MassTransitTestMessage>(m => m.SetEntityName("masstransit-test-topic"));

                    configurator.ConfigureEndpoints(context);
                });
            });

            services.AddMassTransitHostedService();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
