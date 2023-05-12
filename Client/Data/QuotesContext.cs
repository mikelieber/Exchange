using Exchange.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Client.Data;

public class QuotesContext : DbContext
{
    public QuotesContext(DbContextOptions<QuotesContext> options) : base(options)
    {
    }

    public DbSet<FinancialQuote> Quotes { get; set; }
}