﻿namespace Uroskur.Utils;

public static class Extensions
{
    public static IEnumerable<Tuple<int, T, T, T>> GetItems<T>(this IEnumerable<T>? source)
    {
        if (source == null)
        {
            yield break;
        }

        var skip = true;
        T? previous = default, current = default;
        T? next;
        foreach (var (item, index) in source.WithIndex())
        {
            next = item;
            if (!skip && previous != null && current != null)
            {
                yield return new Tuple<int, T, T, T>(index, previous, current, next);
            }

            if (current != null)
            {
                previous = current;
            }

            current = item;
            skip = false;
        }

        if (skip)
        {
            yield break;
        }

        next = default;
        if (previous == null)
        {
            yield break;
        }

        if (current == null)
        {
            yield break;
        }

        if (next != null)
        {
            yield return new Tuple<int, T, T, T>(0, previous, current, next);
        }
    }

    public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> self)
    {
        return self.Select((item, index) => (item, index));
    }

    public static void Sort<T, TKey>(this ObservableCollection<T> collection, Func<ObservableCollection<T>, TKey> sort)
    {
        var sorted = (sort(collection) as IOrderedEnumerable<T> ?? throw new InvalidOperationException()).ToArray();
        for (var i = 0; i < sorted.Length; i++)
        {
            collection.Move(collection.IndexOf(sorted[i]), i);
        }
    }
}