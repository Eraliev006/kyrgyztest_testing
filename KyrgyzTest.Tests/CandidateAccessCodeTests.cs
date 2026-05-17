using System.Text.RegularExpressions;
using Xunit;
using KyrgyzTest.Application.DTOs;
using KyrgyzTest.Application.Interfaces;
using KyrgyzTest.Application.Services;
using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Interfaces;
using NSubstitute;

namespace KyrgyzTest.Tests;

public class CandidateAccessCodeTests
{
    private readonly ICandidateRepository _repo = Substitute.For<ICandidateRepository>();
    private readonly IAttemptRepository _attemptRepo = Substitute.For<IAttemptRepository>();
    private readonly IAuditService _audit = Substitute.For<IAuditService>();

    private CandidateService BuildService() => new(_repo, _attemptRepo, _audit);

    public CandidateAccessCodeTests()
    {
        // CreateAsync checks for duplicates by INN first
        _repo.GetByInnAsync(Arg.Any<string>()).Returns((Candidate?)null);
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
            var dto = new CreateCandidateDto($"User {i}", $"INN{i:D10}", null);
            var result = await BuildService().CreateAsync(dto);
            codes.Add(result.AccessCode);
        }
        Assert.True(codes.Count > 1);
    }
}
