using KyrgyzTest.Core.Entities;

namespace KyrgyzTest.Core.Interfaces;

public interface ISectionTimingRepository
{
    Task<SectionTiming?> GetByAttemptAndSectionAsync(Guid attemptId, SectionType section);

    /// <summary>
    /// Inserts a new timing row for (AttemptId, Section) if one doesn't exist yet, or
    /// returns the existing row unchanged — StartedAt must never reset on repeat calls.
    /// </summary>
    Task<SectionTiming> GetOrStartAsync(Guid attemptId, SectionType section);
}
