using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.SeedData.Command
{
    public sealed class SeedDataCommand : ICommand<int>
    {
        public required string Username { get; set; }
    }
}
