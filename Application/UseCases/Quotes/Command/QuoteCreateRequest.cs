using Application.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Quotes.Command
{
    public class QuoteCreateRequest : IRequest<CommandResponse>
    {
        public QuoteDTO QuoteDTO { get; set; }
    }
}
