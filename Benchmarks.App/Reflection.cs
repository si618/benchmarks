namespace Benchmarks.App;

internal static class Reflection
{
    public static bool TryGetBenchmark(string name, out Benchmark benchmark)
    {
        var found = GetBenchmarkTypes()
            .Select(GetBenchmark)
            .Where(type => type.Name == name)
            .FirstOrDefault(p =>
                p.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase) ||
                p.Description.Equals(name, StringComparison.InvariantCultureIgnoreCase));

        benchmark = found ?? null!;

        return found is not null;
    }

    private static Benchmark GetBenchmark(MemberInfo memberInfo)
    {
        ArgumentNullException.ThrowIfNull(nameof(memberInfo));

        var attribute = memberInfo.GetCustomAttribute<BenchmarkInfoAttribute>();

        if (attribute is null)
        {
            throw new InvalidOperationException($"Invalid member info '{memberInfo.Name}");
        }

        return new Benchmark(
            memberInfo.Name,
            attribute.Description,
            attribute.Link,
            attribute.Category);
    }

    public static IEnumerable<Type> GetBenchmarkTypes() =>
        typeof(GuidPrimaryKey).Assembly
            .GetTypes()
            .Where(m => m.GetCustomAttribute(typeof(BenchmarkInfoAttribute)) is not null)
            .OrderBy(t => t.Name);

    public static IEnumerable<IGrouping<Category, Benchmark>> GetBenchmarksByCategory() =>
        GetBenchmarkTypes()
            .Select(GetBenchmark)
            .OrderBy(p => p.Category)
            .ThenBy(p => p.Description)
            .GroupBy(p => p.Category, p => p);
}
