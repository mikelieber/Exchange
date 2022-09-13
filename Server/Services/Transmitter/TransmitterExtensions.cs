namespace Server.Services.Transmitter;

internal static class TransmitterExtensions
{
    internal static IServiceCollection AddTransmitter(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<TransmitterOptions>(configuration.GetRequiredSection(TransmitterOptions.Section));
        services.AddHostedService<BroadcastTransmitterService>();
        return services;
    }
}
