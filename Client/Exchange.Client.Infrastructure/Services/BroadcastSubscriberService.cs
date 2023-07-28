﻿using System.Net;
using System.Net.Sockets;
using Exchange.Client.Infrastructure.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace Exchange.Client.Infrastructure.Services;

public sealed class BroadcastSubscriberService : IBroadcastSubscriberService
{
    private readonly UdpClient _client;
    private readonly ILogger<BroadcastSubscriberService> _logger;

    public int TotalPackages { get; private set; }

    public BroadcastSubscriberService(ILogger<BroadcastSubscriberService> logger, UdpClient client)
    {
        _logger = logger;
        _client = client;

        _client.BeginReceive(ReceiveCallback, null);
    }

    private void ReceiveCallback(IAsyncResult result)
    {
        var sender = new IPEndPoint(0, 0);
        _client.EndReceive(result, ref sender);
        _client.BeginReceive(ReceiveCallback, null);
        TotalPackages++;
    }

    public async ValueTask<UdpReceiveResult> GetLastMessageAsync(CancellationToken cancellationToken)
    {
        return await _client.ReceiveAsync(cancellationToken);
    }
}