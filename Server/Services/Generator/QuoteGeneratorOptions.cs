namespace Server.Services.Generator;

internal sealed class QuoteGeneratorOptions
{
    public const string Section = "QuoteOptions";
    public QuoteSettings[] Settings { get; set; }
}