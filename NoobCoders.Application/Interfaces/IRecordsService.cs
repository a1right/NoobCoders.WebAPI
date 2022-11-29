using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoobCoders.Domain;

namespace NoobCoders.Application.Interfaces
{
    public interface IRecordsService
    {
        public Task<List<Post>> GetPosts();
    }
}
