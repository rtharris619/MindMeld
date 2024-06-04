using Application.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Quotes.Command;

public sealed class QuoteCreateRequest : ICommand<int>
{
    public QuoteDTO QuoteDTO { get; set; }
}
