using System.Threading.Tasks;
using MassTransit;
using Models;
using ReportConsumer.Handlers;

namespace  ReportConsumer.Consumers
{ 
    public class NewsCreatedConsumer : IConsumer<CreateMessage>
    {
        private readonly IEventHandler<CreateMessage> _eventHandler;

        public NewsCreatedConsumer(IEventHandler<CreateMessage> eventHandler)
        {
            _eventHandler = eventHandler;
        }

        public async Task Consume(ConsumeContext<CreateMessage> context)
        {
            await _eventHandler.Handle(context.Message);
        }
    }
}
