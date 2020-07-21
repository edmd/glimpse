using glimpse.Models;
using glimpse.Models.Messaging;
using glimpse.Models.Repository;
using glimpse.Models.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;

namespace glimpse
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddDbContext<DataContext>(options =>
                options.UseInMemoryDatabase(databaseName: "GlimpseDatabase"));

            services.AddHttpClient<RequestResponseClient>("RequestResponseClient", client => {
                client.Timeout = TimeSpan.FromSeconds(10);
            });

            services.AddSingleton<IRequestResponseRepository, RequestResponseRepository>();
            services.AddSingleton<IHttpResponseEventRepository, HttpResponseEventRepository>();

            services.AddSingleton<IHttpClientInstance>(ctx => {
                var requestResponseClient = ctx.GetRequiredService<RequestResponseClient>().Client;
                var httpClientInstance = new HttpClientInstance(requestResponseClient);
                return httpClientInstance;
            });

            services.AddSingleton<IConnectionFactory>(ctx => {
                var connStr = this.Configuration["Rabbit"];
                var connFactory = new ConnectionFactory() {
                    Uri = new Uri(connStr),
                    DispatchConsumersAsync = true // This is mandatory to have Async Subscribers
                };
                return connFactory;
            });

            services.AddSingleton<IBusConnection>(ctx => {
                var connFactory = ctx.GetRequiredService<IConnectionFactory>();
                var persistentConnection = new RabbitPersistentConnection(connFactory);
                return persistentConnection;
            });

            services.AddSingleton<ISubscriber>(ctx => {
                var busConn = ctx.GetRequiredService<IBusConnection>();
                var subscriber = new RabbitSubscriber(busConn);
                return subscriber;
            });

            var channel = System.Threading.Channels.Channel.CreateBounded<RequestResponse>(100);
            services.AddSingleton(channel);

            services.AddSingleton<IProducer>(ctx =>
            {
                var channel = ctx.GetRequiredService<System.Threading.Channels.Channel<RequestResponse>>();
                var logger = ctx.GetRequiredService<ILogger<Producer>>();
                var instance = ctx.GetRequiredService<IHttpClientInstance>();
                var requestRepo = ctx.GetRequiredService<IRequestResponseRepository>();
                var eventRepo = ctx.GetRequiredService<IHttpResponseEventRepository>();
                var producer = new Producer(channel.Writer, logger, instance, requestRepo, eventRepo);
                return producer;
            });

            services.AddSingleton<IConsumer>(ctx =>
            {
                var channel = ctx.GetRequiredService<System.Threading.Channels.Channel<RequestResponse>>();
                var logger = ctx.GetRequiredService<ILogger<Consumer>>();
                var consumer = new Consumer(channel.Reader, logger, 1);
                return consumer;
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