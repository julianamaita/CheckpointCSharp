using Domain.Entities;
using Infrastructure.Data;

namespace Application.Services;

public class AssessmentService
{
    private readonly IUnitOfWork _uow;
    public AssessmentService(IUnitOfWork uow) => _uow = uow;

    public static string ComputeRisk(int score)
    {
        if (score >= 80) return "High";
        if (score >= 50) return "Medium";
        return "Low";
    }

    public async Task<SelfAssessment> CreateAsync(int userId, int score)
    {
        var risk = ComputeRisk(score);
        var entity = new SelfAssessment
        {
            UserId = userId,
            Score = score,
            RiskLevel = risk,
            CreatedAt = DateTime.UtcNow
        };
        await _uow.SelfAssessments.AddAsync(entity);

        if (risk != "Low")
        {
            await _uow.Alerts.AddAsync(new Alert
            {
                UserId = userId,
                Level = risk == "High" ? "Critical" : "Warning",
                Message = $"Autoavaliação com risco {risk} (score {score}).",
                CreatedAt = DateTime.UtcNow
            });
        }

        await _uow.SaveChangesAsync();
        return entity;
    }
}
