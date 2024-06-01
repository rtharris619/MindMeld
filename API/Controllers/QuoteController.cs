using Application;
using Application.DTO;
using Application.UseCases.Quotes.Command;
using Application.UseCases.Quotes.Query;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuoteController : ControllerBase
    {
        private readonly IMediator _mediator;

        public QuoteController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(Name = "GetQuotes")]
        public async Task<IEnumerable<QuoteDTO>> GetQuotes()
        {
            var quotes = await _mediator.Send(new QuoteListRequest());

            return quotes;
        }

        [HttpPost(Name = "CreateQuote")]
        public async Task<IActionResult> CreateQuote(QuoteDTO quoteDTO)
        {
            var response = await _mediator.Send(new QuoteCreateRequest { QuoteDTO = quoteDTO });

            if (response.Success)
            {
                return Ok(response.Message);
            }

            return BadRequest(response.Message);
        }
    }
}
