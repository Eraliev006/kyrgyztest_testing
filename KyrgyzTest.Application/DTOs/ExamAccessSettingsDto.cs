namespace KyrgyzTest.Application.DTOs;

public class ExamAccessSettingsDto
{
    public string AccessPassword { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; set; }
    public string? UpdatedByFullName { get; set; }
}
