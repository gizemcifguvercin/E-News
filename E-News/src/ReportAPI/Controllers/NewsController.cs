using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc; 

namespace ReportAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NewsController : ControllerBase
    { 
        private readonly IMediator _mediator;

        public NewsController(IMediator mediator)
        {  
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
 
        [HttpPost("create")]
        public async Task<IActionResult> CreateNews([FromBody] CreateNewsCommand command) 
        {           
            bool result = await _mediator.Send(command);

            if(result == false)
                return BadRequest();

            return Ok(result);
        }
    }
}
