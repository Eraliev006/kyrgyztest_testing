namespace KyrgyzTest.Core.Entities;

public class Topic
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public SectionType? SectionType { get; set; }
    public ICollection<Question> Questions { get; set; } = new List<Question>();
}
