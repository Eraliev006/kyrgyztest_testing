using KyrgyzTest.Core.Entities;

namespace KyrgyzTest.Core.Interfaces;

public interface IExamAccessSettingsRepository
{
    Task<ExamAccessSettings?> GetAsync();
    Task UpdateAsync(ExamAccessSettings settings);
}
