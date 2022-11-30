﻿using Nest;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoobCoders.Application.Interfaces;
using NoobCoders.Domain;
using NoobCoders.WebAPI.Common;
using NoobCoders.WebAPI.Models;

namespace NoobCoders.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecordsController : ControllerBase
    {
        private readonly IPostsDbContext _context;
        private readonly IElasticClient _elasticsearchClient;
        private readonly IRecordsService _recordsService;

        public RecordsController(IPostsDbContext context, IElasticClient elasticsearchClient, IRecordsService recordsService)
        {
            _context = context;
            _elasticsearchClient = elasticsearchClient;
            _recordsService = recordsService;
        }

        #region Posts
        [HttpGet]
        public async Task<ActionResult<PostResponse>> Posts()
        {
            var posts = await _recordsService.GetPosts();
            var result = posts.MapToResponse();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PostResponse>> PostDetails(long id)
        {
            var post = await _recordsService.GetPostDetais(id);
            var result = post.MapToResponse();
            return Ok(result);
        }
        [HttpDelete("/api/[controller]/{id}")]
        public async Task<ActionResult> Post(long id, CancellationToken cancellationToken)
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
        #endregion

        [HttpGet("/api/[controller]/text-search/{text}")]
        public async Task<ActionResult<List<PostResponse>>> Posts(string text)
        {
            var posts = await _recordsService.GetPostByText(text);
            var result = posts.MapToResponse();
            return Ok(result);
        }
    }
}