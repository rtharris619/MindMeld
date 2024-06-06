﻿using Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Quotes.Query;

public sealed class QuoteRequest : IQuery<QuoteDTO>
{
    public Guid Id { get; set; }
}
