﻿using NoobCoders.Domain;

namespace NoobCoders.WebAPI.Models
{
    public class PostResponse
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<RubricDto> Rubrics { get; set; }

        public PostResponse MapFrom(Post post)
        {
            Id = post.Id;
            Text = post.Text;
            CreatedDate = post.CreatedDate;
            Rubrics = post.PostRubrics.Select(pr => new RubricDto().MapFrom(pr.Rubric)).ToList();

            return this;
        }
    }
}
