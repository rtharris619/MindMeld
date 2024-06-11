using Application.DTO;
using Application.Persistance;
using Domain.Models;
using Domain.Shared;
using MapsterMapper;

namespace Application.UseCases.Quotes.Query
{
    public sealed class QuoteRequestHandler(IUnitOfWork unitOfWork, IMapper mapper) : IQueryHandler<QuoteRequest, QuoteDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<QuoteDTO>> Handle(QuoteRequest request, CancellationToken cancellationToken)
        {
            QuoteDTO? result = null;

            var dbQuote = await _unitOfWork.QuoteRepository.GetByID(request.Id, includeProperties: "Author");

            if (dbQuote != null)
            {
                result = _mapper.Map<QuoteDTO>(dbQuote);
            }

            return result;
        }
    }
}
