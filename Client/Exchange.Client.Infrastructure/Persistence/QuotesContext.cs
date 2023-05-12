using Exchange.Models;
using Microsoft.EntityFrameworkCore;

namespace Exchange.Client.Infrastructure;

public class QuotesContext : DbContext
{
    public QuotesContext(DbContextOptions<QuotesContext> options) : base(options)
    {
    }

    public DbSet<FinancialQuote> Quotes { get; set; }
}