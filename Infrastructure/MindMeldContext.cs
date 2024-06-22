using Domain.Models;
using Microsoft.EntityFrameworkCore;
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
        public MindMeldContext()
        {
        }

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

            SaveAudit(EntityState.Added);

            SaveAudit(EntityState.Modified);

            SaveAudit(EntityState.Deleted);

            return await base.SaveChangesAsync(cancellationToken);
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

        private void SaveAudit(EntityState state)
        {
            var entries = ChangeTracker.Entries().Where(t => t.State == state);

            var temporaryProperties = entries.SelectMany(e => e.Properties).Where(p => p.IsTemporary).ToList();

            var newValues = new Dictionary<string, object?>();
            var oldValues = new Dictionary<string, object?>();

            var audits = new List<Audit>();

            foreach (var entry in entries)
            {
                if (entry.Entity is Audit || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;

                var audit = new Audit() { AuditAction = "", AuditBy = "", Table = "" };

                foreach (var property in entry.Properties)
                {
                    if (property.IsTemporary)
                    {
                        continue;
                    }

                    if (property.Metadata.IsPrimaryKey())
                    {
                        audit.Key = (Guid)property.CurrentValue;
                        continue;
                    }

                    string propertyName = property.Metadata.Name;

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            newValues[propertyName] = property.CurrentValue;
                            break;

                        case EntityState.Deleted:
                            oldValues[propertyName] = property.OriginalValue;
                            break;

                        case EntityState.Modified:
                            oldValues[propertyName] = property.OriginalValue;
                            newValues[propertyName] = property.CurrentValue;
                            break;
                    }
                }

                audit.AuditDate = DateTime.UtcNow;
                audit.AuditBy = "System";
                audit.AuditAction = entry.State.ToString();
                audit.Table = entry.Metadata.GetTableName();
                audit.NewValues = JsonSerializer.Serialize(newValues);
                audit.OldValues = JsonSerializer.Serialize(oldValues);
                
                audits.Add(audit);
            }

            Audits.AddRange(audits);
        }
    }
}
