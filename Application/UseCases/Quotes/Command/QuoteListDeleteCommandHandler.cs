using Application.Persistance;
using Domain.Models;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Quotes.Command
{
    public sealed class QuoteListDeleteCommandHandler(IUnitOfWork unitOfWork) : ICommandHandler<QuoteListDeleteCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<int>> Handle(QuoteListDeleteCommand request, CancellationToken cancellationToken)
        {
            await DeleteQuotes();
            await DeleteAuthors();

            var rowsAffected = await _unitOfWork.SaveChanges(cancellationToken);

            return rowsAffected;
        }

        private async Task DeleteQuotes()
        {
            var quotes = await _unitOfWork.QuoteRepository.GetList();
            foreach (var quote in quotes)
            {
                _unitOfWork.QuoteRepository.Remove(quote);
            }
        }

        private async Task DeleteAuthors()
        {
            var authors = await _unitOfWork.AuthorRepository.GetList();
            foreach (var author in authors)
            {
                _unitOfWork.AuthorRepository.Remove(author);
            }
        }
    }
}
