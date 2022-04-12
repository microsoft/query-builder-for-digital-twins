// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Text.Json.Serialization;
    using global::Azure.DigitalTwins.Core;

    internal static class QueryValidator
    {
        internal static void ExtractModelAndPropertyName<TModel>(Expression<Func<TModel, object>> propertySelector, out Type type, out string propertyName) where TModel : BasicDigitalTwin
        {
            var member = propertySelector.Body as MemberExpression;
            if (propertySelector.Body is UnaryExpression unary) // for primitive-type properties that require conversion to object
            {
                member = unary.Operand as MemberExpression;
            }

            var propInfo = member?.Member as PropertyInfo;
            type = member?.Expression.Type;

            if (propInfo == null)
            {
                throw new ArgumentException($"Expression '{propertySelector}' does not refer to a property.");
            }

            propertyName = propInfo.GetPropertyAttributeValue<JsonPropertyNameAttribute, string>(attr => attr.Name);
            if (propertyName is null)
            {
                throw new NoJsonPropertyException(propertySelector);
            }
        }

        internal static void ValidateAlias(string alias, IDictionary<string, Type> aliasToTypeMapping)
        {
            if (!string.IsNullOrEmpty(alias) && !aliasToTypeMapping.Keys.Contains(alias))
            {
                throw new ArgumentException($"Alias '{alias}' is not assigned!");
            }
        }

        internal static void ValidateAlias(string alias, IEnumerable<string> aliases)
        {
            if (string.IsNullOrWhiteSpace(alias))
            {
                throw new ArgumentNullException(nameof(alias));
            }

            if (!string.IsNullOrWhiteSpace(alias) && !aliases.Contains(alias))
            {
                throw new ArgumentException($"Alias '{alias}' is not assigned!");
            }
        }

        internal static void ValidateType<TModel>(ISet<Type> allowedType)
        {
            if (!allowedType.Contains(typeof(TModel)))
            {
                throw new ArgumentException($"Typed where clause on Type {typeof(TModel).Name} is only allowed on types used in From<T> or Join<T,T>.");
            }
        }

        private static TValue GetPropertyAttributeValue<TAttribute, TValue>(this PropertyInfo propertyInfo, Func<TAttribute, TValue> valueSelector)
            where TAttribute : Attribute
        {
            var attr = propertyInfo.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() as TAttribute;
            return attr != null ? valueSelector(attr) : default;
        }
    }
}