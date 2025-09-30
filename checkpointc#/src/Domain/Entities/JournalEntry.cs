using System;

namespace Domain.Entities;

public class JournalEntry
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }

    public DateTime Date { get; set; } = DateTime.UtcNow;
    public decimal Amount { get; set; }
    public string Note { get; set; } = string.Empty;
}
