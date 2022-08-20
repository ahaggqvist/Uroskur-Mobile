namespace Uroskur.Utils;

public static class Extensions
{
    public static IEnumerable<Tuple<T, T, T>> GetItems<T>(this IEnumerable<T>? source)
    {
        if (source == null)
        {
            yield break;
        }

        var skip = true;
        T? previous = default, current = default;
        T? next;
        foreach (var item in source)
        {
            next = item;
            if (!skip && previous != null && current != null)
            {
                yield return new Tuple<T, T, T>(previous, current, next);
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
            yield return new Tuple<T, T, T>(previous, current, next);
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

    public static async Task<(T1, T2)> WhenAll<T1, T2>(Task<T1> task1, Task<T2> task2)
    {
        await Task.WhenAll(task1, task2);
        return (task1.Result, task2.Result);
    }

    public static async Task<(T1, T2, T3)> WhenAll<T1, T2, T3>(Task<T1> task1, Task<T2> task2, Task<T3> task3)
    {
        await Task.WhenAll(task1, task2, task3);
        return (task1.Result, task2.Result, task3.Result);
    }

    public static TaskAwaiter<(T1, T2)> GetAwaiter<T1, T2>(this ValueTuple<Task<T1>, Task<T2>> tasks)
    {
        return WhenAll(tasks.Item1, tasks.Item2).GetAwaiter();
    }

    public static TaskAwaiter<(T1, T2, T3)> GetAwaiter<T1, T2, T3>(this ValueTuple<Task<T1>, Task<T2>, Task<T3>> tasks)
    {
        return WhenAll(tasks.Item1, tasks.Item2, tasks.Item3).GetAwaiter();
    }
}