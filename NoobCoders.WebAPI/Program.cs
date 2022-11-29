using Nest;
using NoobCoders.WebAPI.Data;
using NoobCoders.Application;
using NoobCoders.Application.Interfaces;
using NoobCoders.Application.Services;
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
            //builder.Services.AddCsvReader();
            builder.Services.AddSingleton<ICsvReader, CsvReaderService>();
            builder.Services.AddRecordsService();
            var app = builder.Build();

                var context = app.Services.CreateScope().ServiceProvider.GetRequiredService<PostsDbContext>() ;
                var csvReader = app.Services.GetService<ICsvReader>();
                DbInitializer.Initialize(context, app.Configuration, csvReader);
                ElasticInitializer.Initialize(app.Services.GetService<IElasticClient>(), context, app.Configuration);

            //System.Diagnostics.Process.Start(app.Configuration["ELKConfiguration:LocalExecutablePath"]);

            // Configure the HTTP request pipeline.
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