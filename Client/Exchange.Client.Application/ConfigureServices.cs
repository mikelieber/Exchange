using System.Diagnostics.CodeAnalysis;
using Exchange.Client.Application.Common.Interfaces;
using Exchange.Client.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Exchange.Client.Application;

[ExcludeFromCodeCoverage]
public static class ConfigureServices
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IAnalyzerService, AnalyzerService>();

        return services;
    }
}