using Application.UseCases.SeedData.Command;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/seed")]
public class SeedController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly string Username = "System";

    [HttpPost]
    public async Task<IActionResult> SeedData(CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new SeedDataCommand { Username = Username }, cancellationToken);

        return response.IsSuccess ? Ok(response.Value) : BadRequest(response.Errors);
    }
}
