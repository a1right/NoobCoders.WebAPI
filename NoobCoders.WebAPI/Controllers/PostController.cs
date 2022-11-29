using Nest;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoobCoders.Application.Interfaces;
using NoobCoders.Domain;
using NoobCoders.WebAPI.Models;

namespace NoobCoders.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostsDbContext _context;
        private readonly IElasticClient _elasticsearchClient;
        private readonly IRecordsService _recordsService;

        public PostController(IPostsDbContext context, IElasticClient elasticsearchClient, IRecordsService recordsService)
        {
            _context = context;
            _elasticsearchClient = elasticsearchClient;
            _recordsService = recordsService;
        }

        [HttpGet]
        public async Task<ActionResult<PostResponse>> Posts()
        {
            var posts = await _recordsService.GetPosts();
            var response = posts.Select(p => new PostResponse().MapFrom(p)).ToList();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PostResponse>> GetPost(long id)
        {
            var post = await _context.Posts.Include(p => p.PostRubrics)
                .ThenInclude(pr => pr.Rubric)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (post is null)
                return NotFound();
            var result = new PostResponse
            {
                Id = post.Id,
                CreatedDate = post.CreatedDate,
                Rubrics = post.PostRubrics.Select(x => x.Rubric).ToList(),
                Text = post.Text,
            };
            return Ok(result);
        }

        [HttpGet("/api/[controller]/text-search/{text}")]
        public async Task<ActionResult<List<PostResponse>>> GetPostByText(string text)
        {
            var posts = await _elasticsearchClient.SearchAsync<Post>(p => p.Index("posts")
                .Query(q => q.QueryString(qs => qs.Query('*' + text + '*')))
                .Size(20));
            var postsId = posts.Hits.Select(x => long.Parse(x.Id)).ToList();
            var result = await _context.Posts
                .Include(x => x.PostRubrics)
                .ThenInclude(x => x.Rubric)
                .Where(x => postsId.Contains(x.Id))
                .ToListAsync();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePost(long id, CancellationToken cancellationToken)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (post is null)
                return NotFound();
            var rubrics = await _context.PostRubrics.Where(x => x.PostId == id).ToListAsync(cancellationToken);
            _context.PostRubrics.RemoveRange(rubrics);
            _context.Posts.Remove(post);
            var response = await _elasticsearchClient.DeleteAsync<Post>(id.ToString());
            await _context.SaveChangesAsync(cancellationToken);
            return Ok();
        }
    }
}
