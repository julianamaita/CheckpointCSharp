using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Mappings;

public class UserMapping : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(120);
        builder.Property(x => x.Email).IsRequired().HasMaxLength(180);
        builder.HasIndex(x => x.Email).IsUnique();

        builder.HasMany(u => u.Assessments)
               .WithOne(a => a.User!)
               .HasForeignKey(a => a.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.JournalEntries)
               .WithOne(j => j.User!)
               .HasForeignKey(j => j.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.Goals)
               .WithOne(g => g.User!)
               .HasForeignKey(g => g.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.Alerts)
               .WithOne(a => a.User!)
               .HasForeignKey(a => a.UserId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
