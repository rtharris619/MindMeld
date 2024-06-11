using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class QuoteDTO : BaseDTO
    {        
        public required string Description { get; set; }
        public required AuthorDTO Author { get; set; }
    }
}
