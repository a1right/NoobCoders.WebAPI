using NoobCoders.Domain;

namespace NoobCoders.WebAPI.Models
{
    public class PostDto
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; }

        public PostDto MapFrom(Post post)
        {
            Id = post.Id;
            Text = post.Text;
            CreatedDate = post.CreatedDate;
            return this;
        }
    }
}
