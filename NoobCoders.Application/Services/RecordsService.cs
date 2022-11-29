using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Nest;
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

        public async Task<List<Post>> GetPosts()
        {
            return await _context.Posts.Include(p => p.PostRubrics)
                                       .ThenInclude(pr => pr.Rubric)
                                       .ToListAsync();
        }
    }
}
