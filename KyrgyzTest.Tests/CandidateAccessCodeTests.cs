using System.Text.RegularExpressions;
using Xunit;
using KyrgyzTest.Application.DTOs;
using KyrgyzTest.Application.Interfaces;
using KyrgyzTest.Application.Services;
using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace KyrgyzTest.Tests;

public class CandidateAccessCodeTests
{
    private readonly ICandidateRepository _repo = Substitute.For<ICandidateRepository>();
    private readonly IAttemptRepository _attemptRepo = Substitute.For<IAttemptRepository>();
    private readonly IAuditService _audit = Substitute.For<IAuditService>();
    private readonly IMemoryCache _cache = new MemoryCache(Options.Create(new MemoryCacheOptions()));
    private readonly IExamService _examService = Substitute.For<IExamService>();
    private readonly CandidateCacheInvalidator _cacheInvalidator = new();

    private CandidateService BuildService() => new(_repo, _attemptRepo, _audit, _cache, _examService, _cacheInvalidator);

    public CandidateAccessCodeTests()
    {
        _repo.GetByInnAsync(Arg.Any<string>()).Returns((Candidate?)null);
        _repo.GetByAccessCodeAsync(Arg.Any<string>()).Returns((Candidate?)null);
        _repo.CreateAsync(Arg.Any<Candidate>()).Returns(ci => ci.Arg<Candidate>());
    }

    [Fact]
    public async Task AccessCode_Format_Is_YYYYMMDD_Dash_4digits()
    {
        var dto = new CreateCandidateDto("Test User", "12345678901234", null);

        var result = await BuildService().CreateAsync(dto);

        Assert.Matches(@"^\d{8}-\d{4}$", result.AccessCode);
    }

    [Fact]
    public async Task AccessCode_DatePart_Is_Today()
    {
        var dto = new CreateCandidateDto("Test User", "12345678901234", null);

        var result = await BuildService().CreateAsync(dto);

        var datePart = result.AccessCode[..8];
        Assert.Equal(DateTime.UtcNow.ToString("yyyyMMdd"), datePart);
    }

    [Fact]
    public async Task AccessCode_NumberPart_Is_Between_1000_And_9999()
    {
        var dto = new CreateCandidateDto("Test User", "12345678901234", null);

        var result = await BuildService().CreateAsync(dto);

        var numberPart = int.Parse(result.AccessCode[9..]);
        Assert.InRange(numberPart, 1000, 9999);
    }

    [Fact]
    public async Task TwoCandidates_Have_Different_AccessCodes_MostOfTheTime()
    {
        // Statistical check: generate 20 codes, expect at least 2 distinct values.
        // The chance of 20 collisions in a 9000-value space is negligible.
        var codes = new HashSet<string>();
        for (int i = 0; i < 20; i++)
        {
            _repo.GetByInnAsync(Arg.Any<string>()).Returns((Candidate?)null);
            _repo.GetByAccessCodeAsync(Arg.Any<string>()).Returns((Candidate?)null);
            var dto = new CreateCandidateDto($"User {i}", $"INN{i:D10}", null);
            var result = await BuildService().CreateAsync(dto);
            codes.Add(result.AccessCode);
        }
        Assert.True(codes.Count > 1);
    }

    [Fact]
    public async Task CreateAsync_RetriesAccessCode_WhenFirstCodeIsAlreadyTaken()
    {
        var takenCandidate = new Candidate { Id = Guid.NewGuid(), AccessCode = "taken" };

        // First access-code lookup returns a taken candidate; subsequent ones return null.
        var callCount = 0;
        _repo.GetByAccessCodeAsync(Arg.Any<string>()).Returns(_ =>
        {
            callCount++;
            return callCount == 1 ? takenCandidate : (Candidate?)null;
        });

        var dto = new CreateCandidateDto("Test User", "12345678901234", null);
        var result = await BuildService().CreateAsync(dto);

        Assert.Matches(@"^\d{8}-\d{4}$", result.AccessCode);
        Assert.True(callCount >= 2, "Expected at least two access-code uniqueness checks (one collision, one success).");
    }

    [Fact]
    public async Task CreateAsync_ThrowsBusinessException_WhenAllAccessCodeAttemptsCollide()
    {
        var takenCandidate = new Candidate { Id = Guid.NewGuid(), AccessCode = "taken" };

        // All access-code lookups return a taken candidate.
        _repo.GetByAccessCodeAsync(Arg.Any<string>()).Returns(takenCandidate);

        var dto = new CreateCandidateDto("Test User", "12345678901234", null);
        await Assert.ThrowsAsync<KyrgyzTest.Core.Exceptions.BusinessException>(() => BuildService().CreateAsync(dto));
    }
}
