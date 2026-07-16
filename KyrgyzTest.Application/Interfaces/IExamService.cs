using KyrgyzTest.Application.DTOs;
using KyrgyzTest.Core.Entities;

namespace KyrgyzTest.Application.Interfaces;

public interface IExamService
{
    Task<StartExamResultDto> StartAsync(Guid candidateId);
    Task<ExamSectionDto> StartSectionAsync(Guid attemptId, SectionType section);
    Task<SectionStatusDto> GetSectionStatusAsync(Guid attemptId, SectionType section);
    Task<SubmitSectionResultDto> SubmitSectionAsync(Guid attemptId, SectionType section);
    Task SaveAnswerAsync(SaveAnswerDto dto);
    Task<ResultResponseDto> SubmitAsync(Guid attemptId);
    Task<bool> HasActiveAttemptAsync(Guid candidateId);
    Task<List<ActiveAttemptDto>> GetActiveAttemptsAsync();
}
