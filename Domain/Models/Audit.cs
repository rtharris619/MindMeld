using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public record Audit
    {
        public Guid Id { get; set; }
        public DateTime AuditDate { get; set; }
        public required string AuditBy { get; set; }
        public required string AuditAction { get; set; } // Eg. Modified, Added, Deleted from EF Core EntityState
        public Guid Key { get; set; }
        public required string Table { get; set; } // Eg. Quote, Author
        public string? OldValues { get; set; } // JSON string
        public string? NewValues { get; set; } // JSON string
    }
}
