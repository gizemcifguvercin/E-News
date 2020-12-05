using MediatR;

namespace ReportAPI.Controllers
{
    public class CreateNewsCommand : IRequest<string>
    { 
         public string AgencyCode { get; set; }
         public string NewsContent { get; set; }

    }
}