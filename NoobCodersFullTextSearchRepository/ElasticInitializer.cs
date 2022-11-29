using Microsoft.Extensions.Configuration;
using Nest;
using NoobCoders.Application.Interfaces;

namespace NoobCodersFullTextSearchRepository
{
    public class ElasticInitializer
    {
        public static void Initialize(IElasticClient client, IPostsDbContext context, IConfiguration configuration)
        {
            //client.Indices.Delete(configuration["ELKConfiguration:Index"]);
            if (client.Indices.Exists(configuration["ELKConfiguration:Index"]).Exists)
            {
                return;
            }
            var posts = context.Posts.ToList();
            foreach (var post in posts)
            {
                client.Create(post, x => x.Index("posts"));
            }
        }

    }
}
