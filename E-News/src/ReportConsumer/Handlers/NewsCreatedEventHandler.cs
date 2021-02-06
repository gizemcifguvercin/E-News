using System;
using System.Threading.Tasks;
using Models;
using ReportConsumer.Service.Contracts;

namespace ReportConsumer.Handlers
{
    public class NewsCreatedEventHandler : IEventHandler<NewsCreated>
    {
        private IIntegrationService _integrationService;

        public NewsCreatedEventHandler(IIntegrationService integrationService)
        {
            _integrationService = integrationService;
        }
 
        public async Task Handle(NewsCreated message)
        { 
            await _integrationService.SendNewsToAgency(message);
        }
    }
}