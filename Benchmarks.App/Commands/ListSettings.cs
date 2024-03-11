namespace Benchmarks.App.Commands;

internal sealed class ListSettings : CommandSettings
{
    [Description("Filter by Benchmarks.<Name> (* wildcards accepted)")]
    [CommandArgument(0, "[filter]")]
    public string? Filter { get; init; }

    [Description("Allow debug configuration to run - use only for development")]
    [CommandOption("--debug")]
    public bool Debug { get; init; }

    [Description("BenchmarkDotNet Exporters: GitHub/StackOverflow/RPlot/CSV/JSON/HTML/XML")]
    [CommandOption("--exporters")]
    public string? Exporters { get; init; }

    public override ValidationResult Validate()
    {
        return ValidationResult.Success();
    }

    public static Type[] BenchmarkTypes() =>
        [.. Reflection.GetBenchmarkTypes()
            .OrderBy(t => t.Name)
            .ThenBy(t => t.Namespace)];

    public string[] BuildArgs()
    {
        var args = new List<string> { "--filter" };

        // No filter means run all benchmarks for specified options
        if (string.IsNullOrEmpty(Filter))
        {
            args.Add("Benchmarks*");
        }
        else
        {
            var benchmarks = Filter.Split(' ');

            foreach (var benchmark in benchmarks)
            {
                if (benchmark.Contains('*'))
                {
                    // User added wildcard so use whatever was passed
                    args.Add(benchmark);
                }
                else if (benchmark.Contains('.'))
                {
                    // User added namespace but no wildcard so add suffix
                    args.Add($"{benchmark}*");
                }
                else
                {
                    // No wildcard or namespace so add wildcard prefix
                    args.Add($"*{benchmark}");
                }
            }
        }

        if (string.IsNullOrEmpty(Exporters))
        {
            return [.. args];
        }

        args.Add("--exporters");
        args.Add(Exporters);

        return [.. args];
    }
}
