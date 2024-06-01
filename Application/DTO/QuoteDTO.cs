using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class QuoteDTO
    {
        public Guid Id { get; set; }
        public required string Description { get; set; }
        public required string Author { get; set; }
    }
}
