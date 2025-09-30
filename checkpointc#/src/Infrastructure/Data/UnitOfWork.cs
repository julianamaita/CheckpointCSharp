using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public interface IUnitOfWork
{
    IRepository<Domain.Entities.User> Users { get; }
    IRepository<Domain.Entities.SelfAssessment> SelfAssessments { get; }
    IRepository<Domain.Entities.JournalEntry> JournalEntries { get; }
    IRepository<Domain.Entities.Goal> Goals { get; }
    IRepository<Domain.Entities.Alert> Alerts { get; }

    Task<int> SaveChangesAsync();
}

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _ctx;
    public IRepository<Domain.Entities.User> Users { get; }
    public IRepository<Domain.Entities.SelfAssessment> SelfAssessments { get; }
    public IRepository<Domain.Entities.JournalEntry> JournalEntries { get; }
    public IRepository<Domain.Entities.Goal> Goals { get; }
    public IRepository<Domain.Entities.Alert> Alerts { get; }

    public UnitOfWork(AppDbContext ctx)
    {
        _ctx = ctx;
        Users = new Repository<Domain.Entities.User>(_ctx);
        SelfAssessments = new Repository<Domain.Entities.SelfAssessment>(_ctx);
        JournalEntries = new Repository<Domain.Entities.JournalEntry>(_ctx);
        Goals = new Repository<Domain.Entities.Goal>(_ctx);
        Alerts = new Repository<Domain.Entities.Alert>(_ctx);
    }

    public Task<int> SaveChangesAsync() => _ctx.SaveChangesAsync();
}
