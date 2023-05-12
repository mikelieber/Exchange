using Exchange.Server.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Exchange.Server.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddGenerator(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<QuoteGeneratorOptions>(configuration.GetRequiredSection(QuoteGeneratorOptions.Section));
        services.AddSingleton<QuoteGeneratorService>();
        return services;
    }

    public static IServiceCollection AddTransmitter(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<TransmitterOptions>(configuration.GetRequiredSection(TransmitterOptions.Section));
        services.AddHostedService<BroadcastTransmitterService>();
        return services;
    }
}