using System.Text.Json;
using System.Threading.Tasks;
using Infrastructure.Concretes;
using Models;
using ReportAPI.Commands;
using ReportAPI.Controllers; 
using Serilog;

namespace ReportAPI.Services
{
    public class NewsService : INewsService
    {
        protected INewsRepository _newsRepository;
        public NewsService(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public async Task<bool> Save(CreateNewsCommand request)
        {
            Log.ForContext<CreateNewsCommandHandler>()
                .Information("CreateNewsCommandRequest Model : {@request}", JsonSerializer.Serialize(request));

             News news = new News(request.AgencyCode, request.NewsContent, true);
             news.AddNewsDomainEvent();
             
             bool result =  await _newsRepository.Save(news);
             return result;
        }
    }
}