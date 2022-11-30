using Nest;
using NoobCoders.Domain;
using NoobCoders.WebAPI.Models;
using static System.Net.Mime.MediaTypeNames;

namespace NoobCoders.WebAPI.Common
{
    public static class MappingExtensions
    {
        public static IEnumerable<PostResponse> MapToResponse(this IEnumerable<Post> posts)
        {
            return posts.Select(post => new PostResponse().MapFrom(post)).ToList();
        }
        public static PostResponse MapToResponse(this Post post)
        {
            var response = new PostResponse()
            {
                Id = post.Id,
                Text = post.Text,
                CreatedDate = post.CreatedDate,
                Rubrics = post.PostRubrics.Select(pr => new RubricDto().MapFrom(pr.Rubric)).ToList(),
            };
            return response;
        }
    }
}
