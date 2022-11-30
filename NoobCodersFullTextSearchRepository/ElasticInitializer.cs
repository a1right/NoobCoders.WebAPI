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
            client.Indices.Delete(index);
            if (client.Indices.Exists(index).Exists)
            {
                return;
            }
            var posts = context.Posts.ToList();
            _ = client.Indices.Create(index, index => index.Map<Post>(p => p.AutoMap()));
            //foreach (var post in posts)
            //{
            //    client.Create(post, x => x.Index("posts"));
            //}
            BulkAll(client, posts, index);
        }

        private static void BulkAll(IElasticClient client, IEnumerable<Post> posts, string index)
        {
            var observableBulk = client.BulkAll<Post>(posts, p => p
                .MaxDegreeOfParallelism(8)
                .BackOffTime(TimeSpan.FromSeconds(10))
                .BackOffRetries(2)
                .Size(400)
                .RefreshOnCompleted()
                .Index(index)
            );
        }

    }
}
