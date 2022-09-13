namespace Server.Services.Generator;

internal static class GeneratorExtensions
{
    internal static IServiceCollection AddGenerator(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<QuoteGeneratorOptions>(configuration.GetRequiredSection(QuoteGeneratorOptions.Section));
        services.AddSingleton<FinancialQuoteGeneratorService>();
        return services;
    }
}
