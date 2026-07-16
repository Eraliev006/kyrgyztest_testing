using KyrgyzTest.Application.Interfaces;
using KyrgyzTest.Application.Services;

namespace KyrgyzTest.API.Extensions;

public static class ServiceExtension
{
    public static IServiceCollection AddServices(
        this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddSingleton<CandidateCacheInvalidator>();
        services.AddScoped<ICandidateService, CandidateService>();
        services.AddScoped<IQuestionService, QuestionService>();
        services.AddScoped<ISectionConfigService, SectionConfigService>();
        services.AddScoped<ITestVariantGeneratorService, TestVariantGeneratorService>();
        services.AddScoped<IExamService, ExamService>();
        services.AddScoped<ITestVariantService, TestVariantService>();
        services.AddScoped<IOrganizationService, OrganizationService>();
        services.AddScoped<IResultService, ResultService>();
        services.AddScoped<ITopicService, TopicService>();
        services.AddScoped<IAuditService, AuditService>();
        services.AddHttpContextAccessor();
        return services;
    }
}