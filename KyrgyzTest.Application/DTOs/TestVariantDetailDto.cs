namespace KyrgyzTest.Application.DTOs;

public class TestVariantDetailDto
{
    public Guid Id { get; set; }
    public DateTime GeneratedAt { get; set; }
    public Dictionary<string, List<VariantQuestionDto>> QuestionsBySection { get; set; } = new();
}
