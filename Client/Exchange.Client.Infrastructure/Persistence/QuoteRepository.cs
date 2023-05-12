using Exchange.Abstractions;
using Exchange.Client.Application.Common.Interfaces;
using Exchange.Models;
using Microsoft.EntityFrameworkCore;

namespace Exchange.Client.Infrastructure;

public sealed class QuoteRepository : IQuoteRepository, IDisposable
{
    private readonly QuotesContext _context;
    private bool disposed;

    public QuoteRepository(QuotesContext context)
    {
        _context = context;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public void DeleteQuote(int id)
    {
        var quote = _context.Quotes.Find(id);
        if (quote != null)
            _context.Quotes.Remove(quote);
    }

    public IFinancialQuote? GetById(int id)
    {
        return _context.Quotes.Find(id);
    }

    public IEnumerable<IFinancialQuote> GetQuotes()
    {
        return _context.Quotes.ToArray();
    }

    public void InsertQuote(IFinancialQuote quote)
    {
        _context.Quotes.Add(FinancialQuote.Create(quote));
    }

    public void Save()
    {
        _context.SaveChanges();
    }

    public void UpdateQuote(IFinancialQuote quote)
    {
        _context.Entry(quote).State = EntityState.Modified;
    }

    private void Dispose(bool disposing)
    {
        if (!disposed)
            if (disposing)
                _context.Dispose();
        disposed = true;
    }
}