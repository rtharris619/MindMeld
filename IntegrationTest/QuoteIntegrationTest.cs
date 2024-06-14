using Application.UseCases.SeedData.Command;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTest
{
    public class QuoteIntegrationTest
    {
        private readonly MindMeldContext _dbContext;
        //private readonly IMediator _mediator;
        private readonly IServiceScope _scope;
        protected readonly ISender Sender;

        public QuoteIntegrationTest()
        {
            var options = new DbContextOptionsBuilder<MindMeldContext>()
                .UseNpgsql("Host=database;Port=5432;Database=mindmeld_test;Username=postgres;Password=postgres;")
                .Options;

            _dbContext = new MindMeldContext(options);

            if (_dbContext.Database.GetPendingMigrations().Any())
            {
                _dbContext.Database.Migrate();
            }

            _scope = new ServiceCollection()
                //.AddMediatR(typeof())
                .AddDbContext<MindMeldContext>(opt => opt.UseNpgsql("Host=database;Port=5432;Database=mindmeld_test;Username=postgres;Password=postgres;"))
                .BuildServiceProvider()
                .CreateScope();

            Sender = _scope.ServiceProvider.GetRequiredService<ISender>();
        }

        [Fact]
        public async Task Seed_All_Quotes()
        {
            await Sender.Send(new SeedDataCommand { });
        }
    }
}
