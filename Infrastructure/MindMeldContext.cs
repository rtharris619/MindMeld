using Domain.Models;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class MindMeldContext : DbContext
    {
        public MindMeldContext(DbContextOptions options) : base(options)
        {
            
        }

        public virtual DbSet<Quote> Quotes { get; set; }
        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Audit> Audits { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Quote>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CreatedDate).HasColumnType("timestamp(6)");
                entity.Property(e => e.ModifiedDate).HasColumnType("timestamp(6)");
            });

            modelBuilder.Entity<Author>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CreatedDate).HasColumnType("timestamp(6)");
                entity.Property(e => e.ModifiedDate).HasColumnType("timestamp(6)");
            });
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            ChangeTracker.DetectChanges();
            
            UpdateBaseEntities(EntityState.Added);

            UpdateBaseEntities(EntityState.Modified);

            var auditing = new Auditing(this);
            var auditEntries = auditing.SaveAuditsBeforeSaveChanges();

            var result = await base.SaveChangesAsync(cancellationToken);

            auditing.SaveAuditsAfterSaveChanges(auditEntries);

            result += await base.SaveChangesAsync(cancellationToken);

            return result;
        }

        private void UpdateBaseEntities(EntityState state)
        {
            var entities = GetChangeTrackerEntities(state);

            foreach (var entity in entities)
            {
                if (entity is Base)
                {
                    var track = entity as Base;
                    if (state == EntityState.Added)
                    {
                        track.CreatedDate = DateTime.Now;
                    }
                    track.ModifiedDate = DateTime.Now;
                }
            }
        }

        private object[] GetChangeTrackerEntities(EntityState state)
        {
            return ChangeTracker.Entries()
                        .Where(t => t.State == state)
                        .Select(t => t.Entity)
                        .ToArray();
        }
    }
}
