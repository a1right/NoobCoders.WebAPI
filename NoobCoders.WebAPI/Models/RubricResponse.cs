using System.Text.Json.Serialization;
using NoobCoders.Domain;

namespace NoobCoders.WebAPI.Models
{
    public class RubricResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<Post> Posts { get; set; }
    }
}
