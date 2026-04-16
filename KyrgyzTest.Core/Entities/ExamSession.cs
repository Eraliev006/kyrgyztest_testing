using KyrgyzTest.Core.Enums;

namespace KyrgyzTest.Core.Entities;

public class ExamSession
{
    public Guid Id { get; set; }
    public DateTime ScheduledAt { get; set; }
    public Guid CreatedBy { get; set; }
    public ExamSessionStatus Status { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}