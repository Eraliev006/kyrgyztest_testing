using KyrgyzTest.Application.Interfaces;
using KyrgyzTest.Application.Services;
using KyrgyzTest.Core.Interfaces;
using KyrgyzTest.Infrastructure.Repositories;

namespace KyrgyzTest.API.Extensions;

public static class RepositoryExtension
{
    public static IServiceCollection AddRepositories(
        this IServiceCollection services)
    {
        services.AddScoped<ICandidateRepository, CandidateRepository>();
        services.AddScoped<IExamSessionRepository, ExamSessionRepository>();
        services.AddScoped<IComputerRepository, ComputerRepository>();
        services.AddScoped<IExamService, ExamService>();
        services.AddScoped<IComputerService, ComputerService>();

        return services;
    } 
}