using Application.Persistance;
using Domain.Models;
using Domain.Shared;

namespace Application.UseCases.SeedData.Command
{
    public sealed class SeedDataCommandHandler(IUnitOfWork unitOfWork) : ICommandHandler<SeedDataCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<int>> Handle(SeedDataCommand request, CancellationToken cancellationToken)
        {
            return await SeedQuotes(request.Username);
        }

        private async Task<int> SeedQuotes(string username)
        {
            var totalRecordsSaved = 0;

            var author = "Jim Rohn";
            var quote = "Either you run the day or the day runs you.";
            totalRecordsSaved += await SeedQuote(quote, author, username);

            author = "Helen Keller";
            quote = "Never bend your head. Always hold it high. Look the world straight in the face.";
            totalRecordsSaved += await SeedQuote(quote, author, username);

            author = "John Maxwell";
            quote = "As leaders, we must be focused on our present capabilities, not our past regrets.";
            totalRecordsSaved += await SeedQuote(quote, author, username);

            return totalRecordsSaved;
        }

        private async Task<int> SeedQuote(string quote, string author, string username)
        {
            var existingQuote = await CheckForExistingQuote(quote);

            if (existingQuote)
            {
                return 0;
            }

            var authorToSave = await AddAuthor(author, username);
            await AddQuote(quote, authorToSave, username);

            return await _unitOfWork.SaveChanges();
        }

        private async Task<bool> CheckForExistingQuote(string quote)
        {
            var result = await _unitOfWork.QuoteRepository.GetOne(x => x.Description == quote);

            return result != null;
        }

        private async Task<Author> AddAuthor(string author, string username)
        {
            var authorToSave = await _unitOfWork.AuthorRepository.GetOne(filter: x => x.Name == author);

            if (authorToSave == null)
            {
                authorToSave = new Author
                {
                    Name = author,
                    CreatedBy = username,
                    ModifiedBy = username
                };

                await _unitOfWork.AuthorRepository.Add(authorToSave);
            }

            return authorToSave;
        }

        private async Task AddQuote(string quote, Author author, string username)
        {
            var quoteToSave = new Quote
            {
                Author = author,
                Description = quote,
                CreatedBy = username,
                ModifiedBy = username
            };

            await _unitOfWork.QuoteRepository.Add(quoteToSave);
        }
    }
}
