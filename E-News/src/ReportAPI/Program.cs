using Microsoft.AspNetCore.Hosting; 
using Microsoft.Extensions.Hosting; 
using Serilog;

namespace ReportAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
              Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
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
