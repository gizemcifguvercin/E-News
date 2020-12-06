using MediatR;

namespace ReportAPI.Commands
{
    public class CreateNewsCommand : IRequest<bool>
    {  
         public string AgencyCode { get; set; }
         public string NewsContent { get; set; }
    }
}