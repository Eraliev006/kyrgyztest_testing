namespace KyrgyzTest.Application.DTOs;

public record CreateCandidateDto(string FullName, string Inn, Guid? OrganizationId);
