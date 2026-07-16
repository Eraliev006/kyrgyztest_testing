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

public class ExamStartTests
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

    public ExamStartTests()
    {
        _sectionConfigRepo.GetAll().Returns(new List<SectionConfig>());
        _completedSectionRepo.GetByAttemptIdAsync(Arg.Any<Guid>()).Returns(new List<CompletedSection>());
    }

    [Fact]
    public async Task Start_ReturnsExistingActiveAttempt_WhenAlreadyStarted()
    {
        var (existingAttempt, _) = TestData.BuildMcqAttempt(grammarCount: 2);
        var candidate = TestData.DefaultCandidate(existingAttempt.CandidateId);

        _candidateRepo.GetByIdAsync(candidate.Id).Returns(candidate);
        _attemptRepo.GetActiveByCandidate(candidate.Id).Returns(existingAttempt);
        _attemptRepo.GetByIdWithDetailsAsync(existingAttempt.Id).Returns(existingAttempt);

        var result = await BuildService().StartAsync(candidate.Id);

        Assert.Equal(existingAttempt.Id, result.AttemptId);
        await _attemptRepo.DidNotReceive().AddAsync(Arg.Any<Attempt>());
        await _generator.DidNotReceive().GenerateAsync();
    }

    [Fact]
    public async Task Start_CreatesNewAttempt_WhenNoActiveAttemptExists()
    {
        var candidate = TestData.DefaultCandidate(Guid.NewGuid());
        var (existingAttempt, _) = TestData.BuildMcqAttempt(grammarCount: 2);

        _candidateRepo.GetByIdAsync(candidate.Id).Returns(candidate);
        _attemptRepo.GetActiveByCandidate(candidate.Id).Returns((Attempt?)null);
        _generator.GenerateAsync().Returns(existingAttempt.TestVariant);

        var result = await BuildService().StartAsync(candidate.Id);

        await _attemptRepo.Received(1).AddAsync(Arg.Any<Attempt>());
        Assert.NotEqual(Guid.Empty, result.AttemptId);
    }

    [Fact]
    public async Task Start_ThrowsBusinessException_WhenCandidateNotAllowed()
    {
        var candidate = TestData.DefaultCandidate(Guid.NewGuid());
        candidate.IsAllowed = false;

        _candidateRepo.GetByIdAsync(candidate.Id).Returns(candidate);

        await Assert.ThrowsAsync<BusinessException>(() => BuildService().StartAsync(candidate.Id));
        await _attemptRepo.DidNotReceive().AddAsync(Arg.Any<Attempt>());
    }
}
