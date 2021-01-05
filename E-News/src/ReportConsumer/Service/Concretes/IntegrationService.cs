using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Models;
using ReportConsumer.Service.Contracts;
using System.Text.Json;

namespace ReportConsumer.Service.Concretes
{
    public class IntegrationService : BaseClient, IIntegrationService
    {
        private IIntegrationDefinationService _integrationDefinationService;

        public IntegrationService(HttpClient httpClient, IIntegrationDefinationService integrationDefinationService) : base(httpClient)
        {
            _integrationDefinationService = integrationDefinationService;
        }

        public async Task SendNewsToAgency(NewsCreated message)
        {
            var info = await _integrationDefinationService.GetIntegrationDetails(message.AgencyCode);
            
            if(info == null)
                throw new Exception($"Setting Bulunamadı: {JsonSerializer.Serialize(message)}");

            if (info.IsActive)
            {
                var msgContent = new StringContent(JsonSerializer.Serialize(message), Encoding.UTF8,
                    "application/json");

                await SendRequestToIntegration(msgContent, info);
            }
        }
    }
}