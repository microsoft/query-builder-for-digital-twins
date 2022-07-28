// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace QueryBuilder.Test.Generated;

using System.Collections.Generic;

internal static class Extensions
{
    internal static int CustomHash(this object o, params int?[] values)
    {
        var hash = 1009;
        foreach (var i in values)
        {
            if (i != null && i.HasValue)
            {
                hash = (hash * 9176) + i.Value;
            }
        }

        return hash;
    }

    /// <summary>
    /// Checks that two dictionaries have the same key-value pairs.
    /// </summary>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <param name="left">The first dictionary.</param>
    /// <param name="right">The second dictionary.</param>
    /// <param name="valueComparer">An equality comparer for the values in the dictionary.</param>
    /// <returns>True if both dictionaries are equal; false otherwise.</returns>
    internal static bool DictionaryEquals<TKey, TValue>(this IDictionary<TKey, TValue> left, IDictionary<TKey, TValue> right, EqualityComparer<TValue>? valueComparer = null)
    {
        valueComparer ??= EqualityComparer<TValue>.Default;
        if (left.Count != right.Count)
        {
            return false;
        }

        foreach (var kvp in left)
        {
            // Dicts not equal if the right does not contain a left key, or the values are not equal for a shared key.
            if (!right.TryGetValue(kvp.Key, out var rightValue) || !valueComparer.Equals(rightValue, kvp.Value))
            {
                return false;
            }
        }

        return true;
    }
}