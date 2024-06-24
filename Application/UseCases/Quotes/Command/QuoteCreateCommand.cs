using Application.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Quotes.Command;

public sealed class QuoteCreateCommand : ICommand<int>
{
    public QuoteDTO QuoteDTO { get; set; }
    public required string Username { get; set; }
}
