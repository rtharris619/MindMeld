using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Quote : Base
    {
        public required string Description { get; set; }
        public Author Author { get; set; } = null!;
    }
}
