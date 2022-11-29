using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NoobCoders.Domain
{
    public class Rubric
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public ICollection<PostRubric> RubricPosts { get; set; }
    }
}
