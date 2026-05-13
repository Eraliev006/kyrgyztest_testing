using KyrgyzTest.Core.Entities;

namespace KyrgyzTest.Application.DTOs;

public class SectionConfigResponseDto
{
    public Guid Id { get; set; }
    public SectionType Section { get; set; }
    public int TimeLimitMinutes { get; set; }
    public int A1Count { get; set; }
    public int A2Count { get; set; }
    public int B1Count { get; set; }
    public int B2Count { get; set; }
}
