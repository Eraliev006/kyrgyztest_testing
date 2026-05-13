namespace KyrgyzTest.Application.DTOs;

public class VariantQuestionDto
{
    public Guid Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public string Section { get; set; } = string.Empty;
    public string Level { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
}
