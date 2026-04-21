using KyrgyzTest.Core.Enums;
namespace KyrgyzTest.Core.Entities;


public class Candidate
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Inn { get; set; } = string.Empty;
    public string AccessCode { get; set; } = string.Empty;
    public bool IsAllowed { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}