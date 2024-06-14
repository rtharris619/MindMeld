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
            return await SeedQuotes();
        }

        private async Task<int> SeedQuotes()
        {
            var totalRecordsSaved = 0;

            var author = "Jim Rohn";
            var quote = "Either you run the day or the day runs you.";
            totalRecordsSaved += await SeedQuote(quote, author);

            author = "Helen Keller";
            quote = "Never bend your head. Always hold it high. Look the world straight in the face.";
            totalRecordsSaved += await SeedQuote(quote, author);

            author = "John Maxwell";
            quote = "As leaders, we must be focused on our present capabilities, not our past regrets.";
            totalRecordsSaved += await SeedQuote(quote, author);

            return totalRecordsSaved;
        }

        private async Task<int> SeedQuote(string quote, string author)
        {
            var existingQuote = await CheckForExistingQuote(quote);

            if (existingQuote)
            {
                return 0;
            }

            var authorToSave = await AddAuthor(author);
            await AddQuote(quote, authorToSave);

            return await _unitOfWork.SaveChanges();
        }

        private async Task<bool> CheckForExistingQuote(string quote)
        {
            var result = await _unitOfWork.QuoteRepository.GetOne(x => x.Description == quote);

            return result != null;
        }

        private async Task<Author> AddAuthor(string author)
        {
            var authorToSave = await _unitOfWork.AuthorRepository.GetOne(filter: x => x.Name == author);

            if (authorToSave == null)
            {
                authorToSave = new Author
                {
                    Name = author
                };

                await _unitOfWork.AuthorRepository.Add(authorToSave);
            }

            return authorToSave;
        }

        private async Task AddQuote(string quote, Author author)
        {
            var quoteToSave = new Quote
            {
                Author = author,
                Description = quote
            };

            await _unitOfWork.QuoteRepository.Add(quoteToSave);
        }
    }
}
