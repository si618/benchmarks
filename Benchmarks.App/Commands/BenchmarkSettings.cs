namespace Benchmarks.App.Commands;

internal sealed class BenchmarkSettings : CommandSettings
{
    [Description("Benchmark name")]
    [CommandArgument(0, "[filter]")]
    public string Name { get; init; } = string.Empty;

    [Description("Allow debug configuration to run - use only for development")]
    [CommandOption("--debug")]
    public bool Debug { get; init; }

    public override ValidationResult Validate()
    {
        if (!Reflection.GetBenchmarkTypes().Any(type => type.Name == Name))
        {
            return ValidationResult.Error($"Benchmark not found {Name}");
        }

        return ValidationResult.Success();
    }

    public string[] BuildArgs() => ["--filter", $"*{Name}*"];
}
