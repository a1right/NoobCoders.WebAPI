using Microsoft.EntityFrameworkCore;
using NoobCoders.Application.Interfaces;
using NoobCoders.Domain;
using NoobCoders.Repository.EntityConfiguration;

namespace NoobCoders.Repository
{
        public class PostsDbContext : DbContext, IPostsDbContext
        {
            public DbSet<Post> Posts { get; set; }
            public DbSet<Rubric> Rubrics { get; set; }
            public DbSet<PostRubric> PostRubrics { get; set; }

            public PostsDbContext(DbContextOptions<PostsDbContext> options) : base(options) { }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.UseCollation("Cyrillic_General_100_CI_AS_KS_WS_SC");
                modelBuilder.ApplyConfiguration(new PostConfiguration())
                    .ApplyConfiguration(new RubricConfiguration())
                    .ApplyConfiguration(new PostRubricConfiguration());
                base.OnModelCreating(modelBuilder);

            }
        }
}
