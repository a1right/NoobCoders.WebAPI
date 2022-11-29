using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NoobCoders.Domain;

namespace NoobCoders.Repository.EntityConfiguration
{
    public class RubricConfiguration : IEntityTypeConfiguration<Rubric>
    {
        public void Configure(EntityTypeBuilder<Rubric> builder)
        {
            builder.ToTable("Rubric");
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Name)
                   .IsUnique()
                   .HasDatabaseName("UK_Rubric_name");
            builder.Property(r => r.Name).HasColumnName("name");
        }
    }
}