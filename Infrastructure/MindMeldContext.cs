﻿using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class MindMeldContext : DbContext
    {
        public MindMeldContext(DbContextOptions options) : base(options)
        {         
            if (Database.GetPendingMigrations().Any())
                Database.Migrate();
        }

        public virtual DbSet<Quote> Quotes { get; set; }
        public virtual DbSet<Author> Authors { get; set; }

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

            //var deleted = ChangeTracker.Entries()
            //            .Where(t => t.State == EntityState.Deleted)
            //            .Select(t => t.Entity)
            //            .ToArray();

            //foreach (var entity in deleted)
            //{
            //    // AUDIT
            //}

            return await base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateBaseEntities(EntityState state)
        {
            var entities = ChangeTracker.Entries()
                        .Where(t => t.State == state)
                        .Select(t => t.Entity)
                        .ToArray();

            foreach (var entity in entities)
            {
                if (entity is Base)
                {
                    var track = entity as Base;
                    if (state == EntityState.Added)
                    {
                        track.CreatedDate = DateTime.Now; //DateTime.UtcNow;
                    }
                    track.ModifiedDate = DateTime.Now;
                }
            }
        }

        private void ApplyAuditing()
        {

        }
    }
}
