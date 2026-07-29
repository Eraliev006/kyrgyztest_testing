using KyrgyzTest.Application.DTOs;

namespace KyrgyzTest.Application.Interfaces;

public interface IExamAccessSettingsService
{
    Task<ExamAccessSettingsDto> GetAsync();
    Task<ExamAccessSettingsDto> UpdateAsync(UpdateExamAccessPasswordDto dto);
}
