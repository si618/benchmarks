namespace Benchmarks.App;

internal record Benchmark(
    string Name,
    string Description,
    Uri? Link = null,
    Category Category = Category.None);
