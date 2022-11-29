using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoobCoders.Domain
{
    public class Post
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; }
        public ICollection<PostRubric> PostRubrics { get; set; }
    }
}
