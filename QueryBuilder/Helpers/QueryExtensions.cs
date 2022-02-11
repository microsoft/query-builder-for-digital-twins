// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.Azure.DigitalTwins.QueryBuilder.Helpers
{
    using System;
    using System.Linq;

    internal static class QueryExtensions
    {
        internal static string WrapQuotes(this object value)
        {
            var type = value.GetType();
            if (!type.IsPrimitive && type != typeof(decimal) && type != typeof(string))
            {
                throw new InvalidOperationException($"Complex Type of '{type}' cannot be used for comparision in where clause.");
            }

            return value switch
            {
                string _ => $"'{value.ToString().EscapeValue()}'",
                _ => $"{value}",
            };
        }

        internal static string WrapArrayQuotes(this string[] values)
        {
            return string.Join(",", values.Select(WrapQuotes));
        }

        internal static string EscapeValue(this string value)
        {
            // Escape value = some'quoted => some\'quoted
            return value.Replace("'", @"\'");
        }
    }
}