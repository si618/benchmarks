namespace Benchmarks.Core.Benchmarking;

public sealed class BenchmarkConfig : ManualConfig
{
    public BenchmarkConfig()
    {
#if DEBUG
        WithOption(ConfigOptions.DisableOptimizationsValidator, true);
#endif
        AddDiagnoser(MemoryDiagnoser.Default);

        AddColumn(StatisticColumn.OperationsPerSecond);
    }
}
