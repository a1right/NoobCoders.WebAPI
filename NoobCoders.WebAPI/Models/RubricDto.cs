using System.Text.Json.Serialization;
using NoobCoders.Domain;

namespace NoobCoders.WebAPI.Models
{
    public class RubricDto
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public RubricDto MapFrom(Rubric rubric)
        {
            Id = rubric.Id;
            Name = rubric.Name;
            return this;
        }

        public IEnumerable<RubricDto> MapFrom(IEnumerable<Rubric> rubrics)
        {
            return rubrics.Select(r => new RubricDto().MapFrom(r));
        }
    }
}
