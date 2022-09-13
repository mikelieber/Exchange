namespace Client.Services.Receiver;

internal static class ReceiverExtensions
{
    internal static IServiceCollection AddReceiver(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ReceiverOptions>(configuration.GetRequiredSection(ReceiverOptions.Section));
        services.AddHostedService<BroadcastReceiverService>();
        return services;
    }
}
