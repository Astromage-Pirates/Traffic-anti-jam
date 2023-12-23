using System;
using System.Collections.Generic;

public static class CollectionExtensions
{
    public static bool IsEmpty<T>(this List<T> list)
    {
        return list.Count == 0;
    }

    public static bool IsEmpty(this Array arr)
    {
        return arr.Length == 0;
    }
}
