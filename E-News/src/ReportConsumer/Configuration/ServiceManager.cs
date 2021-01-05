using System;
using Infrastructure.Concretes;
using Infrastructure.Contracts;
using MassTransit;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Models;
using ReportConsumer.Configuration.Messaging.Bus;
using ReportConsumer.Handlers;
using ReportConsumer.Service.Concretes;
using ReportConsumer.Service.Contracts;
using Microsoft.Extensions.Configuration;

namespace ReportConsumer.Configuration
{
    public static class ServiceManager
    {
        public static IServiceProvider ServiceProvider { get; private set; }
        public static void Configure(this IServiceCollection services, IConfiguration _configuration)
        {
            services.AddSingleton<IHostedService, MassTransitHostedService>();
            services.AddLogging();
            services.AddSingleton<IBus>(_ => BusFactory.Bus);
            
            services.AddMemoryCache();
            services.AddSingleton<IIntegrationDefinationService, IntegrationDefinationService>();
            services.AddHttpClient();
            services.AddHttpClient<IIntegrationService, IntegrationService>();
            
            // INF
            services.AddSingleton<IIntegrationDefinationRepository, IntegrationDefinationRepository>();         
            services.AddSingleton<IMongoDbContext, MongoDbContext>();
            services.AddMediatR(typeof(ServiceManager));  
            services.Configure<DatabaseSettings>(_configuration.GetSection(nameof(DatabaseSettings)));
            var provider = services.BuildServiceProvider();
            var context = provider.GetService<IMongoDbContext>();
            context.Init();
            //INF 
            
            services.AddSingleton(typeof(IEventHandler<NewsCreated>), typeof(NewsCreatedEventHandler));
            ServiceProvider = services.BuildServiceProvider();
        }
    }
}