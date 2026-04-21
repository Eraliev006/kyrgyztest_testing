using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Enums;

namespace KyrgyzTest.Application.DTOs;

public record CreateQuestionDto(
    SectionType Section,
    LanguageLevel Level,
    QuestionType Type,
    string Content,
    Guid? MediaGroupId,
    List<CreateAnswerOptionDto> AnswerOptions
);

public record CreateAnswerOptionDto(
    string Content,
    bool IsCorrect,
    int OrderIndex
);