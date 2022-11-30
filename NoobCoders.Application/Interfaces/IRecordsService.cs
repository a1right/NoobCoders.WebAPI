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
        public Task<List<Post>> GetPosts(CancellationToken cancellationToken = default(CancellationToken));
        public Task<Post?> GetPostDetais(long id, CancellationToken cancellationToken = default(CancellationToken));
        public Task RemovePost(long id, CancellationToken cancellationToken = default(CancellationToken));
        public Task<List<Post>> GetPostByText(string text,CancellationToken cancellationToken = default(CancellationToken));
    }
}
