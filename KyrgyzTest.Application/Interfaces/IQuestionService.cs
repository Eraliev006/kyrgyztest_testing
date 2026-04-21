using KyrgyzTest.Application.DTOs;
using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Enums;

namespace KyrgyzTest.Application.Interfaces;

public interface IQuestionService
{
    Task<List<QuestionResponseDto>> GetAllAsync(SectionType? section, LanguageLevel? level);
    Task<QuestionResponseDto?> GetByIdAsync(Guid id);
    Task<QuestionResponseDto> CreateAsync(CreateQuestionDto dto);
    Task<QuestionResponseDto> UpdateAsync(Guid id, CreateQuestionDto dto);
    Task DeleteAsync(Guid id);
    Task<MediaGroupResponseDto> CreateMediaGroupAsync(CreateMediaGroupDto dto);
}