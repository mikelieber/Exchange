using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using Exchange.Abstractions;
using Exchange.Client.Infrastructure.Common.Interfaces;
using Exchange.Models;
using Microsoft.Extensions.Hosting;

namespace Exchange.Client.Infrastructure.Services;

public sealed class QuotesHandlerService : BackgroundService
{
    private readonly IBroadcastSubscriberService _subscriber;
    private readonly IQuotesCache _quotesCache;

    public QuotesHandlerService(IBroadcastSubscriberService subscriber, IQuotesCache quotesCache)
    {
        _subscriber = subscriber;
        _quotesCache = quotesCache;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var message = await _subscriber.GetLastMessageAsync(cancellationToken);
            var quote = ParseQuotesAsync(message);
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
            // Ignored
            // TODO: Add logging
        }

        return quote;
    }
}