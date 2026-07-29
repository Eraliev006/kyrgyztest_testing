namespace KyrgyzTest.Core.Entities;

public class ExamAccessSettings
{
    public Guid Id { get; set; }
    public string AccessPassword { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; set; }
    public Guid? UpdatedByUserId { get; set; }
    public Users? UpdatedBy { get; set; }
}
