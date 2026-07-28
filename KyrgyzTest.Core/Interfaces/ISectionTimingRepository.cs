using KyrgyzTest.Core.Entities;

namespace KyrgyzTest.Core.Interfaces;

public interface ISectionTimingRepository
{
    Task<SectionTiming?> GetByAttemptAndSectionAsync(Guid attemptId, SectionType section);
    Task<SectionTiming> GetOrStartAsync(Guid attemptId, SectionType section);
}
