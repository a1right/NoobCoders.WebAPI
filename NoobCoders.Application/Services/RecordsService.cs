using Microsoft.EntityFrameworkCore;
using Nest;
using NoobCoders.Application.Common.Exceptions;
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
                                       .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task DeletePost(long id, CancellationToken cancellationToken = default(CancellationToken))
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
        public async Task<long> CreatePost(string text, long rubricId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var post = new Post() { Text = text, CreatedDate = DateTime.Now };
            var rubric = await _context.Rubrics.FirstOrDefaultAsync(r => r.Id == rubricId, cancellationToken);
            var postRubric = new PostRubric()
            {
                Post = post,
                Rubric = rubric,
            };
            await _context.PostRubrics.AddAsync(postRubric, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            await _elasticClient.IndexAsync(post, request => request.Index<Post>(), cancellationToken);
            return post.Id;
        }

        public async Task UpdatePost(long id, string newText, CancellationToken cancellationToken = default(CancellationToken))
        {
            var postForUpdate = await _context.Posts.FirstOrDefaultAsync(p => p.Id == id);
            if (postForUpdate is null)
                throw new NotFoundException(nameof(Post), id);
            postForUpdate.Text = newText;
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<Post>> GetPostsContainsSubstring(string text, CancellationToken cancellationToken = default(CancellationToken))
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

        public async Task<List<Rubric>> GetRubrics(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _context.Rubrics.Include(r => r.RubricPosts)
                                         .ThenInclude(rp => rp.Post)
                                         .ToListAsync(cancellationToken);
        }
        public async Task<Rubric> GetRubricDetails(long id, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result =  await _context.Rubrics.Include(r => r.RubricPosts)
                                                       .ThenInclude(rp => rp.Post)
                                                       .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
            if (result is null)
                throw new NotFoundException(nameof(Rubric), id);
            return result;
        }

        public async Task<long> CreateRubric(string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            var rubricExists =
                await _context.Rubrics.FirstOrDefaultAsync(r => r.Name == name, cancellationToken) is not null;
            if (rubricExists)
                throw new RubricNameAlreadyExistsException(name);
            var rubric = new Rubric() { Name = name };
            await _context.Rubrics.AddAsync(rubric, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return rubric.Id;
        }

        public async Task UpdateRubric(long id, string newName, CancellationToken cancellationToken = default(CancellationToken))
        {
            var rubricForUpdate = await _context.Rubrics.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
            if (rubricForUpdate is null)
                throw new NotFoundException(nameof(Rubric), id);
            rubricForUpdate.Name = newName;
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteRubric(long id, CancellationToken cancellationToken = default(CancellationToken))
        {
            var rubric = await _context.Rubrics.Include(r => r.RubricPosts)
                                               .ThenInclude(rp => rp.Post)
                                               .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
            if (rubric is null)
            {
                throw new NotFoundException(nameof(Rubric), id);
            }
            var chainigEntities = await _context.PostRubrics.Where(pr => pr.RubricId == id)
                                                                           .ToListAsync(cancellationToken);
            _context.Rubrics.Remove(rubric);
            _context.PostRubrics.RemoveRange(chainigEntities);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
