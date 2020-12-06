using Elastic.Apm.SerilogEnricher;
using Microsoft.AspNetCore.Hosting; 
using Microsoft.Extensions.Hosting; 
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Elasticsearch;

namespace ReportAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithElasticApmCorrelationInfo()  
                .WriteTo.Console(new ElasticsearchJsonFormatter() { })
                .CreateLogger();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { 
                    webBuilder.UseUrls("http://*:5101").UseStartup<Startup>();
                })
                .UseSerilog();
    }
}
