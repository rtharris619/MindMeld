using Application.DTO;
using Application.Mapper;
using Application.Persistance;
using Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Quotes.Query;

public sealed class QuoteListRequestHandler : IQueryHandler<QuoteListRequest, List<QuoteDTO>>
{
    private readonly IQuoteRepository _quoteRepository;

    public QuoteListRequestHandler(IQuoteRepository quoteRepository)
    {
        _quoteRepository = quoteRepository;
    }

    public async Task<Result<List<QuoteDTO>>> Handle(QuoteListRequest request, CancellationToken cancellationToken)
    {
        var dbQuotes = await _quoteRepository.GetList(includeProperties: "Author");

        var quotes = dbQuotes.ToList().MapToQuoteDTOs();

        return quotes;
    }
}
