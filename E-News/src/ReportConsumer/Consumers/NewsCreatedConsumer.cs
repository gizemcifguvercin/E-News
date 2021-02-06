using System;
using System.Threading.Tasks;
using MassTransit;
using Models;
using ReportConsumer.Configuration;
using ReportConsumer.Handlers;

namespace ReportConsumer.Consumers
{
    public class NewsCreatedConsumer : IConsumer<NewsCreated>
    {
        public async Task Consume(ConsumeContext<NewsCreated> context)
        {
            try
            {
                var handler =
                    (NewsCreatedEventHandler) ServiceManager.ServiceProvider.GetService(
                        typeof(IEventHandler<NewsCreated>));

                await handler.Handle(context.Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}