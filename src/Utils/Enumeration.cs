﻿using System.Diagnostics.CodeAnalysis;

namespace Uroskur.Utils;

public abstract class Enumeration : IEqualityComparer<Enumeration>
{
    protected Enumeration()
    {
    }

    protected Enumeration(int id, string name)
    {
        (Id, Name) = (id, name);
    }

    public string Name { get; } = string.Empty;

    public int Id { get; }

    public int GetHashCode([DisallowNull] Enumeration obj)
    {
        return HashCode.Combine(Id, Name);
    }

    public bool Equals(Enumeration? x, Enumeration? y)
    {
        return y != null && x != null && x.Id == y.Id && x.Name == y.Name;
    }

    public static T FromId<T>(int id) where T : Enumeration, new()
    {
        var matchingItem = Parse<T, int>(id, "id", item => item.Id == id);
        return matchingItem;
    }

    public static T FromName<T>(string name) where T : Enumeration, new()
    {
        var matchingItem = Parse<T, string>(name, "name", item => item.Name == name);
        return matchingItem;
    }

    public override string ToString()
    {
        return Name;
    }

    public static IEnumerable<T> GetAll<T>() where T : Enumeration
    {
        return typeof(T).GetFields(BindingFlags.Public |
                                   BindingFlags.Static |
                                   BindingFlags.DeclaredOnly)
            .Select(f => f.GetValue(null))
            .Cast<T>();
    }

    private static T Parse<T, TK>(TK value, string description, Func<T, bool> predicate) where T : Enumeration, new()
    {
        var matchingItem = GetAll<T>().FirstOrDefault(predicate);

        if (matchingItem != null)
        {
            return matchingItem;
        }

        var message = $"'{value}' is not a valid {description} in {typeof(T)}";
        throw new ArgumentException(message);
    }
}