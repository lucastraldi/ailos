using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Queries.Requests;

namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {

        [HttpPost("movement")]
        public IActionResult AccountMovementAsync(
            [FromServices]IMediator mediator,
            [FromBody] AccountMovementRequest command)
        {
            try
            {
                return Ok(mediator.Send(command));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("balance")]
        public IActionResult AccountBalance(
            [FromServices] IMediator mediator,
            [FromBody] AccountBalanceRequest command)
        {
            try
            {
                return Ok(mediator.Send(command));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}