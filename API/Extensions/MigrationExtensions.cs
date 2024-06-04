using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class MigrationExtensions
    {
        public static void ApplyMigration(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            using MindMeldContext context = scope.ServiceProvider.GetRequiredService<MindMeldContext>();            
           
            context.Database.Migrate();
        }
    }
}
