using Microsoft.EntityFrameworkCore;
using NoobCoders.Domain;
using NoobCoders.Repository;
using Microsoft.EntityFrameworkCore.InMemory;

namespace NoobCoders.Tests.Common
{
    public class PostsContextFactory
    {
        public const string dbName = "Posts";
        public const long postIdForDelete = 1;
        public const long rubricIdForDelete = 1;

        public static PostsDbContext Create()
        {
            var options = new DbContextOptionsBuilder<PostsDbContext>().UseInMemoryDatabase(databaseName: dbName).Options;
            var context = new PostsDbContext(options);
            context.Database.EnsureCreated();
            var posts = Enumerable.Range(1, 10).Select(i => new Post
            {
                Id = i,
                CreatedDate = DateTime.Today,
                Text = "TestPostText " + i,
            }).ToList();
            var rubrics = Enumerable.Range(1, 5).Select(i => new Rubric()
            {
                Id = i,
                Name = "TestRubricName " + i,
            }).ToList();
            foreach (var post in posts)
            {
                foreach (var rubric in rubrics)
                {
                    context.PostRubrics.Add(new PostRubric()
                    {
                        Post = post,
                        PostId = post.Id,
                        Rubric = rubric,
                        RubricId = rubric.Id,
                    });
                }
            }

            context.SaveChanges();
            return context;
        }

        public static void Destroy(PostsDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
