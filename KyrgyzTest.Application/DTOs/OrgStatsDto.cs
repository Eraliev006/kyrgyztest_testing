namespace KyrgyzTest.Application.DTOs;

public class OrgStatsDto
{
    public int TotalCount { get; set; }
    public double AverageScore { get; set; }
    public Dictionary<string, int> ByLevel { get; set; } = new();
}
