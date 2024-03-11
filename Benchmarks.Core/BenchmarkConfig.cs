namespace Benchmarks.Core;

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
