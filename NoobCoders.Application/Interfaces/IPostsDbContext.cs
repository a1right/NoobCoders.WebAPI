using NoobCoders.Domain;
using Microsoft.EntityFrameworkCore;

namespace NoobCoders.Application.Interfaces
{
    public interface IPostsDbContext
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Rubric> Rubrics { get; set; }
        public DbSet<PostRubric> PostRubrics { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
