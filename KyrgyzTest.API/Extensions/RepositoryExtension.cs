using KyrgyzTest.Core.Interfaces;
using KyrgyzTest.Infrastructure.Repositories;

namespace KyrgyzTest.API.Extensions;

public static class RepositoryExtension
{
    public static IServiceCollection AddRepositories(
        this IServiceCollection services)
    {
        services.AddScoped<IUserRepository,UserRepository>();
        services.AddScoped<ICandidateRepository, CandidateRepository>();
        services.AddScoped<IQuestionRepository, QuestionRepository>();
        services.AddScoped<IMediaGroupRepository, MediaGroupRepository>();
        
        return services;
    } 
}