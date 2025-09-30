using System;

namespace Domain.Entities;

public class Alert
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }

    public string Level { get; set; } = "Info"; // Info/Warning/Critical
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
