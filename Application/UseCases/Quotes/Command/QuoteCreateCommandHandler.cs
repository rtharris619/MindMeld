using Application.Persistance;
using Domain.Models;
using Domain.Shared;
using FluentValidation;
using MapsterMapper;

namespace Application.UseCases.Quotes.Command
{
    public sealed class QuoteCreateCommandHandler(IUnitOfWork unitOfWork, IValidator<Quote> validator, IMapper mapper) 
        : ICommandHandler<QuoteCreateCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IValidator<Quote> _validator = validator;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<int>> Handle(QuoteCreateCommand request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Result.Failure<int>("Request was cancelled");
            }

            var quote = _mapper.Map<Quote>(request.QuoteDTO);

            var validationResult = await Validate(quote);
            if (validationResult != null)
            {
                return Result.Failure<int>(validationResult);
            }

            var author = await AddAuthor(request, quote);

            await AddQuote(quote, author);

            var rowsAffected = await _unitOfWork.SaveChanges(cancellationToken);

            return rowsAffected;
        }

        private async Task AddQuote(Quote quote, Author author)
        {
            var quoteToSave = new Quote
            {
                Author = author,
                Description = quote.Description
            };

            await _unitOfWork.QuoteRepository.Add(quoteToSave);
        }

        private async Task<Author> AddAuthor(QuoteCreateCommand request, Quote quote)
        {
            var author = await _unitOfWork.AuthorRepository.GetOne(filter: x => x.Name == quote.Author.Name);

            if (author == null)
            {
                author = new Author
                {
                    Name = request.QuoteDTO.Author.Name
                };

                await _unitOfWork.AuthorRepository.Add(author);
            }

            return author;
        }

        private async Task<List<string>?> Validate(Quote quote)
        {
            var validationResult = await _validator.ValidateAsync(quote);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                return errors;
            }

            return null;
        }
    }
}
