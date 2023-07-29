using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using Exchange.Abstractions;
using Exchange.Client.Infrastructure.Common.Interfaces;
using Exchange.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Exchange.Client.Infrastructure.Services.Background;

public sealed class QuotesHandlerService : BackgroundService
{
    private readonly IQuotesCache _quotesCache;
    private readonly ILogger<QuotesHandlerService> _logger;
    private readonly IBroadcastSubscriberService _subscriber;

    public QuotesHandlerService(IBroadcastSubscriberService subscriber, IQuotesCache quotesCache, ILogger<QuotesHandlerService> logger)
    {
        _logger = logger;
        _subscriber = subscriber;
        _quotesCache = quotesCache;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var message = await _subscriber.GetLastMessageAsync(cancellationToken);
            if (message == null)
                continue;

            var quote = ParseQuotesAsync(message.Value);
            if (quote != null)
                _quotesCache.Add(quote);
        }
    }

    private IFinancialQuote? ParseQuotesAsync(UdpReceiveResult message)
    {
        IFinancialQuote? quote = null;

        try
        {
            var json = Encoding.UTF8.GetString(message.Buffer);
            quote = JsonSerializer.Deserialize<FinancialQuote>(json);
        }
        catch (Exception e)
        {
            _logger.LogError("There was an error parsing a quote: {Error}", e.Message);
        }

        return quote;
    }
}