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

public class ExamSubmitSectionTests
{
    private readonly ICandidateRepository _candidateRepo = Substitute.For<ICandidateRepository>();
    private readonly IAttemptRepository _attemptRepo = Substitute.For<IAttemptRepository>();
    private readonly ISectionConfigRepository _sectionConfigRepo = Substitute.For<ISectionConfigRepository>();
    private readonly ITestVariantGeneratorService _generator = Substitute.For<ITestVariantGeneratorService>();
    private readonly ICandidateAnswerRepository _answerRepo = Substitute.For<ICandidateAnswerRepository>();
    private readonly IResultRepository _resultRepo = Substitute.For<IResultRepository>();
    private readonly ICompletedSectionRepository _completedSectionRepo = Substitute.For<ICompletedSectionRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    private ExamService BuildService() => new(
        _candidateRepo, _attemptRepo, _sectionConfigRepo,
        _generator, _answerRepo, _resultRepo, _completedSectionRepo, _unitOfWork);

    [Fact]
    public async Task SubmitSection_Sequential_DuplicateSubmit_ThrowsBusinessException()
    {
        // Sequential duplicate: completed section already returned by GetByAttemptIdAsync
        var (attempt, _) = TestData.BuildMcqAttempt(grammarCount: 2, listeningCount: 2);
        var alreadyCompleted = new List<CompletedSection>
        {
            new() { Id = Guid.NewGuid(), AttemptId = attempt.Id, Section = SectionType.Grammar }
        };

        _attemptRepo.GetByIdWithDetailsAsync(attempt.Id).Returns(attempt);
        _completedSectionRepo.GetByAttemptIdAsync(attempt.Id).Returns(alreadyCompleted);

        await Assert.ThrowsAsync<BusinessException>(
            () => BuildService().SubmitSectionAsync(attempt.Id, SectionType.Grammar));

        await _completedSectionRepo.DidNotReceive().TryAddAsync(Arg.Any<CompletedSection>());
    }

    [Fact]
    public async Task SubmitSection_ConcurrentDuplicate_ThrowsBusinessException()
    {
        // Concurrent duplicate: sequential check passes (section not in list yet),
        // but TryAddAsync returns false (DB unique constraint caught in Infrastructure).
        var (attempt, _) = TestData.BuildMcqAttempt(grammarCount: 2, listeningCount: 2);

        _attemptRepo.GetByIdWithDetailsAsync(attempt.Id).Returns(attempt);
        _completedSectionRepo.GetByAttemptIdAsync(attempt.Id).Returns(new List<CompletedSection>());
        _completedSectionRepo.TryAddAsync(Arg.Any<CompletedSection>()).Returns(false);

        await Assert.ThrowsAsync<BusinessException>(
            () => BuildService().SubmitSectionAsync(attempt.Id, SectionType.Grammar));
    }

    [Fact]
    public async Task SubmitSection_FirstSubmit_InsertsAndReturnsNotCompleted_WhenSectionsRemain()
    {
        var (attempt, _) = TestData.BuildMcqAttempt(grammarCount: 2, listeningCount: 2);

        _attemptRepo.GetByIdWithDetailsAsync(attempt.Id).Returns(attempt);
        _completedSectionRepo.GetByAttemptIdAsync(attempt.Id).Returns(new List<CompletedSection>());
        _completedSectionRepo.TryAddAsync(Arg.Any<CompletedSection>()).Returns(true);

        var result = await BuildService().SubmitSectionAsync(attempt.Id, SectionType.Grammar);

        Assert.False(result.IsExamCompleted);
        await _completedSectionRepo.Received(1).TryAddAsync(Arg.Any<CompletedSection>());
    }

    [Fact]
    public async Task SubmitSection_LastSection_TriggersSubmitAndReturnsCompleted()
    {
        var (attempt, _) = TestData.BuildMcqAttempt(grammarCount: 2);
        var candidate = TestData.DefaultCandidate(attempt.CandidateId);

        _attemptRepo.GetByIdWithDetailsAsync(attempt.Id).Returns(attempt);
        // One section total (Grammar only), none completed yet
        _completedSectionRepo.GetByAttemptIdAsync(attempt.Id).Returns(new List<CompletedSection>());
        _completedSectionRepo.TryAddAsync(Arg.Any<CompletedSection>()).Returns(true);

        // SubmitAsync deps
        _answerRepo.GetByAttemptIdAsync(attempt.Id).Returns(new List<CandidateAnswer>());
        _resultRepo.GetByAttemptIdAsync(attempt.Id).Returns((Result?)null);
        _resultRepo.TryAddAsync(Arg.Any<Result>()).Returns(true);
        _candidateRepo.GetByIdAsync(attempt.CandidateId).Returns(candidate);
        _candidateRepo.UpdateAsync(Arg.Any<Candidate>()).Returns(ci => ci.Arg<Candidate>());

        var result = await BuildService().SubmitSectionAsync(attempt.Id, SectionType.Grammar);

        Assert.True(result.IsExamCompleted);
        Assert.NotNull(result.Result);
    }
}
