using KyrgyzTest.Application.Interfaces;
using KyrgyzTest.Application.Services;
using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Enums;
using KyrgyzTest.Core.Exceptions;
using KyrgyzTest.Core.Interfaces;
using KyrgyzTest.Tests.Helpers;
using NSubstitute;
using Xunit;

namespace KyrgyzTest.Tests;

public class ExamSectionStartTests
{
    private readonly ICandidateRepository _candidateRepo = Substitute.For<ICandidateRepository>();
    private readonly IAttemptRepository _attemptRepo = Substitute.For<IAttemptRepository>();
    private readonly ISectionConfigRepository _sectionConfigRepo = Substitute.For<ISectionConfigRepository>();
    private readonly ITestVariantGeneratorService _generator = Substitute.For<ITestVariantGeneratorService>();
    private readonly ICandidateAnswerRepository _answerRepo = Substitute.For<ICandidateAnswerRepository>();
    private readonly IResultRepository _resultRepo = Substitute.For<IResultRepository>();
    private readonly ICompletedSectionRepository _completedSectionRepo = Substitute.For<ICompletedSectionRepository>();
    private readonly ISectionTimingRepository _sectionTimingRepo = Substitute.For<ISectionTimingRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    private ExamService BuildService() => new(
        _candidateRepo, _attemptRepo, _sectionConfigRepo,
        _generator, _answerRepo, _resultRepo, _completedSectionRepo, _sectionTimingRepo, _unitOfWork);

    public ExamSectionStartTests()
    {
        _completedSectionRepo.GetByAttemptIdAsync(Arg.Any<Guid>()).Returns(new List<CompletedSection>());
    }

    [Fact]
    public async Task StartSection_SetsDeadline_FromSectionTimingAndConfig()
    {
        var (attempt, _) = TestData.BuildMcqAttempt(grammarCount: 2);
        var startedAt = new DateTime(2026, 1, 1, 10, 0, 0, DateTimeKind.Utc);

        _attemptRepo.GetByIdWithDetailsAsync(attempt.Id).Returns(attempt);
        _sectionConfigRepo.GetAll().Returns(new List<SectionConfig>
        {
            new() { Section = SectionType.Grammar, TimeLimitMinutes = 20 }
        });
        _sectionTimingRepo.GetOrStartAsync(attempt.Id, SectionType.Grammar).Returns(new SectionTiming
        {
            Id = Guid.NewGuid(),
            AttemptId = attempt.Id,
            Section = SectionType.Grammar,
            StartedAt = startedAt
        });

        var result = await BuildService().StartSectionAsync(attempt.Id, SectionType.Grammar);

        Assert.Equal(startedAt.AddMinutes(20), result.DeadlineUtc);
        Assert.Equal(2, result.Questions.Count);
    }

    [Fact]
    public async Task StartSection_CalledTwice_ReturnsSameDeadline()
    {
        // GetOrStartAsync is idempotent — the second call must not push the deadline back.
        var (attempt, _) = TestData.BuildMcqAttempt(grammarCount: 1);
        var startedAt = new DateTime(2026, 1, 1, 10, 0, 0, DateTimeKind.Utc);

        _attemptRepo.GetByIdWithDetailsAsync(attempt.Id).Returns(attempt);
        _sectionConfigRepo.GetAll().Returns(new List<SectionConfig>
        {
            new() { Section = SectionType.Grammar, TimeLimitMinutes = 20 }
        });
        _sectionTimingRepo.GetOrStartAsync(attempt.Id, SectionType.Grammar).Returns(new SectionTiming
        {
            Id = Guid.NewGuid(),
            AttemptId = attempt.Id,
            Section = SectionType.Grammar,
            StartedAt = startedAt
        });

        var service = BuildService();
        var first = await service.StartSectionAsync(attempt.Id, SectionType.Grammar);
        var second = await service.StartSectionAsync(attempt.Id, SectionType.Grammar);

        Assert.Equal(first.DeadlineUtc, second.DeadlineUtc);
    }

    [Fact]
    public async Task StartSection_ThrowsBusinessException_WhenSectionAlreadyCompleted()
    {
        var (attempt, _) = TestData.BuildMcqAttempt(grammarCount: 1);
        _attemptRepo.GetByIdWithDetailsAsync(attempt.Id).Returns(attempt);
        _completedSectionRepo.GetByAttemptIdAsync(attempt.Id).Returns(new List<CompletedSection>
        {
            new() { Id = Guid.NewGuid(), AttemptId = attempt.Id, Section = SectionType.Grammar, CompletedAt = DateTime.UtcNow }
        });

        await Assert.ThrowsAsync<BusinessException>(
            () => BuildService().StartSectionAsync(attempt.Id, SectionType.Grammar));
    }

    [Fact]
    public async Task GetSectionStatus_ReturnsNullDeadline_WhenSectionNeverStarted()
    {
        var attemptId = Guid.NewGuid();
        _sectionTimingRepo.GetByAttemptAndSectionAsync(attemptId, SectionType.Grammar).Returns((SectionTiming?)null);

        var status = await BuildService().GetSectionStatusAsync(attemptId, SectionType.Grammar);

        Assert.Null(status.DeadlineUtc);
        Assert.False(status.IsCompleted);
    }

    [Fact]
    public async Task GetSectionStatus_ReturnsComputedDeadline_WhenSectionStarted()
    {
        var attemptId = Guid.NewGuid();
        var startedAt = new DateTime(2026, 1, 1, 10, 0, 0, DateTimeKind.Utc);
        _sectionTimingRepo.GetByAttemptAndSectionAsync(attemptId, SectionType.Grammar).Returns(new SectionTiming
        {
            Id = Guid.NewGuid(),
            AttemptId = attemptId,
            Section = SectionType.Grammar,
            StartedAt = startedAt
        });
        _sectionConfigRepo.GetAll().Returns(new List<SectionConfig>
        {
            new() { Section = SectionType.Grammar, TimeLimitMinutes = 15 }
        });

        var status = await BuildService().GetSectionStatusAsync(attemptId, SectionType.Grammar);

        Assert.Equal(startedAt.AddMinutes(15), status.DeadlineUtc);
    }
}
