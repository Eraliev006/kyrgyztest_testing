using Microsoft.Extensions.Primitives;

namespace KyrgyzTest.Application.Services;

// Registered as a singleton so the change token survives across scoped CandidateService
// instances: one request's Invalidate() must cancel the token another request's cache
// entry was built with.
public sealed class CandidateCacheInvalidator
{
    private CancellationTokenSource _cts = new();

    public IChangeToken GetChangeToken() => new CancellationChangeToken(_cts.Token);

    public void Invalidate()
    {
        var previous = _cts;
        _cts = new CancellationTokenSource();
        previous.Cancel();
        previous.Dispose();
    }
}
