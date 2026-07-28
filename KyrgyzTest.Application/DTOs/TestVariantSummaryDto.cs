namespace KyrgyzTest.Application.DTOs;

public class TestVariantSummaryDto
{
    public Guid Id { get; set; }
    public int Number { get; set; }
    public DateTime GeneratedAt { get; set; }
    public int QuestionCount { get; set; }
    public bool IsArchived { get; set; }
}
