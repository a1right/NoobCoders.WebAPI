using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NoobCoders.Application.Interfaces;

namespace NoobCoders.Repository
{
    public static class DependencyInject
    {
        public static IServiceCollection AddPostsDbContext(this IServiceCollection services, IConfiguration configuration,
            string connectionStringName)
        {
            var connectionString = configuration.GetConnectionString(connectionStringName);
            services.AddDbContext<PostsDbContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped<IPostsDbContext>(provider => provider.GetService<PostsDbContext>());
            return services;
        }
    }
}
