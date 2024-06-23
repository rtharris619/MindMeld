using Application.UseCases.Audits.Command;
using Application.UseCases.Quotes.Command;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/audits")]
    public class AuditController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpDelete]
        public async Task<IActionResult> DeleteAudits(CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new AuditListDeleteCommand(), cancellationToken);

            return response.IsSuccess ? Ok(response.Value) : BadRequest(response.Errors);
        }
    }
}
