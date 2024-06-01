using Application.DTO;
using Application.Mapper;
using Application.Persistance;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Quotes.Query
{
    public class QuoteListRequestHandler : IRequestHandler<QuoteListRequest, List<QuoteDTO>>
    {
        private readonly IQuoteRepository _quoteRepository;

        public QuoteListRequestHandler(IQuoteRepository quoteRepository)
        {
            _quoteRepository = quoteRepository;
        }

        public async Task<List<QuoteDTO>> Handle(QuoteListRequest request, CancellationToken cancellationToken)
        {
            var dbQuotes = _quoteRepository.Get(includeProperties: "Author");

            var quotes = dbQuotes.ToList().MapToQuoteDTOs();

            return quotes;
        }
    }
}
