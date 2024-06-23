using Application.Persistance;
using Domain.Models;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class UnitOfWork(MindMeldContext context) : IUnitOfWork, IDisposable
    {
        private readonly MindMeldContext _context = context;
        private bool _disposed = false;

        private IAuditRepository? _auditRepository;
        private IAuthorRepository? _authorRepository;
        private IQuoteRepository? _quoteRepository;

        public IAuditRepository AuditRepository
        {
            get
            {
                _auditRepository ??= new AuditRepository(_context);
                return _auditRepository;
            }
        }

        public IAuthorRepository AuthorRepository
        {
            get
            {
                _authorRepository ??= new AuthorRepository(_context);
                return _authorRepository;
            }
        }

        public IQuoteRepository QuoteRepository
        {
            get
            {
                _quoteRepository ??= new QuoteRepository(_context);
                return _quoteRepository;
            }
        }        

        public async Task<int> SaveChanges(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }
    }
}
