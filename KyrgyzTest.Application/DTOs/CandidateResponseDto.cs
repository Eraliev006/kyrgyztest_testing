namespace KyrgyzTest.Application.DTOs;

public class CandidateResponseDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Inn { get; set; } = string.Empty;
    public string AccessCode { get; set; } = string.Empty;
    public bool IsAllowed { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid? OrganizationId { get; set; }
    public string? Photo { get; set; }
    public DateTime? BlockedUntil { get; set; }
}