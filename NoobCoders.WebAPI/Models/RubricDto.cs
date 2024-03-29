﻿using NoobCoders.Domain;

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
    }
}
