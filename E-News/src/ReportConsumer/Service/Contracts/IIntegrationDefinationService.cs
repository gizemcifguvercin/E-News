using System.Threading.Tasks;
using Models;

namespace ReportConsumer.Service.Contracts
{
    public interface IIntegrationDefinationService
    {
        Task<AgencyInfo> GetIntegrationDetails(string agencyCode);
    }
}