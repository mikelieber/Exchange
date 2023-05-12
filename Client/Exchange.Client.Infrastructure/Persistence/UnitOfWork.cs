using System.ComponentModel.DataAnnotations;
using Exchange.Client.Application.Common.Interfaces;

namespace Exchange.Client.Infrastructure;

public class Ticker
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; }
}

public class UnitOfWork : IDisposable
{
    private readonly QuotesContext _context;
    private bool _disposed;
    private IQuoteRepository _quoteRepository;
    private GenericRepository<Ticker> _tickerRepository;

    public UnitOfWork(QuotesContext context)
    {
        _context = context;
    }

    public GenericRepository<Ticker> TickerRepository
    {
        get
        {
            if (_tickerRepository == null)
                _tickerRepository = new GenericRepository<Ticker>(_context);

            return _tickerRepository;
        }
    }

    public IQuoteRepository QuoteRepository
    {
        get
        {
            if (_quoteRepository == null)
                _quoteRepository = new QuoteRepository(_context);

            return _quoteRepository;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public void Save()
    {
        _context.SaveChanges();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
            if (disposing)
                _context.Dispose();
        _disposed = true;
    }
}