using KyrgyzTest.Application.Interfaces;
using KyrgyzTest.Application.Services;
using Xunit;
using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Enums;
using KyrgyzTest.Core.Exceptions;
using KyrgyzTest.Core.Interfaces;
using NSubstitute;

namespace KyrgyzTest.Tests;

public class TestVariantReplaceTests
{
    private readonly ITestVariantRepository _variantRepo = Substitute.For<ITestVariantRepository>();
    private readonly IQuestionRepository _questionRepo = Substitute.For<IQuestionRepository>();
    private readonly IAttemptRepository _attemptRepo = Substitute.For<IAttemptRepository>();
    private readonly ITestVariantGeneratorService _generator = Substitute.For<ITestVariantGeneratorService>();
    private readonly IAuditService _audit = Substitute.For<IAuditService>();

    private TestVariantService BuildService() => new(_variantRepo, _questionRepo, _attemptRepo, _generator, _audit);

    private static (TestVariant variant, Question oldQuestion) BuildVariantWithQuestion(
        SectionType section = SectionType.Grammar,
        LanguageLevel level = LanguageLevel.A1,
        QuestionType type = QuestionType.MCQ)
    {
        var oldQuestion = new Question
        {
            Id = Guid.NewGuid(),
            Section = section,
            Level = level,
            Type = type,
            Content = "Old question"
        };

        var tvq = new TestVariantQuestion
        {
            Id = Guid.NewGuid(),
            QuestionId = oldQuestion.Id,
            Question = oldQuestion,
            OrderIndex = 0
        };

        var variant = new TestVariant
        {
            Id = Guid.NewGuid(),
            Number = 1,
            Questions = new List<TestVariantQuestion> { tvq }
        };

        return (variant, oldQuestion);
    }

    // ── успешная замена ───────────────────────────────────────────────────────

    [Fact]
    public async Task Replace_SameSectionLevelType_Succeeds()
    {
        var (variant, old) = BuildVariantWithQuestion();
        var newQuestion = new Question
        {
            Id = Guid.NewGuid(),
            Section = old.Section,
            Level = old.Level,
            Type = old.Type,
            Content = "New question"
        };

        _variantRepo.GetByIdAsync(variant.Id).Returns(variant);
        _questionRepo.GetByIdAsync(newQuestion.Id).Returns(newQuestion);

        // должен пройти без исключений
        await BuildService().ReplaceQuestionAsync(variant.Id, old.Id, newQuestion.Id);

        await _variantRepo.Received(1).SaveAsync();
    }

    [Fact]
    public async Task Replace_Updates_QuestionId_In_Variant()
    {
        var (variant, old) = BuildVariantWithQuestion();
        var newQuestion = new Question
        {
            Id = Guid.NewGuid(),
            Section = old.Section,
            Level = old.Level,
            Type = old.Type,
            Content = "New question"
        };

        _variantRepo.GetByIdAsync(variant.Id).Returns(variant);
        _questionRepo.GetByIdAsync(newQuestion.Id).Returns(newQuestion);

        await BuildService().ReplaceQuestionAsync(variant.Id, old.Id, newQuestion.Id);

        Assert.Equal(newQuestion.Id, variant.Questions.First().QuestionId);
    }

    // ── валидация секции / уровня / типа ──────────────────────────────────────

    [Fact]
    public async Task Replace_DifferentSection_Throws_ValidationException()
    {
        var (variant, old) = BuildVariantWithQuestion(section: SectionType.Grammar);
        var newQuestion = new Question
        {
            Id = Guid.NewGuid(),
            Section = SectionType.Reading, // другая секция
            Level = old.Level,
            Type = old.Type
        };

        _variantRepo.GetByIdAsync(variant.Id).Returns(variant);
        _questionRepo.GetByIdAsync(newQuestion.Id).Returns(newQuestion);

        await Assert.ThrowsAsync<ValidationException>(
            () => BuildService().ReplaceQuestionAsync(variant.Id, old.Id, newQuestion.Id));
    }

    [Fact]
    public async Task Replace_DifferentLevel_Throws_ValidationException()
    {
        var (variant, old) = BuildVariantWithQuestion(level: LanguageLevel.A1);
        var newQuestion = new Question
        {
            Id = Guid.NewGuid(),
            Section = old.Section,
            Level = LanguageLevel.B2, // другой уровень
            Type = old.Type
        };

        _variantRepo.GetByIdAsync(variant.Id).Returns(variant);
        _questionRepo.GetByIdAsync(newQuestion.Id).Returns(newQuestion);

        await Assert.ThrowsAsync<ValidationException>(
            () => BuildService().ReplaceQuestionAsync(variant.Id, old.Id, newQuestion.Id));
    }

    [Fact]
    public async Task Replace_DifferentType_Throws_ValidationException()
    {
        var (variant, old) = BuildVariantWithQuestion(type: QuestionType.MCQ);
        var newQuestion = new Question
        {
            Id = Guid.NewGuid(),
            Section = old.Section,
            Level = old.Level,
            Type = QuestionType.WordOrder // другой тип
        };

        _variantRepo.GetByIdAsync(variant.Id).Returns(variant);
        _questionRepo.GetByIdAsync(newQuestion.Id).Returns(newQuestion);

        await Assert.ThrowsAsync<ValidationException>(
            () => BuildService().ReplaceQuestionAsync(variant.Id, old.Id, newQuestion.Id));
    }

    // ── несуществующие сущности ───────────────────────────────────────────────

    [Fact]
    public async Task Replace_QuestionNotInVariant_Throws_NotFoundException()
    {
        var (variant, _) = BuildVariantWithQuestion();
        var strangeQuestionId = Guid.NewGuid(); // не входит в variant

        _variantRepo.GetByIdAsync(variant.Id).Returns(variant);

        await Assert.ThrowsAsync<NotFoundException>(
            () => BuildService().ReplaceQuestionAsync(variant.Id, strangeQuestionId, Guid.NewGuid()));
    }

    [Fact]
    public async Task Replace_NewQuestionNotFound_Throws_NotFoundException()
    {
        var (variant, old) = BuildVariantWithQuestion();
        var missingId = Guid.NewGuid();

        _variantRepo.GetByIdAsync(variant.Id).Returns(variant);
        _questionRepo.GetByIdAsync(missingId).Returns((Question?)null);

        await Assert.ThrowsAsync<NotFoundException>(
            () => BuildService().ReplaceQuestionAsync(variant.Id, old.Id, missingId));
    }

    [Fact]
    public async Task Replace_VariantNotFound_Throws_NotFoundException()
    {
        _variantRepo.GetByIdAsync(Arg.Any<Guid>()).Returns((TestVariant?)null);

        await Assert.ThrowsAsync<NotFoundException>(
            () => BuildService().ReplaceQuestionAsync(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()));
    }

    [Fact]
    public async Task Delete_NoAttempts_RemovesVariant()
    {
        var (variant, _) = BuildVariantWithQuestion();
        _variantRepo.GetByIdAsync(variant.Id).Returns(variant);
        _attemptRepo.ExistsByTestVariantIdAsync(variant.Id).Returns(false);

        await BuildService().DeleteAsync(variant.Id);

        await _variantRepo.Received(1).DeleteAsync(variant);
    }

    [Fact]
    public async Task Delete_HasAttempts_Throws_BusinessException_AndDoesNotDelete()
    {
        var (variant, _) = BuildVariantWithQuestion();
        _variantRepo.GetByIdAsync(variant.Id).Returns(variant);
        _attemptRepo.ExistsByTestVariantIdAsync(variant.Id).Returns(true);

        await Assert.ThrowsAsync<BusinessException>(() => BuildService().DeleteAsync(variant.Id));

        await _variantRepo.DidNotReceive().DeleteAsync(Arg.Any<TestVariant>());
    }

    [Fact]
    public async Task Delete_VariantNotFound_Throws_NotFoundException()
    {
        _variantRepo.GetByIdAsync(Arg.Any<Guid>()).Returns((TestVariant?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => BuildService().DeleteAsync(Guid.NewGuid()));
    }
}
