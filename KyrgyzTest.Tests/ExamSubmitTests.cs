using KyrgyzTest.Application.Interfaces;
using KyrgyzTest.Application.Services;
using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Enums;
using KyrgyzTest.Core.Interfaces;
using KyrgyzTest.Tests.Helpers;
using NSubstitute;
using Xunit;

namespace KyrgyzTest.Tests;

public class ExamSubmitTests
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

    [Fact]
    public async Task Submit_IsIdempotent_ReturnsExistingResult_WithoutCallingTryAdd()
    {
        var (attempt, _) = TestData.BuildMcqAttempt(grammarCount: 2);
        var existingResult = new Result
        {
            Id = Guid.NewGuid(),
            AttemptId = attempt.Id,
            CandidateId = attempt.CandidateId,
            Level = LanguageLevel.B1,
            TotalScore = 1,
            CreatedAt = DateTime.UtcNow
        };

        _attemptRepo.GetByIdWithDetailsAsync(attempt.Id).Returns(attempt);
        _resultRepo.GetByAttemptIdAsync(attempt.Id).Returns(existingResult);
        _candidateRepo.GetByIdAsync(attempt.CandidateId)
            .Returns(TestData.DefaultCandidate(attempt.CandidateId));
        _candidateRepo.UpdateAsync(Arg.Any<Candidate>()).Returns(ci => ci.Arg<Candidate>());

        var response = await BuildService().SubmitAsync(attempt.Id);

        Assert.Equal(existingResult.Id, response.Id);
        Assert.Equal(existingResult.Level, response.Level);
        await _resultRepo.DidNotReceive().TryAddAsync(Arg.Any<Result>());
    }

    [Fact]
    public async Task Submit_PartialFailure_CompletesCleanup_WhenAttemptStillInProgress()
    {
        // Result exists in DB but Attempt is still InProgress (partial failure scenario).
        var (attempt, _) = TestData.BuildMcqAttempt(grammarCount: 2);
        attempt.Status = AttemptStatus.InProgress;

        var existingResult = new Result
        {
            Id = Guid.NewGuid(),
            AttemptId = attempt.Id,
            CandidateId = attempt.CandidateId,
            Level = LanguageLevel.A1,
            CreatedAt = DateTime.UtcNow
        };
        var candidate = TestData.DefaultCandidate(attempt.CandidateId);

        _attemptRepo.GetByIdWithDetailsAsync(attempt.Id).Returns(attempt);
        _resultRepo.GetByAttemptIdAsync(attempt.Id).Returns(existingResult);
        _candidateRepo.GetByIdAsync(attempt.CandidateId).Returns(candidate);
        _candidateRepo.UpdateAsync(Arg.Any<Candidate>()).Returns(ci => ci.Arg<Candidate>());

        await BuildService().SubmitAsync(attempt.Id);

        // Attempt must be marked Completed.
        await _attemptRepo.Received(1).UpdateAsync(Arg.Is<Attempt>(a => a.Status == AttemptStatus.Completed));
        // Candidate must have IsAllowed cleared.
        await _candidateRepo.Received(1).UpdateAsync(Arg.Is<Candidate>(c => !c.IsAllowed));
    }

    [Fact]
    public async Task Submit_PartialFailure_SkipsAttemptUpdate_WhenAlreadyCompleted()
    {
        // Result exists and Attempt is already Completed — only Candidate cleanup needed.
        var (attempt, _) = TestData.BuildMcqAttempt(grammarCount: 2);
        attempt.Status = AttemptStatus.Completed;

        var existingResult = new Result
        {
            Id = Guid.NewGuid(),
            AttemptId = attempt.Id,
            CandidateId = attempt.CandidateId,
            Level = LanguageLevel.A1,
            CreatedAt = DateTime.UtcNow
        };
        var candidate = TestData.DefaultCandidate(attempt.CandidateId);

        _attemptRepo.GetByIdWithDetailsAsync(attempt.Id).Returns(attempt);
        _resultRepo.GetByAttemptIdAsync(attempt.Id).Returns(existingResult);
        _candidateRepo.GetByIdAsync(attempt.CandidateId).Returns(candidate);
        _candidateRepo.UpdateAsync(Arg.Any<Candidate>()).Returns(ci => ci.Arg<Candidate>());

        await BuildService().SubmitAsync(attempt.Id);

        await _attemptRepo.DidNotReceive().UpdateAsync(Arg.Any<Attempt>());
    }

    [Fact]
    public async Task Submit_ConcurrentRace_ReturnsExistingResult_WhenTryAddReturnsFalse()
    {
        // Both requests pass the idempotency check simultaneously (no Result yet),
        // but TryAddAsync returns false because the other request already inserted.
        var (attempt, _) = TestData.BuildMcqAttempt(grammarCount: 2);
        var candidate = TestData.DefaultCandidate(attempt.CandidateId);

        var raceResult = new Result
        {
            Id = Guid.NewGuid(),
            AttemptId = attempt.Id,
            CandidateId = attempt.CandidateId,
            Level = LanguageLevel.A2,
            CreatedAt = DateTime.UtcNow
        };

        _attemptRepo.GetByIdWithDetailsAsync(attempt.Id).Returns(attempt);
        _answerRepo.GetByAttemptIdAsync(attempt.Id).Returns(new List<CandidateAnswer>());
        // First call (idempotency check) returns null; second call (after TryAdd fails) returns the race result.
        _resultRepo.GetByAttemptIdAsync(attempt.Id).Returns((Result?)null, raceResult);
        _resultRepo.TryAddAsync(Arg.Any<Result>()).Returns(false);
        _candidateRepo.GetByIdAsync(attempt.CandidateId).Returns(candidate);
        _candidateRepo.UpdateAsync(Arg.Any<Candidate>()).Returns(ci => ci.Arg<Candidate>());

        var response = await BuildService().SubmitAsync(attempt.Id);

        Assert.Equal(raceResult.Id, response.Id);
    }
}
