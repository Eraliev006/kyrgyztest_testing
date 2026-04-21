using KyrgyzTest.Core.Enums;

namespace KyrgyzTest.Core.Entities;

public class MediaGroup
{
    public Guid Id { get; set; }
    public MediaType Type { get; set; }
    public string Content { get; set; } = string.Empty; // url или текст
    public ICollection<Question> Questions { get; set; } = new List<Question>();
}