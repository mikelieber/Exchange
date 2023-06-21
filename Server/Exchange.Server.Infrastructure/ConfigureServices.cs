using Exchange.Server.Application.Common.Interfaces;
using Exchange.Server.Domain.Models;
using Exchange.Server.Infrastructure.Services;
using Exchange.Server.Infrastructure.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Exchange.Server.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IQuotesStorage, QuotesStorage>();
        services.AddGenerator(configuration);
        services.AddTransmitter(configuration);

        return services;
    }

    private static IServiceCollection AddGenerator(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<QuoteGeneratorOptions>(configuration.GetRequiredSection(QuoteGeneratorOptions.Section));
        services.AddSingleton<QuoteGeneratorService>();
        return services;
    }

    private static IServiceCollection AddTransmitter(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<TransmitterOptions>(configuration.GetRequiredSection(TransmitterOptions.Section));
        services.AddHostedService<BroadcastTransmitterService>();
        return services;
    }
}