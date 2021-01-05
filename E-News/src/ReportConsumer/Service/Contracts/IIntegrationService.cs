using System.Threading.Tasks;
using Models;

namespace ReportConsumer.Service.Contracts
{
    public interface IIntegrationService
    {
        Task SendNewsToAgency(NewsCreated message);
    }
}