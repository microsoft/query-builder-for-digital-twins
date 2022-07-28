// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace QueryBuilder.Test.Generated;

using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Azure.DigitalTwins.Core;

/// <summary>
/// Extends models to add common functionality.
/// </summary>
public static class ModelHelper
{
    /// <summary>
    /// Applies a property selector expression to the given twin to extract the property's JSON name and the Type of the property owner.
    /// </summary>
    /// <typeparam name="T">The twin type.</typeparam>
    /// <param name="propertySelector">A functional expression that specifies a property.</param>
    /// <param name="modelType">The type of the twin if the property is on the twin, or the type of relationship if the property is on a relationship.</param>
    /// <returns>The JSON property name of the property as defined on the model.</returns>
    public static string? SelectJsonProperty<T>(Expression<Func<T, object>> propertySelector, out Type? modelType)
    where T : BasicDigitalTwin
    {
        var member = propertySelector.Body as MemberExpression;
        if (propertySelector.Body is UnaryExpression unary) // for primitive-type properties that require conversion to object
        {
            member = unary.Operand as MemberExpression;
        }

        var propInfo = member?.Member as PropertyInfo;
        modelType = member?.Expression?.Type;

        if (propInfo == null)
        {
            throw new ArgumentException($"Expression '{propertySelector}' does not refer to a property.");
        }

        return propInfo.GetPropertyAttributeValue<JsonPropertyNameAttribute, string>(attr => attr.Name);
    }

    /// <summary>
    /// Gets the JSON name for the property selected from the invoked twin.
    /// </summary>
    /// <typeparam name="T">Type of the twin.</typeparam>
    /// <param name="twin">The twin to extract the property from.</param>
    /// <param name="propertySelector">A functional expression to select the property.</param>
    /// <returns>The JSON name of the property.</returns>
    public static string? GetPropertyJsonName<T>(this T twin, Expression<Func<T, object>> propertySelector)
    where T : BasicDigitalTwin
    {
        return SelectJsonProperty(propertySelector, out Type _);
    }

    /// <summary>
    /// Gets the enum value for the given Enum.
    /// </summary>
    /// <typeparam name="T?">Type of the Enum.</typeparam>
    /// <param name="customEnum">The enum to extract the enum value from.</param>
    /// <returns>The enum value of the Enum.</returns>
    public static string? GetEnumValue<T>(this T? customEnum)
    where T : struct, Enum
    {
        return GetEnumCustomAttribute<T, EnumMemberAttribute>(customEnum)?.Value;
    }

    /// <summary>
    /// Gets the enum value for the given Enum.
    /// </summary>
    /// <typeparam name="T">Type of the Enum.</typeparam>
    /// <param name="customEnum">The enum to extract the enum value from.</param>
    /// <returns>The enum value of the Enum.</returns>
    public static string? GetEnumValue<T>(this T customEnum)
    where T : struct, Enum
    {
        return GetEnumCustomAttribute<T, EnumMemberAttribute>(customEnum)?.Value;
    }

    /// <summary>
    /// Gets the source value for the given Enum.
    /// </summary>
    /// <typeparam name="T?">Type of the Enum.</typeparam>
    /// <param name="customEnum">The enum to extract the source value from.</param>
    /// <returns>The source value of the Enum.</returns>
    public static string? GetSourceValue<T>(this T? customEnum)
    where T : struct, Enum
    {
        return GetEnumCustomAttribute<T, SourceValueAttribute>(customEnum)?.Value;
    }

    /// <summary>
    /// Gets the source value for the given Enum.
    /// </summary>
    /// <typeparam name="T">Type of the Enum.</typeparam>
    /// <param name="customEnum">The enum to extract the source value from.</param>
    /// <returns>The source value of the Enum.</returns>
    public static string? GetSourceValue<T>(this T customEnum)
    where T : struct, Enum
    {
        return GetEnumCustomAttribute<T, SourceValueAttribute>(customEnum)?.Value;
    }

    /// <summary>
    /// Gets the display attribute for the given Enum.
    /// </summary>
    /// <typeparam name="T?">Type of the Enum.</typeparam>
    /// <param name="customEnum">The enum to extract the display attribute from.</param>
    /// <returns>The display attribute of the Enum.</returns>
    public static DisplayAttribute? GetDisplayAttribute<T>(this T? customEnum)
    where T : struct, Enum
    {
        return GetEnumCustomAttribute<T, DisplayAttribute>(customEnum);
    }

    /// <summary>
    /// Gets the display attribute for the given Enum.
    /// </summary>
    /// <typeparam name="T">Type of the Enum.</typeparam>
    /// <param name="customEnum">The enum to extract the display attribute from.</param>
    /// <returns>The display attribute of the Enum.</returns>
    public static DisplayAttribute? GetDisplayAttribute<T>(this T customEnum)
        where T : struct, Enum
    {
        return GetEnumCustomAttribute<T, DisplayAttribute>(customEnum);
    }

    private static TAttribute? GetEnumCustomAttribute<TEnum, TAttribute>(TEnum? customEnum)
    where TEnum : struct, Enum
    where TAttribute : Attribute
    {
        if (customEnum is null || !customEnum.HasValue)
        {
            return null;
        }

        var enumType = customEnum.GetType();
        var enumName = enumType.GetEnumName(customEnum);
        if (enumName is null)
        {
            return null;
        }

        return enumType.GetField(enumName)?.GetCustomAttribute<TAttribute>();
    }

    private static TValue? GetPropertyAttributeValue<TAttribute, TValue>(this PropertyInfo propertyInfo, Func<TAttribute, TValue> valueSelector)
    where TAttribute : Attribute
    {
        var attr = propertyInfo.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() as TAttribute;
        return attr != null ? valueSelector(attr) : default;
    }
}