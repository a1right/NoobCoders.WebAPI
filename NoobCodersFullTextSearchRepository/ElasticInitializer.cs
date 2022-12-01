using Microsoft.Extensions.Configuration;
using Nest;
using NoobCoders.Application.Interfaces;
using NoobCoders.Domain;

namespace NoobCodersFullTextSearchRepository
{
    public class ElasticInitializer
    {
        public static void Initialize(IElasticClient client, IPostsDbContext context, IConfiguration configuration)
        {
            var index = configuration["ELKConfiguration:Index"];
            if (client.Indices.Exists(index).Exists)
            {
                return;
            }
            var posts = context.Posts.ToList();
            CreateIndex(client, index);
            IndexAll(client, posts, index);
        }

        private static void IndexAll(IElasticClient client, IEnumerable<Post> posts, string index)
        {
            client.IndexMany(posts, index);
        }

        private static void CreateIndex(IElasticClient client, string index)
        {
            client.Indices.Create(index, index => index.Map<Post>(p => p.AutoMap()));
        }

    }
}
