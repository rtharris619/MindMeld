using Application.DTO;
using Application.Mapper;
using Application.Persistance;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Quotes.Query
{
    public sealed class QuoteRequestHandler : IQueryHandler<QuoteRequest, QuoteDTO>
    {
        private readonly IQuoteRepository _quoteRepository;

        public QuoteRequestHandler(IQuoteRepository quoteRepository)
        {
            _quoteRepository = quoteRepository;
        }

        public async Task<Result<QuoteDTO>> Handle(QuoteRequest request, CancellationToken cancellationToken)
        {
            var dbQuote = await _quoteRepository.GetByID(request.Id, includeProperties: "Author");
            var quote = dbQuote.MapToQuoteDTO();

            return quote;
        }
    }
}
