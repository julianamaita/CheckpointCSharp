using Domain.Entities;
using Infrastructure.Data;

namespace Application.Services;

public class JournalService
{
    private readonly IUnitOfWork _uow;
    public JournalService(IUnitOfWork uow) => _uow = uow;

    public async Task<JournalEntry> CreateAsync(int userId, decimal amount, string note)
    {
        var entity = new JournalEntry
        {
            UserId = userId,
            Amount = amount,
            Note = note,
            Date = DateTime.UtcNow
        };
        await _uow.JournalEntries.AddAsync(entity);
        await _uow.SaveChangesAsync();
        return entity;
    }
}
