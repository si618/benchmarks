namespace Benchmarks.App.Commands;

internal sealed class WorkflowCommand : Command
{
    [SuppressMessage("ReSharper", "RedundantNullableFlowAttribute")]
    public override int Execute([NotNull] CommandContext context)
    {
        if (BenchmarkRunner.IsDebugConfiguration(true))
        {
            return 1;
        }

        // Exporters: GitHub/StackOverflow/RPlot/CSV/JSON/HTML/XML")]
        var args = new[] { "--filter", $"Benchmarks*", "--exporters", "json" };

        BenchmarkRunner.RunBenchmarks([.. Reflection.GetBenchmarkTypes()], args);

        CombineBenchmarkResults();

        return 0;
    }

    private static void CombineBenchmarkResults(
        string resultsDir = "./BenchmarkDotNet.Artifacts/results",
        string resultsFile = "Benchmarks",
        string searchPattern = "Benchmarks.*.json")
    {
        var resultsPath = Path.Combine(resultsDir, resultsFile + ".json");

        if (!Directory.Exists(resultsDir))
        {
            throw new DirectoryNotFoundException($"Directory not found '{resultsDir}'");
        }

        if (File.Exists(resultsPath))
        {
            File.Delete(resultsPath);
        }

        const string ns = "Benchmarks.XSharp.Benchmarks.";
        var reports = Directory
            .GetFiles(resultsDir, searchPattern, SearchOption.TopDirectoryOnly)
            .OrderBy(report => report[ns.Length..])
            .ToArray();
        if (reports.Length == 0)
        {
            throw new FileNotFoundException($"Reports not found '{searchPattern}'");
        }

        var firstReport = reports.First();
        var combinedReport = JsonNode.Parse(File.ReadAllText(firstReport))!;
        var title = combinedReport["Title"]!;
        var benchmarks = combinedReport["Benchmarks"]!.AsArray();
        SetBenchmarkName(combinedReport["Benchmarks"]![0]!);

        // Rename title whilst keeping original timestamp
        combinedReport["Title"] = $"{resultsFile}{title.GetValue<string>()[^16..]}";

        foreach (var report in reports.Skip(1))
        {
            var node = JsonNode.Parse(File.ReadAllText(report))!["Benchmarks"]!;
            SetBenchmarkName(node[0]!);

            foreach (var benchmark in node.AsArray())
            {
                // Double parse avoids "The node already has a parent" exception
                benchmarks.Add(JsonNode.Parse(benchmark!.ToJsonString())!);
            }
        }

        File.WriteAllText(resultsPath, combinedReport.ToString());
    }

    private static void SetBenchmarkName(JsonNode benchmark) =>
        benchmark["FullName"] = $"{benchmark["Method"]}";
}
