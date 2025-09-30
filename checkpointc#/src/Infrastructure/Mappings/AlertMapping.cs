using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Mappings;

public class AlertMapping : IEntityTypeConfiguration<Alert>
{
    public void Configure(EntityTypeBuilder<Alert> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Level).HasMaxLength(30);
        builder.Property(x => x.Message).HasMaxLength(400);
        builder.Property(x => x.CreatedAt);
    }
}
