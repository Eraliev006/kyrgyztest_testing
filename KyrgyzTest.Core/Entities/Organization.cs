namespace KyrgyzTest.Core.Entities;

public class Organization
{
    public Guid Id { get; set; }
    public string NameRu { get; set; } = string.Empty;
    public string NameKg { get; set; } = string.Empty;
    public string ShortName { get; set; } = string.Empty;
}
