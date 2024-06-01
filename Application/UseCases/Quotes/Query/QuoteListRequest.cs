using Application.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Quotes.Query
{
    public class QuoteListRequest : IRequest<List<QuoteDTO>>
    {

    }
}
