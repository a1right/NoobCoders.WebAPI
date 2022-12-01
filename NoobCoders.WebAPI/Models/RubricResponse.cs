using NoobCoders.Domain;

namespace NoobCoders.WebAPI.Models
{
    public class RubricResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<PostDto> RubricPosts { get; set; }

        public RubricResponse MapFrom(Rubric rubric)
        {
            Id = rubric.Id;
            Name = rubric.Name;
            RubricPosts = rubric.RubricPosts.Select(rp => new PostDto().MapFrom(rp.Post)).ToList();

            return this;
        }
    }
}
