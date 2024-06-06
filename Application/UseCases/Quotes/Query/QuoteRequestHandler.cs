using Application.DTO;
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
        public async Task<Result<QuoteDTO>> Handle(QuoteRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
