using System.Collections.Generic;

namespace Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public List<SelfAssessment> Assessments { get; set; } = new();
    public List<JournalEntry> JournalEntries { get; set; } = new();
    public List<Alert> Alerts { get; set; } = new();
    public List<Goal> Goals { get; set; } = new();
}
