namespace Exchange.Server.Domain.Models;

public sealed class QuoteGeneratorOptions
{
    public const string Section = "QuoteOptions";
    public QuoteSetting[] Settings { get; set; }
}