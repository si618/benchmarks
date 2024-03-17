namespace Benchmarks.Core.Benchmarking;

public enum Category
{
    None,
    Database
}

public static class CategoryExtensions
{
    public static string Description(this Category category) =>
        category == Category.None ? string.Empty : category.ToString();
}
