namespace Benchmarks.Core.Extensions;

public static class StringExtensions
{
    public static bool EqualsIgnoreCase(this string value, string? other) =>
        string.Equals(value, other, StringComparison.InvariantCultureIgnoreCase);
}
