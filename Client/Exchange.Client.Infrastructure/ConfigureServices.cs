using System.Diagnostics.CodeAnalysis;
using System.Net.Sockets;
using Exchange.Client.Domain.Models;
using Exchange.Client.Infrastructure.Common.Interfaces;
using Exchange.Client.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Exchange.Client.Infrastructure;

[ExcludeFromCodeCoverage]
public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddUdpReceiverClient(configuration);

        services.AddSingleton<IQuotesCache, QuotesCache>();
        services.AddSingleton<IBroadcastSubscriberService, BroadcastSubscriberService>();

        services.AddHostedService<BroadcastReceiverService>();
        services.AddHostedService<QuotesHandlerService>();

        return services;
    }

    private static void AddUdpReceiverClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ReceiverOptions>(configuration.GetRequiredSection(ReceiverOptions.Section));
        services.AddSingleton<UdpClient>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<ReceiverOptions>>().Value;

            var client = new UdpClient();
            client.ExclusiveAddressUse = options.IsExclusiveAddress;
            client.Client.SetSocketOption(SocketOptionLevel.Socket, options.OptionName, true);
            client.Client.Bind(options.LocalEndpoint);

            foreach (var group in options.MulticastGroupsAddresses)
                client.JoinMulticastGroup(group, options.LocalIPAddress);

            return client;
        });
    }
}