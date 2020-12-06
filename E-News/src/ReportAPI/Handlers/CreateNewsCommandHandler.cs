using System.Threading;
using System.Threading.Tasks;  
using MediatR; 
using ReportAPI.Commands;
using ReportAPI.Services; 
 
namespace ReportAPI.Controllers
{
    public class CreateNewsCommandHandler : IRequestHandler<CreateNewsCommand,bool>
    {  
        protected INewsService _newsService;
        
        public CreateNewsCommandHandler( INewsService newsService)
        { 
            _newsService = newsService;
        }

        public async Task<bool> Handle(CreateNewsCommand request, CancellationToken cancellationToken)
        {  
            bool result =  await _newsService.Save(request); 
            return result;
        }
    }
}