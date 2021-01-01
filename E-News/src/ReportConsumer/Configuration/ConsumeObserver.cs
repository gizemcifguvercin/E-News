using System;
using System.Threading.Tasks;
using MassTransit;
using System.Text.Json;

namespace ReportConsumer.Configuration
{
    public class ConsumeObserver : IConsumeObserver
    {
        
        public Task ConsumeFault<T>(ConsumeContext<T> context, Exception exception) where T : class
        {
            return Task.CompletedTask;
        }

        public Task PostConsume<T>(ConsumeContext<T> context) where T : class
        {
            return Task.CompletedTask;
        }

        public Task PreConsume<T>(ConsumeContext<T> context) where T : class
        {
            Console.WriteLine(JsonSerializer.Serialize(context.Message));
            return Task.CompletedTask;
        }
    }
}