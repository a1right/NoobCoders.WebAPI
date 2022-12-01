using Microsoft.Extensions.Configuration;
using NoobCoders.Application.Interfaces;
using NoobCoders.Application.Models;
using NoobCoders.Domain;

namespace NoobCoders.Repository
{
    public class DbInitializer
    {
        public void Initialize(PostsDbContext context, IConfiguration configuration, ICsvReader reader)
        {
            context.Database.EnsureDeleted();
            var isExists = !context.Database.EnsureCreated();
            if (isExists)
                return;
            var records = reader.GetRecordTemplates(configuration);
            context.Rubrics.AddRange(ToRublicsHashSet(records));
            context.SaveChanges();
            var posts = new List<Post>();
            var rubrics = context.Rubrics.ToDictionary(r => r.Name);
            var postRubric = new List<PostRubric>();
            foreach (var record in records)
            {
                var post = new Post
                {
                    Text = record.Text,
                    CreatedDate = DateTime.Parse(record.CreatedDate),
                };
                posts.Add(post);
                postRubric.AddRange(record.Rubrics.Select(rubricName =>
                {
                    var rubric = rubrics[rubricName];
                    return new PostRubric { Post = post, RubricId = rubric.Id, Rubric = rubric };
                }).ToList());
            }

            context.Posts.AddRange(posts);
            context.PostRubrics.AddRange(postRubric);
            context.SaveChanges();
        }
        private HashSet<Rubric> ToRublicsHashSet(IEnumerable<RecordTemplate> records)
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
                if (ReferenceEquals(left, right)) return true;
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
