using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Configuration;
using Models;
using ReportAPI.Controllers; 
using Serilog;

namespace ReportAPI.DomainEventHandlers 
{
    public class CreateNewsDomainEventHandler : INotificationHandler<CreateNewsDomainEvent>
    {
        private readonly IBus _bus;
        private readonly IConfiguration _configuration;
        private ISendEndpoint _sendEndpoint;
        public CreateNewsDomainEventHandler(IBus bus, IConfiguration configuration)
        {
            _bus = bus ?? throw new ArgumentNullException(nameof(bus));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        
         private async Task SendMessageToBus(CreateMessage command)
        {
            Uri UriBuilder(string server, string vHost)
            {
                return new Uri($"{server}/{vHost}");
            }

            var uri = UriBuilder(_configuration.GetSection("RabbitMqSettings:Host").Value,
                "ENews.Events.V1.NewsCreated");

            var sendEndpoint = await _bus.GetSendEndpoint(uri);
            await sendEndpoint.Send(command);
        }

        public async Task Handle(CreateNewsDomainEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                var message = new CreateMessage(notification.News.AgencyCode,
                  notification.News.NewsContent , notification.News.CreatedOn, notification.News.IsActive);

                await SendMessageToBus(message);
 
            }
            catch(Exception e) { 
                Log.Error(e.Message);
                Log.ForContext<CreateNewsCommandHandler>()
                .Error(
                    "Haber Kuyruğa atılamadı : {@notification}", JsonSerializer.Serialize(notification));
 
            }
 
        }
    }
}