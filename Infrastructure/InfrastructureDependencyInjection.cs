using Application.Persistance;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class InfrastructureDependencyInjection
    {
        public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Database");

            services.AddDbContext<MindMeldContext>(opt =>
                opt.UseNpgsql(connectionString));

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IQuoteRepository, QuoteRepository>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();

            return services;
        }

        public static void ApplyAutomaticMigrations(this IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<MindMeldContext>();

            if (db.Database.GetPendingMigrations().Any())
                db.Database.Migrate();
        }
    }
}
