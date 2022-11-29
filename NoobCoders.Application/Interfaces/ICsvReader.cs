using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace NoobCoders.Application.Interfaces
{
    public interface ICsvReader
    {
        public void SeedFromFile(IPostsDbContext context, IConfiguration configuration);
    }
}
