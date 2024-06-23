using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class Auditing
    {
        private readonly DbContext _context;
        private const string AUDIT_BY = "System";

        public Auditing(DbContext context)
        {
            _context = context;
        }

        public List<AuditEntry> SaveAuditsBeforeSaveChanges()
        {
            var auditEntries = new List<AuditEntry>();

            var entries = _context.ChangeTracker.Entries();

            foreach (var entry in entries)
            {
                if (entry.Entity is Audit || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;

                var auditEntry = new AuditEntry 
                {
                    AuditBy = AUDIT_BY,
                    AuditAction = entry.State.ToString(),
                    Table = entry.Metadata.GetTableName(),
                };
                auditEntries.Add(auditEntry);

                foreach (var property in entry.Properties)
                {
                    if (property.IsTemporary)
                    {
                        auditEntry.TemporaryProperties.Add(property);
                        continue;
                    }

                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.Key = (Guid)property.CurrentValue;
                        continue;
                    }

                    string propertyName = property.Metadata.Name;

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            break;

                        case EntityState.Deleted:
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            break;

                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                auditEntry.OldValues[propertyName] = property.OriginalValue;
                                auditEntry.NewValues[propertyName] = property.CurrentValue;
                            }
                            break;
                    }
                }
            }

            AddAuditEntriesToContext(auditEntries);

            var temporaryAuditEntries = auditEntries.Where(a => a.HasTemporaryProperties).ToList();

            return temporaryAuditEntries;
        }

        private void AddAuditEntriesToContext(List<AuditEntry> auditEntries)
        {
            auditEntries = auditEntries.Where(a => !a.HasTemporaryProperties).ToList();

            foreach (var auditEntry in auditEntries)
            {
                _context.Add(auditEntry.ToAudit());
            }
        }

        public void SaveAuditsAfterSaveChanges(List<AuditEntry> auditEntries)
        {
            if (auditEntries == null || auditEntries.Count == 0)
                return;

            foreach (var auditEntry in auditEntries)
            {
                foreach (var property in auditEntry.TemporaryProperties)
                {
                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.Key = (Guid)property.CurrentValue;
                    }
                    else
                    {
                        auditEntry.NewValues[property.Metadata.Name] = property.CurrentValue;
                    }
                }

                _context.Add(auditEntry.ToAudit());
            }
        }
    }
}
