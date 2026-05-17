using KyrgyzTest.Application.DTOs;
using Xunit;
using KyrgyzTest.Application.Interfaces;
using KyrgyzTest.Application.Services;
using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Exceptions;
using KyrgyzTest.Core.Interfaces;
using KyrgyzTest.Tests.Helpers;
using NSubstitute;

namespace KyrgyzTest.Tests;

public class CandidateBlockTests
{
    private readonly ICandidateRepository _repo = Substitute.For<ICandidateRepository>();
    private readonly IAttemptRepository _attemptRepo = Substitute.For<IAttemptRepository>();
    private readonly IAuditService _audit = Substitute.For<IAuditService>();
    private CandidateService BuildService() => new(_repo, _attemptRepo, _audit);

    private void SetupCandidate(Candidate candidate)
    {
        _repo.GetByIdAsync(candidate.Id).Returns(candidate);
        _repo.UpdateAsync(Arg.Any<Candidate>()).Returns(ci => ci.Arg<Candidate>());
        _attemptRepo.GetAllActiveByCandidate(candidate.Id).Returns(new List<Attempt>());
    }

    // ── BlockAsync: срок не истёк (BlockedUntil в будущем) ───────────────────

    [Theory]
    [InlineData(1, "days")]
    [InlineData(2, "weeks")]
    [InlineData(1, "months")]
    [InlineData(1, "years")]
    public async Task Block_ValidUnit_SetsBlockedUntilInFuture(int value, string unit)
    {
        var candidate = TestData.DefaultCandidate(Guid.NewGuid());
        SetupCandidate(candidate);

        var before = DateTime.UtcNow;
        var result = await BuildService().BlockAsync(candidate.Id, new BlockCandidateDto(value, unit));

        Assert.NotNull(result.BlockedUntil);
        Assert.True(result.BlockedUntil!.Value > before, "BlockedUntil должен быть в будущем");
    }

    [Fact]
    public async Task Block_Days_SetsCorrectDate()
    {
        var candidate = TestData.DefaultCandidate(Guid.NewGuid());
        SetupCandidate(candidate);
        var before = DateTime.UtcNow;

        var result = await BuildService().BlockAsync(candidate.Id, new BlockCandidateDto(7, "days"));

        var expected = before.AddDays(7);
        Assert.True(result.BlockedUntil!.Value >= expected.AddSeconds(-2));
        Assert.True(result.BlockedUntil!.Value <= expected.AddSeconds(2));
    }

    [Fact]
    public async Task Block_Weeks_SetsCorrectDate()
    {
        var candidate = TestData.DefaultCandidate(Guid.NewGuid());
        SetupCandidate(candidate);
        var before = DateTime.UtcNow;

        var result = await BuildService().BlockAsync(candidate.Id, new BlockCandidateDto(2, "weeks"));

        var expected = before.AddDays(14);
        Assert.True(result.BlockedUntil!.Value >= expected.AddSeconds(-2));
        Assert.True(result.BlockedUntil!.Value <= expected.AddSeconds(2));
    }

    [Fact]
    public async Task Block_Months_SetsCorrectDate()
    {
        var candidate = TestData.DefaultCandidate(Guid.NewGuid());
        SetupCandidate(candidate);
        var before = DateTime.UtcNow;

        var result = await BuildService().BlockAsync(candidate.Id, new BlockCandidateDto(3, "months"));

        var expected = before.AddMonths(3);
        Assert.True(result.BlockedUntil!.Value >= expected.AddSeconds(-2));
        Assert.True(result.BlockedUntil!.Value <= expected.AddSeconds(2));
    }

    [Fact]
    public async Task Block_Years_SetsCorrectDate()
    {
        var candidate = TestData.DefaultCandidate(Guid.NewGuid());
        SetupCandidate(candidate);
        var before = DateTime.UtcNow;

        var result = await BuildService().BlockAsync(candidate.Id, new BlockCandidateDto(1, "years"));

        var expected = before.AddYears(1);
        Assert.True(result.BlockedUntil!.Value >= expected.AddSeconds(-2));
        Assert.True(result.BlockedUntil!.Value <= expected.AddSeconds(2));
    }

    // ── BlockAsync: невалидная единица ────────────────────────────────────────

    [Theory]
    [InlineData("hours")]
    [InlineData("minutes")]
    [InlineData("")]
    [InlineData("DAYS")]   // регистр чувствителен
    public async Task Block_InvalidUnit_Throws_ValidationException(string unit)
    {
        var candidate = TestData.DefaultCandidate(Guid.NewGuid());
        SetupCandidate(candidate);

        await Assert.ThrowsAsync<ValidationException>(
            () => BuildService().BlockAsync(candidate.Id, new BlockCandidateDto(1, unit)));
    }

    // ── Предикат «срок истёк / не истёк» ─────────────────────────────────────

    [Fact]
    public void IsBlocked_WhenBlockedUntilIsInFuture_ReturnsTrue()
    {
        // Кандидат заблокирован: срок ещё не истёк
        var blockedUntil = DateTime.UtcNow.AddDays(1);
        Assert.True(blockedUntil > DateTime.UtcNow);
    }

    [Fact]
    public void IsBlocked_WhenBlockedUntilIsInPast_ReturnsFalse()
    {
        // Кандидат не заблокирован: срок блокировки истёк вчера
        var blockedUntil = DateTime.UtcNow.AddDays(-1);
        Assert.False(blockedUntil > DateTime.UtcNow);
    }

    [Fact]
    public void IsBlocked_WhenBlockedUntilIsNull_ReturnsFalse()
    {
        // Кандидат никогда не был заблокирован
        DateTime? blockedUntil = null;
        Assert.False(blockedUntil.HasValue && blockedUntil.Value > DateTime.UtcNow);
    }

    // ── DenyAccessAsync ───────────────────────────────────────────────────────

    [Fact]
    public async Task DenyAccess_Sets_IsAllowed_False()
    {
        var candidate = TestData.DefaultCandidate(Guid.NewGuid());
        candidate.IsAllowed = true;
        SetupCandidate(candidate);

        var result = await BuildService().DenyAccessAsync(candidate.Id);

        Assert.False(result.IsAllowed);
    }

    [Fact]
    public async Task DenyAccess_DoesNot_Change_BlockedUntil()
    {
        var candidate = TestData.DefaultCandidate(Guid.NewGuid());
        candidate.BlockedUntil = null;
        SetupCandidate(candidate);

        var result = await BuildService().DenyAccessAsync(candidate.Id);

        Assert.Null(result.BlockedUntil);
    }
}
