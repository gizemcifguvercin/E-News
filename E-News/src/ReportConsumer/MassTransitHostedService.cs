using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using ReportConsumer.Configuration.Messaging.Bus;

namespace ReportConsumer
{
    public class MassTransitHostedService : IHostedService
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await BusFactory.Bus.StartAsync(cancellationToken);
        }
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await BusFactory.Bus.StopAsync(cancellationToken);
        }
    }
}
