using API.Controllers;
using Application.DTO;
using Application.UseCases;
using Application.UseCases.Quotes.Query;
using Domain.Models;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest
{
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
    }
}
