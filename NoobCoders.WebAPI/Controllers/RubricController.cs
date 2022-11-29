using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoobCoders.Application.Interfaces;
using NoobCoders.WebAPI.Models;

namespace NoobCoders.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private IPostsDbContext _context;

        public TestController(IPostsDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<PostResponse>>> Posts()
        {
            var posts = await _context.Posts.Include(p => p.PostRubrics).ThenInclude(pr => pr.Rubric).ToListAsync();
            var result = posts.Select(p => new PostResponse(){Id = p.Id, Text = p.Text, CreatedDate = p.CreatedDate, Rubrics = p.PostRubrics.Select(x => x.Rubric).ToList()}).ToList();
            return Ok(result);
        }
    }
}
