using System;

namespace Domain.Entities;

public class Goal
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }

    public string Title { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool Completed { get; set; }
}
