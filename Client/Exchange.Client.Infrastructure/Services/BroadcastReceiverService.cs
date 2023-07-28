using System.Text;
using System.Text.Json;
using Exchange.Abstractions;
using Exchange.Client.Application.Common.Interfaces;
using Exchange.Client.Domain.Models;
using Exchange.Client.Infrastructure.Common.Interfaces;
using Exchange.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Exchange.Client.Infrastructure.Services;

internal sealed class BroadcastReceiverService : BackgroundService
{
    private const string PackagesLog = "Total packets: {TotalReceived} \t Processed: {TotalProcessed}";

    private readonly IAnalyzerService _analyzer;
    private readonly ILogger<BroadcastReceiverService> _logger;
    private readonly IQuotesCache _quotesCache;

    private int _totalProcessed;
    private int _totalReceived;

    public BroadcastReceiverService(ILogger<BroadcastReceiverService> logger, IAnalyzerService analyzer,
        IQuotesCache quotesCache)
    {
        _logger = logger;
        _analyzer = analyzer;
        _quotesCache = quotesCache;
    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            _logger.LogInformation(PackagesLog, _totalReceived, _totalProcessed);
            _logger.LogInformation("Press ENTER to update results");
            Console.ReadKey();

            await Task.Run(() =>
            {
                var quotes = _quotesCache.GetQuotes();
                var results = _analyzer.Analyze(quotes);
                foreach (var result in results)
                    _logger.LogInformation(result.ToString());
            }, ct);
        }
    }

    public override async Task StartAsync(CancellationToken ct)
    {
        //_receivingDataTask = Task.Run(() => ReceiveDataAsync(ct), ct);
        await base.StartAsync(ct);
    }

    private async Task ReceiveDataAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            await Task.Delay(1 * 1000);
            //var receivedData = await _udpClient.ReceiveAsync(ct);

            //await Task.Run(() => HandleIncomingAsync(receivedData.Buffer, ct), ct);
        }
    }
}