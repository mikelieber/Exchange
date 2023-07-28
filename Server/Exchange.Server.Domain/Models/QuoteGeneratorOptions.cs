using System.Diagnostics.CodeAnalysis;

namespace Exchange.Server.Domain.Models;

[ExcludeFromCodeCoverage]
public sealed class QuoteGeneratorOptions
{
    public const string Section = "QuoteOptions";
    public QuoteSetting[] Settings { get; set; } = null!;
}