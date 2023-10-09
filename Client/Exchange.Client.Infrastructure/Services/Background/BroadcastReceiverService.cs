using Exchange.Client.Application.Common.Interfaces;
using Exchange.Client.Infrastructure.Common.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Exchange.Client.Infrastructure.Services.Background;

internal sealed class BroadcastReceiverService : BackgroundService
{
    private const string PackagesLog = "Total packets: {TotalReceived} \t Processed: {TotalProcessed}";

    private readonly IAnalyzerService _analyzer;
    private readonly IQuotesCache _quotesCache;
    private readonly IBroadcastSubscriberInfo _subscriberInfo;
    private readonly ILogger<BroadcastReceiverService> _logger;

    private int _totalReceived;
    private int _totalProcessed;

    public BroadcastReceiverService(ILogger<BroadcastReceiverService> logger, IAnalyzerService analyzer,
        IQuotesCache quotesCache, IBroadcastSubscriberInfo subscriberInfo)
    {
        _logger = logger;
        _analyzer = analyzer;
        _quotesCache = quotesCache;
        _subscriberInfo = subscriberInfo;
    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        await Task.Delay(5 * 1000, ct);

        while (!ct.IsCancellationRequested)
        {
            WaitForInput();
            await Task.Run(RunAnalysis, ct);
        }
    }

    private void WaitForInput()
    {
        _logger.LogInformation(PackagesLog, _totalReceived, _totalProcessed);
        _logger.LogInformation("Press any key to print results");
        Console.ReadKey();
    }

    private void RunAnalysis()
    {
        var quotes = _quotesCache.GetQuotes();
        var results = _analyzer.Analyze(quotes);
        foreach (var result in results)
            _logger.LogInformation("{Result}", result);
        _totalProcessed = quotes.Sum(c => c.Value.Count);
        _totalReceived = _subscriberInfo.TotalPackages;
    }
}