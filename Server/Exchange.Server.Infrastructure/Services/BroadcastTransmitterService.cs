using System.Net;
using System.Net.Sockets;
using Exchange.Abstractions;
using Exchange.Server.Application.Common.Interfaces;
using Exchange.Server.Domain.Models;
using Exchange.Server.Infrastructure.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Exchange.Server.Infrastructure.Services;

public sealed class BroadcastTransmitterService : BackgroundService
{
    private readonly QuoteGeneratorService _generator;
    private readonly ILogger<BroadcastTransmitterService> _logger;
    private readonly TransmitterOptions _options;
    private readonly UdpClient _udpClient;

    public BroadcastTransmitterService(ILogger<BroadcastTransmitterService> logger, IQuotesStorage quotesStorage,
        IOptions<TransmitterOptions> options, QuoteGeneratorService generator)
    {
        _logger = logger;
        _generator = generator;
        _options = options.Value;
        _udpClient = new UdpClient();

        _udpClient.ExclusiveAddressUse = _options.IsExclusiveAddress;
        _udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, _options.OptionName, true);
        _udpClient.Client.Bind(_options.LocalEndpoint);
    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            var quotes = _generator.GenerateQuotes();
            await SendByListAsync(quotes, ct);
            await Task.Delay(1000, ct);
        }
    }

    private async Task SendByListAsync(IDictionary<string, IFinancialQuote> quotes, CancellationToken ct)
    {
        foreach (var (quoteGroup, quote) in quotes)
        {
            if (!_options.MulticastGroups.TryGetValue(quoteGroup, out var group))
                continue;

            await Task.Run(() => SendMulticast(quote.SerializeToBytes(), IPEndPoint.Parse(group)), ct);

            _logger.LogTrace("{Time} Message broadcasted to {Group}", DateTime.Now, group);
        }
    }

    private async Task SendMulticast(byte[] bufferToSend, IPEndPoint group)
    {
        await _udpClient.SendAsync(bufferToSend, bufferToSend.Length, group);
    }
}