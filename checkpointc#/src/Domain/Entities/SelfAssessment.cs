using System;

namespace Domain.Entities;

public class SelfAssessment
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }

    public int Score { get; set; } // 0-100
    public string RiskLevel { get; set; } = "Low";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
