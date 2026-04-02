using KyrgyzTest.Core.Enums;

namespace KyrgyzTest.Application.DTOs;

public class RegisterCandidateDto
{
    public string FullName { get; set; } = string.Empty;
    public string PassportNumber { get; set; } = string.Empty;
    public CandidateCategory Category { get; set; }
}