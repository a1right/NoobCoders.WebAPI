using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoobCoders.Domain
{
    public class PostRubric
    {
        public long PostId { get; set; }
        public Post Post { get; set; }
        public long RubricId { get; set; }
        public Rubric Rubric { get; set; }
    }
}
