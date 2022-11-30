using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NoobCoders.Application.Interfaces;
using NoobCoders.Application.Models;
using NoobCoders.Domain;

namespace NoobCoders.Application.Services
{
    public class CsvReaderService : ICsvReader
    {
        public void SeedFromFile(IPostsDbContext context, IConfiguration configuration)
        {
            var config = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                Delimiter = ",",
            };
            using (var reader = new StreamReader(configuration["PathToInitialDataFile"]))
            using (var csv = new CsvReader(reader, config))
            {
                var records = csv.GetRecords<RecordDto>().ToList();
                context.Rubrics.AddRangeAsync(ParseRubrics(records));
                context.SaveChangesAsync(CancellationToken.None);
                var posts = new List<Post>();
                var rubrics = context.Rubrics.ToList();
                var postRubric = new List<PostRubric>();
                foreach (var record in records)
                {
                    var post = new Post
                    {
                        Text = record.Text,
                        CreatedDate = DateTime.Parse(record.CreatedDate),
                    };
                    posts.Add(post);
                    postRubric.AddRange(record.Rubrics.Select(cpr =>
                    {
                        var rubric = rubrics.FirstOrDefault(x => x.Name == cpr);
                        return new PostRubric { Post = post, RubricId = rubric.Id, Rubric = rubric };
                    }).ToList());
                }

                context.Posts.AddRange(posts);
                context.PostRubrics.AddRange(postRubric);
                context.SaveChangesAsync(CancellationToken.None);
            }
        }

        private HashSet<Rubric> ParseRubrics(IEnumerable<RecordDto> records)
        {
            var result = new HashSet<Rubric>(new MyRubricComparer());
            foreach (var record in records)
            {
                foreach (var rubric in record.Rubrics)
                {
                    result.Add(new Rubric { Name = rubric });
                }
            }
            return result;
        }

        private class MyRubricComparer : IEqualityComparer<Rubric>
        {
            public bool Equals(Rubric left, Rubric right)
            {
                if(ReferenceEquals(left, right)) return true;
                if (left.Name == right.Name) return true;
                return false;
            }

            public int GetHashCode(Rubric obj)
            {
                return obj.Name.GetHashCode();
            }
        }
    }
}
