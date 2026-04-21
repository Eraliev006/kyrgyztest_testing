namespace KyrgyzTest.Core.Entities;

public class TestVariantQuestion
{
    public Guid Id { get; set; }
    public Guid TestVariantId { get; set; }
    public TestVariant TestVariant { get; set; } = null!;
    public Guid QuestionId { get; set; }
    public Question Question { get; set; } = null!;
    public int OrderIndex { get; set; }
}