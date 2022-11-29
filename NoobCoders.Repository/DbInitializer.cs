using Microsoft.Extensions.Configuration;
using NoobCoders.Application.Interfaces;

namespace NoobCoders.Repository
{
    public class DbInitializer
    {
        public static void Initialize(PostsDbContext context, IConfiguration configuration, ICsvReader reader)
        {
            context.Database.EnsureDeleted();
            var isNotExists = context.Database.EnsureCreated();
            if (isNotExists)
            {
                reader.SeedFromFile(context, configuration);
            }
        }
    }
}
