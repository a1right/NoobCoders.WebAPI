using Microsoft.AspNetCore.Mvc;
using NoobCoders.Application.Interfaces;
using NoobCoders.WebAPI.Common;
using NoobCoders.WebAPI.Models;

namespace NoobCoders.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecordsController : ControllerBase
    {
        private readonly IRecordsService _recordsService;

        public RecordsController(IRecordsService recordsService) => _recordsService = recordsService;

        #region Posts
        [HttpGet("/api/[controller]/posts")]
        public async Task<ActionResult<PostResponse>> Posts()
        {
            var posts = await _recordsService.GetPosts();
            var result = posts.MapToResponse();
            return Ok(result);
        }

        [HttpGet("/api/[controller]/posts/{id}")]
        public async Task<ActionResult<PostResponse>> PostDetails(long id)
        {
            var post = await _recordsService.GetPostDetais(id);
            var result = post.MapToResponse();
            return Ok(result);
        }

        [HttpPost("/api/[controller]/posts/{rubricName}/{text}")]
        public async Task<ActionResult<long>> PostCreate(string text, string rubricName)
        {
            var result = await _recordsService.CreatePost(text, rubricName);
            return Ok(result);
        }
        [HttpDelete("/api/[controller]/posts/{id}")]
        public async Task<ActionResult> PostDelete(long id)
        {
            await _recordsService.DeletePost(id);
            return Ok();
        } 
        #endregion

        [HttpGet("/api/[controller]/posts/contains/{text}")]
        public async Task<ActionResult<List<PostResponse>>> Posts(string text)
        {
            var posts = await _recordsService.GetPostsContainsSubstring(text);
            var result = posts.MapToResponse();
            return Ok(result);
        }

        #region Rubric
        [HttpGet("/api/[controller]/rubrics")]
        public async Task<ActionResult<PostResponse>> Rubrics()
        {
            var rubrics = await _recordsService.GetRubrics();
            var result = rubrics.MapToResponse();
            return Ok(result);
        }
        [HttpGet("/api/[controller]/rubrics/{id}")]
        public async Task<ActionResult<PostResponse>> RubricDetails(long id)
        {
            var rubric = await _recordsService.GetRubricDetails(id);
            var result = rubric.MapToResponse();
            return Ok(result);
        }
        [HttpDelete("/api/[controller]/rubrics/{id}")]
        public async Task<ActionResult> RubricDelete(long id)
        {
            await _recordsService.DeleteRubric(id);
            return Ok();
        }
        #endregion
    }
}
