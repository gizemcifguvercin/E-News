using Microsoft.Extensions.Configuration;

namespace ReportConsumer.Configuration
{
    public static class ConfigurationManager
    {
        public static RabbitMqConfiguration RabbitMqDefinitions { get; } = new RabbitMqConfiguration();

        public static void BindConfigurations(this IConfiguration config)
        {
            config.Bind("RabbitMq", RabbitMqDefinitions);
        }
    }
}