using KyrgyzTest.Application.Interfaces;
using KyrgyzTest.Application.Services;
using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Interfaces;
using KyrgyzTest.Tests.Helpers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace KyrgyzTest.Tests;

public class CandidateCacheInvalidationTests
{
    private readonly ICandidateRepository _repo = Substitute.For<ICandidateRepository>();
    private readonly IAttemptRepository _attemptRepo = Substitute.For<IAttemptRepository>();
    private readonly IAuditService _audit = Substitute.For<IAuditService>();
    private readonly IExamService _examService = Substitute.For<IExamService>();
    private readonly IMemoryCache _cache = new MemoryCache(Options.Create(new MemoryCacheOptions()));
    private readonly CandidateCacheInvalidator _cacheInvalidator = new();

    private CandidateService BuildService() => new(_repo, _attemptRepo, _audit, _cache, _examService, _cacheInvalidator);

    // Regression test for a bug where the cache-invalidation token lived on a per-instance
    // CancellationTokenSource: since CandidateService is scoped (new instance per request),
    // a mutation on one request's instance never invalidated a list cached by another
    // request's instance. The shared CandidateCacheInvalidator singleton fixes this.
    [Fact]
    public async Task Mutation_OnDifferentServiceInstance_InvalidatesCacheFromEarlierInstance()
    {
        var candidate = TestData.DefaultCandidate(Guid.NewGuid());
        var dateFrom = DateTime.UtcNow.Date;
        var dateTo = dateFrom.AddDays(1);
        _repo.GetPagedAsync(null, dateFrom, dateTo, 1, 20)
            .Returns((new List<Candidate> { candidate }, 1));

        var readerInstance = BuildService();
        await readerInstance.GetPagedAsync(null, dateFrom, dateTo, 1, 20);

        _repo.GetByIdAsync(candidate.Id).Returns(candidate);
        _repo.UpdateAsync(Arg.Any<Candidate>()).Returns(ci => ci.Arg<Candidate>());
        var mutatorInstance = BuildService();
        await mutatorInstance.AllowAccessAsync(candidate.Id);

        await _repo.Received(1).GetPagedAsync(null, dateFrom, dateTo, 1, 20);
        await readerInstance.GetPagedAsync(null, dateFrom, dateTo, 1, 20);
        await _repo.Received(2).GetPagedAsync(null, dateFrom, dateTo, 1, 20);
    }
}
