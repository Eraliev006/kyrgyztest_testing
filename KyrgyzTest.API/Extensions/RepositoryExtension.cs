using KyrgyzTest.Core.Interfaces;
using KyrgyzTest.Infrastructure.Persistence;
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
        services.AddScoped<ISectionConfigRepository, SectionConfigRepository>();
        services.AddScoped<ITestVariantRepository, TestVariantRepository>();
        services.AddScoped<IAttemptRepository, AttemptRepository>();
        services.AddScoped<ICandidateAnswerRepository, CandidateAnswerRepository>();
        services.AddScoped<IResultRepository, ResultRepository>();
        services.AddScoped<ICompletedSectionRepository, CompletedSectionRepository>();
        services.AddScoped<ISectionTimingRepository, SectionTimingRepository>();
        services.AddScoped<IManualGradeRepository, ManualGradeRepository>();
        services.AddScoped<IOrganizationRepository, OrganizationRepository>();
        services.AddScoped<ITopicRepository, TopicRepository>();
        services.AddScoped<IAuditLogRepository, AuditLogRepository>();
        services.AddScoped<IPasswordResetRequestRepository, PasswordResetRequestRepository>();
        services.AddScoped<IExamAccessSettingsRepository, ExamAccessSettingsRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    } 
}