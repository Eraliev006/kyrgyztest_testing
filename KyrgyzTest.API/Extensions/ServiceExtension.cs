using KyrgyzTest.Application.Interfaces;
using KyrgyzTest.Application.Services;

namespace KyrgyzTest.API.Extensions;

public static class ServiceExtension
{
    public static IServiceCollection AddServices(
        this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        return services;
    }
}