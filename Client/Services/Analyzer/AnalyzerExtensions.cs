namespace Client.Services.Analyzer;

internal static class AnalyzerExtensions
{
    internal static IServiceCollection AddAnalyzer(this IServiceCollection services)
    {
        return services.AddSingleton<AnalyzerService>();
    }
}