using System.Net.Sockets;

namespace Exchange.Client.Infrastructure.Common.Interfaces;

public interface IBroadcastSubscriberService
{
    ValueTask<UdpReceiveResult?> GetLastMessageAsync(CancellationToken cancellationToken);
}