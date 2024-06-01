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
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly MindMeldContext _context;
        private bool disposed = false;

        private GenericRepository<Author> authorRepository;
        private GenericRepository<Quote> quoteRepository;

        public UnitOfWork(MindMeldContext context)
        {
            _context = context;
            authorRepository = new GenericRepository<Author>(_context);
            quoteRepository = new GenericRepository<Quote>(_context);
        }

        public IGenericRepository<Author> AuthorRepository => authorRepository;
        public IGenericRepository<Quote> QuoteRepository => quoteRepository;

        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            disposed = true;
        }
    }
}
