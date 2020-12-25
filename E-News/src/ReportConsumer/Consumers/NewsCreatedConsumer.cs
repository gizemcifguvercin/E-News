using System.Threading.Tasks;
using MassTransit;
using Models;
using ReportConsumer.Handlers;

namespace  ReportConsumer.Consumers
{ 
    public class NewsCreatedConsumer : IConsumer<News>
    {
        private readonly IEventHandler<News> _eventHandler;

        public NewsCreatedConsumer(IEventHandler<News> eventHandler)
        {
            _eventHandler = eventHandler;
        }

        public async Task Consume(ConsumeContext<News> context)
        {
            await _eventHandler.Handle(context.Message);
        }
    }
}
