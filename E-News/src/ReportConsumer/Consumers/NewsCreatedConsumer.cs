using System;
using System.Threading.Tasks;
using MassTransit;
using Models;
using ReportConsumer.Configuration;
using ReportConsumer.Handlers;

namespace ReportConsumer.Consumers
{
    public class NewsCreatedConsumer : IConsumer<CreateMessage>
    {
        public Task Consume(ConsumeContext<CreateMessage> context)
        {
            try
            {
                var handler =
                    (NewsCreatedEventHandler) ServiceManager.ServiceProvider.GetService(
                        typeof(IEventHandler<CreateMessage>));

                handler.Handle(context.Message);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}