namespace KyrgyzTest.Core.Entities;

public class TestVariant
{
    public Guid Id { get; set; }
    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
    public ICollection<TestVariantQuestion> Questions { get; set; } = new List<TestVariantQuestion>();
}