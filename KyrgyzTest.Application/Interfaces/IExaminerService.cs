using KyrgyzTest.Application.DTOs;

namespace KyrgyzTest.Application.Interfaces;

public interface IExaminerService
{
    Task<List<ExaminerQueueItemDto>> GetQueueAsync();
    Task<ExaminerReviewDto> GetReviewAsync(Guid attemptId);
    Task GradeAsync(GradeAnswerDto dto, Guid examinerUserId);
}
