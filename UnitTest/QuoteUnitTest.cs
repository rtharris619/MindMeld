using API.Controllers;
using Application.DTO;
using Application.Persistance;
using Application.UseCases;
using Application.UseCases.Quotes.Query;
using Domain.Models;
using Domain.Shared;
using Infrastructure;
using Infrastructure.Repositories;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.DependencyModel;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest;

public class QuoteUnitTest
{
    [Fact]
    public async Task Get_All_Quotes_Returns_Ok()
    {
        var td = new Mock<IMediator>();

        var expected = new List<QuoteDTO>();

        td.Setup(m => m.Send(It.IsAny<QuoteListRequest>(), default)).ReturnsAsync(expected);

        var sut = new QuoteController(td.Object);

        var result = await sut.GetQuotes(new CancellationToken());

        var ok = Assert.IsAssignableFrom<OkObjectResult>(result);

        Assert.Equal(expected, ok.Value);
    }

    [Fact]
    public async Task Get_Quote_Returns_Ok()
    {
        var td = new Mock<IMediator>();

        var expected = new QuoteDTO() { Description = "", Author = new AuthorDTO() { Name = "" } };

        td.Setup(m => m.Send(It.IsAny<QuoteRequest>(), default)).ReturnsAsync(expected);

        var sut = new QuoteController(td.Object);

        var result = await sut.GetQuoteById(Guid.NewGuid(), new CancellationToken());

        var ok = Assert.IsAssignableFrom<OkObjectResult>(result);

        Assert.Equal(expected, ok.Value);
    }

    [Fact]
    public async Task Get_Quote_List_Request_Handler()
    {
        var quoteDTOList = new List<Quote>
        {
            new Quote()
            {
                Description = "Quote 1",
                Author = new Author()
                {
                    Name = "Author 1"
                }
            },
            new Quote()
            {
                Description = "Quote 2",
                Author = new Author()
                {
                    Name = "Author 2"
                }
            }
        };

        var expected = new List<QuoteDTO>
        {
            new QuoteDTO()
            {
                Description = "Quote 1",
                Author = new AuthorDTO()
                {
                    Name = "Author 1"
                }
            },
            new QuoteDTO()
            {
                Description = "Quote 2",
                Author = new AuthorDTO()
                {
                    Name = "Author 2"
                }
            }
        };

        var unitOfWork = new Mock<IUnitOfWork>();
        var mapper = new Mock<IMapper>();

        unitOfWork.Setup(m => m.QuoteRepository.GetList(It.IsAny<Expression<Func<Quote, bool>>?>(), 
            It.IsAny<Func<IQueryable<Quote>, IOrderedQueryable<Quote>>?>(), It.IsAny<string>()))
            .ReturnsAsync(quoteDTOList);

        mapper.Setup(m => m.Map<IEnumerable<QuoteDTO>>(It.IsAny<IEnumerable<Quote>>())).Returns(expected);

        var handler = new QuoteListRequestHandler(unitOfWork.Object, mapper.Object);

        var actual = await handler.Handle(new QuoteListRequest(), default);

        Assert.Equal(2, actual.Value.Count);

        foreach (var item in actual.Value)
        {
            Assert.Contains(expected, x => x.Description == item.Description);
            Assert.Contains(Assert.Single(expected, x => x.Description == item.Description).Author.Name, item.Author.Name);
        }
    }

    [Fact]
    public async Task Get_All_Quotes()
    {
        var expected = new List<Quote>
        {
            new Quote()
            {
                Description = "Quote 1",
                Author = new Author()
                {
                    Name = "Author 1"
                }
            },
            new Quote()
            {
                Description = "Quote 2",
                Author = new Author()
                {
                    Name = "Author 2"
                }
            }
        }.AsQueryable();

        var mockSet = new Mock<DbSet<Quote>>();

        mockSet.As<IAsyncEnumerable<Quote>>()
            .Setup(d => d.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
            .Returns(new TestAsyncEnumerator<Quote>(expected.GetEnumerator()));

        mockSet.As<IQueryable<Quote>>()
            .Setup(m => m.Provider)
            .Returns(new TestAsyncQueryProvider<Quote>(expected.Provider));

        mockSet.As<IQueryable<Quote>>().Setup(m => m.Expression).Returns(expected.Expression);
        mockSet.As<IQueryable<Quote>>().Setup(m => m.ElementType).Returns(expected.ElementType);
        mockSet.As<IQueryable<Quote>>().Setup(m => m.GetEnumerator()).Returns(() => expected.GetEnumerator());

        var context = new Mock<MindMeldContext>();

        context.Setup(x => x.Quotes).Returns(mockSet.Object);

        var actual = await context.Object.Quotes.ToListAsync();

        Assert.Equal(2, actual.Count());
    }
}

internal class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
{
    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default) =>
        new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());


    public TestAsyncEnumerable(IEnumerable<T> enumerable)
            : base(enumerable)
    { }

    public TestAsyncEnumerable(Expression expression)
        : base(expression)
    { }
}

internal class TestAsyncEnumerator<T> : IAsyncEnumerator<T>, IAsyncDisposable, IDisposable
{
    private readonly IEnumerator<T> enumerator;

    private Utf8JsonWriter? _jsonWriter = new(new MemoryStream());

    public TestAsyncEnumerator(IEnumerator<T> enumerator)
    {
        this.enumerator = enumerator;
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore().ConfigureAwait(false);

        Dispose(disposing: false);
#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize

        GC.SuppressFinalize(this);
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _jsonWriter?.Dispose();
            _jsonWriter = null;
        }
    }

    protected virtual async ValueTask DisposeAsyncCore()
    {
        if (_jsonWriter is not null)
        {
            await _jsonWriter.DisposeAsync().ConfigureAwait(false);
        }

        _jsonWriter = null;
    }

    public T Current => enumerator.Current;

    public Task<bool> MoveNextAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(enumerator.MoveNext());
    }

    public ValueTask<bool> MoveNextAsync()
    {
        return new ValueTask<bool>(enumerator.MoveNext());
    }
}

internal class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
{
    private readonly IQueryProvider _inner;

    internal TestAsyncQueryProvider(IQueryProvider inner)
    {
        _inner = inner;
    }

    public IQueryable CreateQuery(Expression expression)
    {
        return new TestAsyncEnumerable<TEntity>(expression);
    }

    public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
    {
        return new TestAsyncEnumerable<TElement>(expression);
    }

    public object Execute(Expression expression)
    {
        return _inner.Execute(expression);
    }

    public TResult Execute<TResult>(Expression expression)
    {
        return _inner.Execute<TResult>(expression);
    }

    public Task<object> ExecuteAsync(Expression expression, CancellationToken cancellationToken)
    {
        return Task.FromResult(Execute(expression));
    }

    public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
    {
        return Task.FromResult(Execute<TResult>(expression));
    }

    TResult IAsyncQueryProvider.ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
    {
        return Task.FromResult(Execute<TResult>(expression)).Result;

        //return Execute<TResult>(expression);

        //throw new NotImplementedException();
    }
}
