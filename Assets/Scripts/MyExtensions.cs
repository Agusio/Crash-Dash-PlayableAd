using System.Collections.Generic;
using UnityEngine;

public static class MyExtensions
{
    public static List<T> Shuffle<T>(this List<T> source)
    {
        if (source == null) return null;

        for (int i = source.Count - 1; i > 0; i--)
        {
            var r = Random.Range(0, i + 1);
            (source[i], source[r]) = (source[r], source[i]);
        }
        return source;
    }
}