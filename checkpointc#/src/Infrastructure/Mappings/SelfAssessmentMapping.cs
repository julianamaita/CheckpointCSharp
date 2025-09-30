using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Mappings;

public class SelfAssessmentMapping : IEntityTypeConfiguration<SelfAssessment>
{
    public void Configure(EntityTypeBuilder<SelfAssessment> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Score).IsRequired();
        builder.Property(x => x.RiskLevel).HasMaxLength(20);
        builder.Property(x => x.CreatedAt);
    }
}
