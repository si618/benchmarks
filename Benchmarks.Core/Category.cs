﻿namespace Benchmarks.Core;

public enum Category
{
    None,
    Database
}

public static partial class CategoryExtensions
{
    public static string Description(this Category category) =>
        category == Category.None ? string.Empty : category.ToString();
}
