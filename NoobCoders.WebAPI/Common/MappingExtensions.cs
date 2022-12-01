using NoobCoders.Domain;
using NoobCoders.WebAPI.Models;

namespace NoobCoders.WebAPI.Common
{
    public static class MappingExtensions
    {
        public static IEnumerable<PostResponse> MapToResponse(this IEnumerable<Post> posts)
        {
            return posts.Select(post => new PostResponse().MapFrom(post)).ToList();
        }
        public static PostResponse MapToResponse(this Post rubric)
        {
            var response = new PostResponse()
            {
                Id = rubric.Id,
                Text = rubric.Text,
                CreatedDate = rubric.CreatedDate,
                Rubrics = rubric.PostRubrics.Select(pr => new RubricDto().MapFrom(pr.Rubric)).ToList(),
            };
            return response;
        }
        public static IEnumerable<RubricResponse> MapToResponse(this IEnumerable<Rubric> rubrics)
        {
            return rubrics.Select(rubric => new RubricResponse().MapFrom(rubric)).ToList();
        }
        public static RubricResponse MapToResponse(this Rubric rubric)
        {
            var response = new RubricResponse()
            {
                Id = rubric.Id,
                Name = rubric.Name,
                RubricPosts = rubric.RubricPosts.Select(rp => new PostDto().MapFrom(rp.Post)).ToList(),
            };
            return response;
        }
    }
}
