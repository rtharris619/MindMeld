using Application;
using Application.DTO;
using Application.UseCases.Quotes.Command;
using Application.UseCases.Quotes.Query;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace API.Controllers
{
    [ApiController]
    [Route("api/quotes")]
    public class QuoteController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetQuotes(CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new QuoteListRequest(), cancellationToken);

            return response.IsSuccess ? Ok(response.Value) : BadRequest(response.Errors);
        }

        [HttpGet("{id}", Name = "GetQuoteById")]
        public async Task<IActionResult> GetQuoteById(Guid id, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new QuoteRequest { Id = id }, cancellationToken);

            return response.IsSuccess ? Ok(response.Value) : BadRequest(response.Errors);
        }

        [HttpPost]
        public async Task<IActionResult> CreateQuote(QuoteDTO quoteDTO, CancellationToken cancellationToken)
        {           
            var response = await _mediator.Send(new QuoteCreateCommand { QuoteDTO = quoteDTO }, cancellationToken);

            return response.IsSuccess ? Ok(response.Value) : BadRequest(response.Errors);
        }
    }
}
