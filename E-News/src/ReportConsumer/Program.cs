
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ReportConsumer.Configuration;
using ReportConsumer.Service.Concretes;
using ReportConsumer.Service.Contracts;

namespace ReportConsumer
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var hostBuilder = new HostBuilder()
                .ConfigureHostConfiguration((config) => { config.AddEnvironmentVariables(prefix: "ASPNETCORE_"); })
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.SetBasePath(Environment.CurrentDirectory);
                    config.AddJsonFile("appsettings.json", optional: false);
                    config.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true);
                    config.AddEnvironmentVariables();

                    if (args != null)
                        config.AddCommandLine(args);

                    config.Build().BindConfigurations();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.Configure(hostContext.Configuration);
                })
                .ConfigureLogging((hostContext, logging) =>
                {
                    logging
                        .AddConfiguration(hostContext.Configuration.GetSection("Logging"))
                        .AddDebug()
                        .AddConsole();
                });

            await hostBuilder.RunConsoleAsync();
        }
    }
}