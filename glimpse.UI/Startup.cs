using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using glimpse.Models.Repository;
using Microsoft.AspNetCore.Mvc;
using glimpse.Models;
using RabbitMQ.Client;
using glimpse.Models.Messaging;
using Microsoft.Extensions.Logging;

namespace glimpse
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddRazorPages();
            services.AddServerSideBlazor();
            //services.AddSingleton<RequestService>();
            //services.AddSingleton<HeaderService>();

            services.AddDbContext<DataContext>(options =>
                options.UseInMemoryDatabase(databaseName: "GlimpseDatabase"));

            services.AddSingleton<IRequestResponseRepository, RequestResponseRepository>();
            services.AddSingleton<IConnectionFactory>(ctx =>
            {
                var connStr = this.Configuration["Rabbit"];
                return new ConnectionFactory()
                {
                    Uri = new Uri(connStr),
                    DispatchConsumersAsync = true // this is mandatory to have Async Subscribers
                };
            });
            services.AddSingleton<IBusConnection, RabbitPersistentConnection>();
            services.AddSingleton<ISubscriber, RabbitSubscriber>();

            var channel = System.Threading.Channels.Channel.CreateBounded<RequestResponse>(100);
            services.AddSingleton(channel);

            services.AddSingleton<IProducer>(ctx => {
                var channel = ctx.GetRequiredService<System.Threading.Channels.Channel<RequestResponse>>();
                var logger = ctx.GetRequiredService<ILogger<Producer>>();
                return new Producer(channel.Writer, logger);
            });

            services.AddSingleton<IConsumer>(ctx => { 
                var channel = ctx.GetRequiredService<System.Threading.Channels.Channel<RequestResponse>>();
                var logger = ctx.GetRequiredService<ILogger<Consumer>>();
                var repo = ctx.GetRequiredService<IRequestResponseRepository>();

                var consumer = new Consumer(channel.Reader, logger, 1, repo);

                return consumer;
            });

            services.AddSingleton<IHttpEventManager>(ctx => {
                var context = ctx.GetRequiredService<DataContext>();
                var publisher = ctx.GetRequiredService<RabbitPublisher>();
                return new HttpEventManager(context, publisher);
            });

            services.AddHostedService<BackgroundSubscriberWorker>();
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}