using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace ReportAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReportController : ControllerBase
    { 
        public ReportController()
        {  
        }

        [HttpGet("IsAvailable")]
        public bool IsReportAPIAvailable() 
        {
            var result = false;
            Log.Information("IsReportAPIAvailable: {result}", result); 
            return result;
        }

    }
}
