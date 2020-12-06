using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Configuration; 
using Serilog;
 
namespace ReportAPI.Controllers
{
    public class CreateNewsCommandHandler : IRequestHandler<CreateNewsCommand,bool>
    { 
        private readonly IBus _bus;
        private readonly IConfiguration _configuration;
        private ISendEndpoint _sendEndpoint;
        
        public CreateNewsCommandHandler(IBus bus, IConfiguration configuration)
        {
            _bus = bus ?? throw new ArgumentNullException(nameof(bus));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _sendEndpoint =  _bus.GetSendEndpoint(new Uri($"{_configuration.GetSection("RabbitMqSettings:server").Value}/News")).GetAwaiter().GetResult();
        }
        public async Task<bool> Handle(CreateNewsCommand request, CancellationToken cancellationToken)
        { 
            Log.ForContext<CreateNewsCommandHandler>()
                .Information("CreateNewsCommandRequest Model : {@request}", JsonSerializer.Serialize(request));
    
            try
            { 
                await _sendEndpoint.Send(request);
            } 
            catch {
                Log.ForContext<CreateNewsCommandHandler>()
                .Error("Haber Kuyruğa atılamadı : {@request}", JsonSerializer.Serialize(request));
            }
            
            return true;
        }
    }
}