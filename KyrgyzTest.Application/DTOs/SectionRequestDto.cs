using KyrgyzTest.Core.Entities;

namespace KyrgyzTest.Application.DTOs;

public class SectionRequestDto
{
    public Guid AttemptId { get; set; }
    public SectionType Section { get; set; }
}
