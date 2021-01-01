using System.Threading.Tasks;
using Models;
using ReportConsumer.Service.Contracts;

namespace ReportConsumer.Handlers
{
    public class NewsCreatedEventHandler : IEventHandler<CreateMessage>
    {
        private IIntegrationService _integrationService;

        public NewsCreatedEventHandler(IIntegrationService integrationService)
        {
            _integrationService = integrationService;
        }
 
        public Task Handle(CreateMessage message)
        { 
            _integrationService.SendNewsToAgency(message);
            return Task.CompletedTask;
        }
    }
}