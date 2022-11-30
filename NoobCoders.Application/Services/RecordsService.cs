using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Nest;
using NoobCoders.Application.Common.Exception;
using NoobCoders.Application.Interfaces;
using NoobCoders.Domain;

namespace NoobCoders.Application.Services
{
    public class RecordsService : IRecordsService
    {
        private readonly IPostsDbContext _context;
        private readonly IElasticClient _elasticClient;
        public RecordsService(IPostsDbContext context, IElasticClient elasticClient)
        {
            _context = context;
            _elasticClient = elasticClient;
        }

        public async Task<List<Post>> GetPosts(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _context.Posts.Include(p => p.PostRubrics)
                                       .ThenInclude(pr => pr.Rubric)
                                       .ToListAsync(cancellationToken);
        }

        public async Task<Post?> GetPostDetais(long id, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _context.Posts.Include(p => p.PostRubrics)
                                       .ThenInclude(pr => pr.Rubric)
                                       .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task RemovePost(long id, CancellationToken cancellationToken = default(CancellationToken))
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
            if (post is null)
            {
                throw new NotFoundException(nameof(Post), id);
            }
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync(cancellationToken);
            await _elasticClient.DeleteAsync<Post>(post.Id, null, cancellationToken);
        }

        public async Task<List<Post>> GetPostByText(string text, CancellationToken cancellationToken = default(CancellationToken))
        {
            var posts = await _elasticClient.SearchAsync<Post>(post =>
                post.Index("posts").Query(q =>
                        q.QueryString(qs =>
                            qs.Query('*' + text + '*')))
                                .Size(20), cancellationToken);
            var hitsId = posts.Hits.Select(hits => long.Parse(hits.Id)).ToList();
            var result = await _context.Posts.Include(p => p.PostRubrics)
                                             .ThenInclude(pr => pr.Rubric)
                                             .Where(p => hitsId.Contains(p.Id))
                                             .OrderBy(p => p.CreatedDate)
                                             .ToListAsync(cancellationToken);
            return result;
        }


    }
}
