using Application.DTO;
using Application.Persistance;
using Domain.Shared;
using MapsterMapper;

namespace Application.UseCases.Quotes.Query;

public sealed class QuoteListRequestHandler(IUnitOfWork unitOfWork, IMapper mapper) 
    : IQueryHandler<QuoteListRequest, List<QuoteDTO>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<List<QuoteDTO>>> Handle(QuoteListRequest request, CancellationToken cancellationToken)
    {
        var dbQuotes = await _unitOfWork.QuoteRepository.GetList(includeProperties: "Author");

        var quotes = _mapper.Map<IEnumerable<QuoteDTO>>(dbQuotes);

        return quotes.ToList();
    }
}
