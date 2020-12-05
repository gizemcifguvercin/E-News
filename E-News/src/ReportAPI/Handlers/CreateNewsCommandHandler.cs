using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Serilog;

namespace ReportAPI.Controllers
{
    public class CreateNewsCommandHandler : IRequestHandler<CreateNewsCommand, string>
    { 
        public Task<string> Handle(CreateNewsCommand request, CancellationToken cancellationToken)
        {
            Log.Information("Path: CreateNewsCommandHandler"); 
            Log.ForContext<CreateNewsCommandHandler>()
                .Information("Kuyruğa atılacak haber: Ajans {@agencyCode} , İçerik {@newsContent}", request.AgencyCode, request.NewsContent);

            throw new System.NotImplementedException();
        }
    }
}