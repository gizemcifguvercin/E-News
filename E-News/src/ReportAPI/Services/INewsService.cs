using System.Threading.Tasks;
using ReportAPI.Commands;

namespace ReportAPI.Services
{
    public interface INewsService
    { 
        Task<bool> Save(CreateNewsCommand request);
    }
}