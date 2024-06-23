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
        IAuditRepository AuditRepository { get; }
        IAuthorRepository AuthorRepository { get; }
        IQuoteRepository QuoteRepository { get; }

        Task<int> SaveChanges(CancellationToken cancellationToken = default);
    }
}
