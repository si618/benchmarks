namespace Benchmarks.App.Commands;

internal sealed class BenchmarkRunSettings : CommandSettings
{
    [Description("Name of benchmark")]
    [CommandArgument(0, "[name]")]
    public string Name { get; init; } = string.Empty;

    public override ValidationResult Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            return ValidationResult.Error("No benchmark argument passed");
        }

        if (!Reflection.TryGetBenchmark(Name, out _))
        {
            return ValidationResult.Error($"Benchmark not found '{Name}'");
        }

        return ValidationResult.Success();
    }
}