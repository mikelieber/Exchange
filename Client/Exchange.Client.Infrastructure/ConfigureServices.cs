using Exchange.Client.Domain.Models;
using Exchange.Client.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Exchange.Client.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddAnalyzer(this IServiceCollection services)
    {
        return services.AddSingleton<AnalyzerService>();
    }

    public static IServiceCollection AddReceiver(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ReceiverOptions>(configuration.GetRequiredSection(ReceiverOptions.Section));
        services.AddHostedService<BroadcastReceiverService>();
        return services;
    }
}