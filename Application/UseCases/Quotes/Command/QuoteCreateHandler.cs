using Application.Persistance;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Quotes.Command
{
    public class QuoteCreateHandler : IRequestHandler<QuoteCreateRequest, CommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public QuoteCreateHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CommandResponse> Handle(QuoteCreateRequest request, CancellationToken cancellationToken)
        {
            var author = _unitOfWork.AuthorRepository.Get(filter: x => x.Name == request.QuoteDTO.Author).FirstOrDefault();

            if (author == null)
            {
                author = new Author
                {
                    Name = request.QuoteDTO.Author
                };

                _unitOfWork.AuthorRepository.Create(author);
            }

            var quote = new Quote
            {
                Author = author,
                Description = request.QuoteDTO.Description
            };

            _unitOfWork.QuoteRepository.Create(quote);

            _unitOfWork.Commit();

            return new CommandResponse
            {
                Success = true,
                Message = "Quote created successfully"
            };
        }
    }
}
