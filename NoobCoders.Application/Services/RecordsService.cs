﻿using Microsoft.EntityFrameworkCore;
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
                                       .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<long> CreatePost(string text, string rubricName, CancellationToken cancellationToken = default(CancellationToken))
        {
            var post = new Post() { Text = text, CreatedDate = DateTime.Now };
            var rubric = await _context.Rubrics.FirstOrDefaultAsync(r => r.Name == rubricName, cancellationToken);
            if (rubric is null)
            {
                rubric = new Rubric() { Name = rubricName };
            }
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
