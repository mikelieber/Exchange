﻿using Client.Models;
using Client.Services.Analyzer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Models;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace Client.Services.Receiver;

internal sealed class BroadcastReceiverService : BackgroundService
{
    private int _totalReceived;
    private int _totalProcessed;
    private Task _receivingDataTask;
    private Dictionary<string, List<FinancialQuote>> _tickerQuotes;

    private readonly UdpClient _udpClient;
    private readonly ReceiverOptions _options;
    private readonly AnalyzerService _analyzer;
    private readonly ILogger<BroadcastReceiverService> _logger;

    public BroadcastReceiverService(ILogger<BroadcastReceiverService> logger,
        IOptions<ReceiverOptions> options, AnalyzerService analyzer)
    {
        _logger = logger;
        _analyzer = analyzer;
        _options = options.Value;
        _udpClient = new UdpClient();
        _tickerQuotes = new Dictionary<string, List<FinancialQuote>>();

        _udpClient.ExclusiveAddressUse = _options.IsExclusiveAddress;
        _udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, _options.OptionName, true);
        _udpClient.Client.Bind(_options.LocalEndpoint);

        foreach (var group in _options.MulticastGroupsAddresses)
            _udpClient.JoinMulticastGroup(group, _options.LocalIPAddress);

        _udpClient.BeginReceive(new AsyncCallback(ReceivedCounterCallback), null);
    }

    private async Task HandleIncomingAsync(byte[] bytes, CancellationToken ct)
    {
        try
        {
            var quote = JsonSerializer.Deserialize<FinancialQuote>(Encoding.UTF8.GetString(bytes));

            if (!_tickerQuotes.ContainsKey(quote.Symbol))
                _tickerQuotes[quote.Symbol] = new List<FinancialQuote>();

            _tickerQuotes[quote.Symbol].Add(quote);
            _totalProcessed++;
        }
        catch (Exception)
        {
            // Ignore
        }

        await Task.CompletedTask;
    }

    public override async Task StartAsync(CancellationToken ct)
    {
        _receivingDataTask = Task.Run(() => ReceiveDataAsync(ct), ct);
        await base.StartAsync(ct);
    }

    private async Task ReceiveDataAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            await Task.Delay(1 * 1000);
            var receivedData = await _udpClient.ReceiveAsync(ct);
            await Task.Run(() => HandleIncomingAsync(receivedData.Buffer, ct), ct);
        }
    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            _logger.LogInformation("Press ENTER to update results");
            Console.ReadKey();
            _logger.LogInformation($"Total packets: {_totalReceived}\tProcessed: {_totalProcessed}");
            await Task.Run(() => AnalyzeAsync(ct));
        }
    }

    private async Task AnalyzeAsync(CancellationToken ct)
    {
        if (_tickerQuotes.Count == 0) return;
        AnalysisResult[] results = await _analyzer.AnalyzeAsync(_tickerQuotes, ct);
        for (int i = 0; i < results.Length; i++)
            _logger.LogInformation(results[i].ToString());
    }

    private void ReceivedCounterCallback(IAsyncResult ar)
    {
        IPEndPoint sender = new(0, 0);
        Byte[] receivedBytes = _udpClient.EndReceive(ar, ref sender);
        _totalReceived++;
        _udpClient.BeginReceive(new AsyncCallback(ReceivedCounterCallback), null);
    }
}