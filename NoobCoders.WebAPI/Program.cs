using Nest;
using NoobCoders.Application;
using NoobCoders.Application.Interfaces;
using NoobCoders.Repository;
using NoobCodersFullTextSearchRepository;

namespace NoobCoders.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddPostsDbContext(builder.Configuration, "Posts");
            builder.Services.AddElasticSearch(builder.Configuration);
            builder.Services.AddCsvReader();
            builder.Services.AddRecordsService();
            var app = builder.Build();
            var csvReader = app.Services.GetService<ICsvReader>();
            var elasticClient = app.Services.GetService<IElasticClient>();
            using (var context = app.Services.CreateScope().ServiceProvider.GetService<PostsDbContext>())
            {
                var dbInitializer = new DbInitializer();
                dbInitializer.Initialize(context, app.Configuration, csvReader);
            }
            using (var context = app.Services.CreateScope().ServiceProvider.GetService<PostsDbContext>())
            {
                ElasticInitializer.Initialize(elasticClient, context, app.Configuration);
            }


            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}