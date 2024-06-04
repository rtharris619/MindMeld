using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Persistance
{
    public interface IUnitOfWork
    {
        IGenericRepository<Author> AuthorRepository { get; }
        IGenericRepository<Quote> QuoteRepository { get; }

        Task<int> Commit(CancellationToken cancellationToken);
    }
}
