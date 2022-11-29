using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NoobCoders.Domain;

namespace NoobCoders.Repository.EntityConfiguration
{
    public class PostRubricConfiguration : IEntityTypeConfiguration<PostRubric>
    {
        public void Configure(EntityTypeBuilder<PostRubric> builder)
        {
            builder.HasKey(pr => new { pr.PostId, pr.RubricId });
            builder.ToTable("PostRubric");

            builder.HasOne(pr => pr.Post)
                   .WithMany(p => p.PostRubrics)
                   .HasForeignKey(pr => pr.PostId);

            builder.HasOne(pr => pr.Rubric)
                   .WithMany(r => r.RubricPosts)
                   .HasForeignKey(pr => pr.RubricId);
        }
    }
}
