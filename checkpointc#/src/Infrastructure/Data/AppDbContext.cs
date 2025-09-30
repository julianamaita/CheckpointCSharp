using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<SelfAssessment> SelfAssessments => Set<SelfAssessment>();
    public DbSet<JournalEntry> JournalEntries => Set<JournalEntry>();
    public DbSet<Goal> Goals => Set<Goal>();
    public DbSet<Alert> Alerts => Set<Alert>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
