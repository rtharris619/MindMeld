using Application.Persistance;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class AuditRepository : GenericRepository<Audit>, IAuditRepository
    {
        public AuditRepository(MindMeldContext context) : base(context)
        {
        }
    }
}
