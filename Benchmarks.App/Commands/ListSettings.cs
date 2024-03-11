namespace Benchmarks.App.Commands;

internal sealed class ListSettings : CommandSettings
{
    [Description("Benchmark name")]
    [CommandArgument(0, "[filter]")]
    public string Name { get; init; } = string.Empty;

    [Description("Allow debug configuration to run - use only for development")]
    [CommandOption("--debug")]
    public bool Debug { get; init; }

    [Description("BenchmarkDotNet Exporters: GitHub/StackOverflow/RPlot/CSV/JSON/HTML/XML")]
    [CommandOption("--exporters")]
    public string? Exporters { get; init; }

    public override ValidationResult Validate()
    {
        if (!Reflection.GetBenchmarkTypes().Any(type => type.Name == Name))
        {
            return ValidationResult.Error($"Benchmark not found {Name}");
        }

        return ValidationResult.Success();
    }

    public string[] BuildArgs()
    {
        var args = new List<string> { "--filter", $"*{Name}*" };

        if (!string.IsNullOrEmpty(Exporters))
        {
            args.Add("--exporters");
            args.Add(Exporters);
        }

        return [.. args];
    }
}
