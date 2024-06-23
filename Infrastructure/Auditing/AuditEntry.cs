using Domain.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class AuditEntry
    {
        public DateTime AuditDate { get; set; }
        public required string AuditBy { get; set; }
        public required string AuditAction { get; set; } // Eg. Modified, Added, Deleted from EF Core EntityState
        public Guid Key { get; set; }
        public string? Table { get; set; } // Eg. Quote, Author
        public Dictionary<string, object?> OldValues { get; set; } = [];
        public Dictionary<string, object?> NewValues { get; set; } = [];

        public List<PropertyEntry> TemporaryProperties { get; } = [];
        public bool HasTemporaryProperties => TemporaryProperties.Count != 0;

        public Audit ToAudit()
        {
            return new Audit()
            {
                AuditDate = DateTime.UtcNow,
                AuditBy = AuditBy,
                AuditAction = AuditAction,
                Key = Key,
                Table = Table,
                OldValues = OldValues.Count == 0 ? null : JsonSerializer.Serialize(OldValues),
                NewValues = NewValues.Count == 0 ? null : JsonSerializer.Serialize(NewValues)
            };
        }
    }
}
