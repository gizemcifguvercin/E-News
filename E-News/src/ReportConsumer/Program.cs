using System;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using MassTransit;
using MassTransit.AutofacIntegration;
using MassTransit.RabbitMqTransport.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http;
using Models;
using RabbitMQ.Client;
using ReportConsumer.Consumers;
using ReportConsumer.Handlers;

namespace ReportConsumer
{
    public class Program
    {
        private static IContainer _container;
        private static IConfigurationRoot _configurationRoot;
        private static IBusControl _busControl;

        public static void Main(string[] args)
        {
            var hostBuilder = new HostBuilder()
                .ConfigureHostConfiguration((config) => { config.AddEnvironmentVariables(prefix: "ASPNETCORE_"); })
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.SetBasePath(Environment.CurrentDirectory);
                    config.AddJsonFile("appsettings.json", optional: true);
                    config.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json",
                        optional: true);
                    Console.WriteLine(hostContext.HostingEnvironment.EnvironmentName);

                    config.AddEnvironmentVariables();

                    if (args != null)
                        config.AddCommandLine(args);

                    _configurationRoot = config.Build();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services = RegisterIoc(services);
                    _container = BuildContainer(RegisterServices, services);

                    _busControl = _container.Resolve<IBusControl>();
                    _busControl.StartAsync();
                }).Build();

            hostBuilder.Run();
        }

        static IServiceCollection RegisterIoc(IServiceCollection services)
        {
            services.AddHttpClient();
            
            services.Replace(ServiceDescriptor.Singleton<IHttpMessageHandlerBuilderFilter, CustomLoggingFilter>());
           
            services.Configure<RabbitMqSettings>(_configurationRoot.GetSection("RabbitMqSettings"));

            return services;
        }

        static IContainer BuildContainer(Action<ContainerBuilder> registrations = null,
            IServiceCollection services = null)
        {
            var builder = new ContainerBuilder();
            registrations?.Invoke(builder);
            builder.Populate(services);
            return builder.Build();
        }

        private static Action<ContainerBuilder> RegisterServices
        {
            get
            {
                return builder =>
                {
                    builder.RegisterType<NewsCreatedEventHandler>().As<IEventHandler<News>>()
                        .SingleInstance();

                    builder.RegisterAssemblyTypes(Assembly.Load("ReportConsumer"))
                        .InstancePerLifetimeScope();

                    builder.RegisterGeneric(typeof(AutofacConsumerFactory<>))
                        .WithParameter(new NamedParameter("name", "message"))
                        .As(typeof(IConsumerFactory<>))
                        .InstancePerLifetimeScope();
                    
                    builder.Register(context => Bus.Factory.CreateUsingRabbitMq(cfg =>
                        {
                
                            cfg.Host(new Uri(_configurationRoot.GetSection("RabbitMqSettings:host").Value),"/",
                                h =>
                                { 
                                    h.Username(_configurationRoot.GetSection("RabbitMqSettings:username").Value);
                                    h.Password(_configurationRoot.GetSection("RabbitMqSettings:password").Value);  
                                    h.UseCluster(x =>
                                    {
                                        x.ClusterMembers = _configurationRoot.GetSection("RabbitMqSettings:server")
                                            .Value.Split(",").ToArray();
                                    });
                                });


                            cfg.ReceiveEndpoint("ENews.Events.V1.NewsCreated",
                                x =>
                                {
                                    var contextResolved = context.Resolve<IComponentContext>();
                                    x.Consumer(() =>
                                        new NewsCreatedConsumer(contextResolved.Resolve<IEventHandler<News>>()
                                        ));
                                    x.BindMessageExchanges = false;
                                    x.Bind("ENews.Events.V1.News:Created", y =>
                                    {
                                        y.ExchangeType = ExchangeType.Topic;
                                        y.RoutingKey = "#.NWS";
                                    });
                                });
                        })).As<IBusControl>()
                        .As<IBus>()
                        .SingleInstance();
                };
            }
        }
    }
}