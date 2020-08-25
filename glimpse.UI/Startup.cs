using glimpse.Data;
using glimpse.Entities;
using glimpse.Models;
using glimpse.Models.Queue;
using glimpse.Models.Repository;
using glimpse.Models.RequestResponses;
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
using System.Collections.Generic;

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
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
                       .EnableSensitiveDataLogging());
            services.AddTransient<DbContext, DataContext>();

            services.AddHttpClient<RequestResponseClient>("RequestResponseClient", client => {
                client.Timeout = TimeSpan.FromSeconds(10);
            });

            services.AddTransient<IRequestResponseRepository>(ctx => {
                var context = ctx.GetRequiredService<DataContext>();
                var requestResponseRepository = new RequestResponseRepository(context);
                return requestResponseRepository;
            });

            services.AddTransient<IHttpResponseEventRepository>(ctx => {
                var context = ctx.GetRequiredService<DataContext>();
                var httpResponseEventRepository = new HttpResponseEventRepository(context);
                return httpResponseEventRepository;
            });

            services.AddTransient<IHttpClientInstance>(ctx => {
                var requestResponseClient = ctx.GetRequiredService<RequestResponseClient>().Client;
                var httpClientInstance = new HttpClientInstance(requestResponseClient);
                return httpClientInstance;
            });

            services.AddTransient<IConnectionFactory>(ctx => {
                var connStr = this.Configuration["Rabbit"];
                var connFactory = new ConnectionFactory() {
                    Uri = new Uri(connStr),
                    DispatchConsumersAsync = true // This is mandatory to have Async Subscribers
                };
                return connFactory;
            });

            services.AddTransient<IBusConnection>(ctx => {
                var connFactory = ctx.GetRequiredService<IConnectionFactory>();
                var persistentConnection = new RabbitPersistentConnection(connFactory);
                return persistentConnection;
            });


            services.AddTransient<IHttpEventManager>(ctx => {
                var requestRepo = ctx.GetRequiredService<IRequestResponseRepository>();
                var connection = ctx.GetRequiredService<IBusConnection>();
                var httpEventManager = new HttpEventManager(requestRepo, connection);
                return httpEventManager;
            });

            services.AddTransient<ISubscriber>(ctx => {
                var busConn = ctx.GetRequiredService<IBusConnection>();
                var subscriber = new RabbitSubscriber(busConn);
                return subscriber;
            });

            var channel = System.Threading.Channels.Channel.CreateBounded<RequestResponse>(100);
            services.AddSingleton(channel);

            services.AddTransient<IProducer>(ctx =>
            {
                var channel = ctx.GetRequiredService<System.Threading.Channels.Channel<RequestResponse>>();
                var logger = ctx.GetRequiredService<ILogger<RequestResponseProducer>>();
                var instance = ctx.GetRequiredService<IHttpClientInstance>();
                var requestRepo = ctx.GetRequiredService<IRequestResponseRepository>();
                var eventRepo = ctx.GetRequiredService<IHttpResponseEventRepository>();
                var producer = new RequestResponseProducer(channel.Writer, logger, instance, eventRepo);
                return producer;
            });

            services.AddTransient<IConsumer>(ctx =>
            {
                var channel = ctx.GetRequiredService<System.Threading.Channels.Channel<RequestResponse>>();
                var logger = ctx.GetRequiredService<ILogger<RequestResponseConsumer>>();
                var consumer = new RequestResponseConsumer(channel.Reader, logger, 1);
                return consumer;
            });

            // DI'ing these as an IHostedService to automatically call the Start and Stop methods
            services.AddSingleton<IHostedService>(ctx =>
            {
                var logger = ctx.GetRequiredService<ILogger<BackgroundPublisherWorker>>();
                var httpEventManager = ctx.GetRequiredService<IHttpEventManager>();
                var backgroundPublisherWorker = new BackgroundPublisherWorker(httpEventManager, logger);
                return backgroundPublisherWorker;
            });

            services.AddSingleton<IHostedService>(ctx =>
            {
                var subscriber = ctx.GetRequiredService<ISubscriber>();
                var producer = ctx.GetRequiredService<IProducer>();
                var consumer = ctx.GetRequiredService<IConsumer>();
                var logger = ctx.GetRequiredService<ILogger<BackgroundSubscriberWorker>>();
                var backgroundSubscriberWorker = new BackgroundSubscriberWorker(
                    subscriber, producer, new List<IConsumer>() { consumer }, logger);
                return backgroundSubscriberWorker;
            });

            //services.AddHostedService<BackgroundSubscriberWorker>(ctx =>
            //{
            //    var subscriber = ctx.GetRequiredService<ISubscriber>();
            //    var producer = ctx.GetRequiredService<IProducer>();
            //    var consumer = ctx.GetRequiredService<IConsumer>();
            //    var logger = ctx.GetRequiredService<ILogger<BackgroundSubscriberWorker>>();
            //    var backgroundSubscriberWorker = new BackgroundSubscriberWorker(
            //        subscriber, producer, new List<IConsumer>() { consumer }, logger);
            //    return backgroundSubscriberWorker;
            //});
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