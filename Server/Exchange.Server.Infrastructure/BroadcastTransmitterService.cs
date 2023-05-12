using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using Exchange.Abstractions;
using Exchange.Server.Domain.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Exchange.Server.Infrastructure;

public sealed class BroadcastTransmitterService : BackgroundService
{
    private readonly QuoteGeneratorService _generator;
    private readonly ILogger<BroadcastTransmitterService> _logger;
    private readonly TransmitterOptions _options;
    private readonly UdpClient _udpClient;

    public BroadcastTransmitterService(ILogger<BroadcastTransmitterService> logger,
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
            var quotes = await _generator.GenerateQuotesAsync(ct);
            await Task.Run(() => SendByListAsync(quotes, ct), ct);
        }
    }

    private async Task SendByListAsync(IDictionary<string, IFinancialQuote> quotes, CancellationToken ct)
    {
        byte[] ToBytes(IFinancialQuote quote)
        {
            var message = JsonSerializer.Serialize(quote);
            return Encoding.UTF8.GetBytes(message);
        }

        foreach (var quote in quotes)
        {
            var group = _options.MulticastGroups[quote.Key];
            if (string.IsNullOrEmpty(group)) continue;
            await Task.Run(() => SendMulticast(ToBytes(quote.Value), IPEndPoint.Parse(group)), ct);
            _logger.LogInformation("{0} Message broadcasted to {1}", DateTime.Now, group);
        }
    }

    private async Task SendMulticast(byte[] bufferToSend, IPEndPoint group)
    {
        await _udpClient.SendAsync(bufferToSend, bufferToSend.Length, group);
    }
}