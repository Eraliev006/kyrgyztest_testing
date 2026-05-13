using KyrgyzTest.Core.Entities;

namespace KyrgyzTest.Application.Interfaces;

public interface ITestVariantGeneratorService
{
    Task<TestVariant> GenerateAsync();
}
