namespace Benchmarks.Tests.Architecture;

public class BenchmarkTests
{
    private static readonly Assembly BenchmarkAssembly = typeof(GuidPrimaryKeyBenchmark).Assembly;

    [Fact]
    public void Benchmarks_Should_HaveNameEndingWithBenchmark() =>
        Types.InAssembly(BenchmarkAssembly)
            .That()
            .Inherit(typeof(BenchmarkBase))
            .Should()
            .HaveNameEndingWith("Benchmark");
}
