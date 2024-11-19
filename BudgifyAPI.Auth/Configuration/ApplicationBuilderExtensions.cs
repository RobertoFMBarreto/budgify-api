using BudgifyAPI.Auth.Helpers;
using BudgifyAPI.Auth.Helpers.Interfaces;
using BudgifyAPI.Auth.Models.Queries;
using BudgifyAPI.Auth.Models.Queries.Interfaces;
using BudgifyAPI.Auth.Models.Request;
using BudgifyAPI.Auth.Models.Request.Interfaces;
using BudgifyAPI.Auth.Services;
using BudgifyAPI.Auth.Services.Interfaces;

namespace BudgifyAPI.Auth.Configuration;

public static class ApplicationBuilderExtensions
{
    
    public static IServiceCollection AddModels(this IServiceCollection services)
    {
        services.AddScoped<ILoginRequest, LoginRequest>();
        services.AddScoped<IRegisterRequest, RegisterRequest>();
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticationServices, AuthenticationServices>();
        return services;
    }
    
    public static IServiceCollection AddQueries(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticationQueries, AuthenticationQueries>();
        return services;
    }
    public static IServiceCollection AddHelpers(this IServiceCollection services)
    {
        services.AddScoped<IPasetoHelper, PasetoHelper>();
        return services;
    }
}