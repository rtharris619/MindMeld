using Application.Persistance;
using Domain.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validation
{
    public class QuoteValidator : AbstractValidator<Quote>
    {
        private readonly IQuoteRepository _quoteRepository;

        public QuoteValidator(IQuoteRepository quoteRepository)
        {
            _quoteRepository = quoteRepository;

            ApplyRules();
        }

        private void ApplyRules()
        {
            RuleFor(x => x).NotNull().WithMessage("Quote is required");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required")
                .MaximumLength(500).WithMessage("Description must be less than 500 characters");

            RuleFor(x => x.Author).NotNull().WithMessage("Author is required");

            RuleFor(x => x.Author.Name).NotEmpty().WithMessage("Author is required");

            RuleFor(x => x).MustAsync(async (quote, token) =>
            {
                var result = await _quoteRepository.GetOne(x => x.Description == quote.Description);
                return result == null;
            }).WithMessage("Quote already exists");
        }
    }
}
