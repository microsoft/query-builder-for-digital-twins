// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Helpers
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Text.Json.Serialization;
    using global::Azure.DigitalTwins.Core;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder;

    internal class LinqExpressionParser<T> where T : BasicDigitalTwin
    {
        internal string PropertyName { get; private set; }

        internal object Value { get; private set; }

        internal ComparisonOperators ComparisonOperator { get; private set; }

        internal ScalarOperator ScalarOperator { get; private set; }

        internal bool IsLogical { get; private set; }

        internal Type PropertyType { get; private set; }

        internal Expression LeftLogicalExpression { get; private set; }

        internal Expression RightLogicalExpression { get; private set; }

        internal ExpressionType LogicalExpressionType { get; private set; }

        internal readonly ExpressionType[] LogicalExpressionTypes = new ExpressionType[] { ExpressionType.AndAlso, ExpressionType.OrElse };

        internal bool IsScalar => ScalarOperator != null;

        private const string DTID = "dtId";
        private const string ADTDTID = "$dtId";

        /// <summary>
        /// Initializes a new instance of the <see cref="LinqExpressionParser{T}" /> class.
        /// </summary>
        /// <param name="expression">
        /// The expression to extract the PropertyName, Operator, and Value from.
        /// If a property has a <see cref="JsonPropertyNameAttribute"/> it will use the value
        /// provided to that attribute, otherwise, it will use the property name parsed to
        /// camelCase.
        /// </param>
        public LinqExpressionParser(Expression<Func<T, bool>> expression)
        {
            ExtractExpressionComponents(expression.Body);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinqExpressionParser{T}" /> class.
        /// </summary>
        /// <param name="expression">
        /// The expression to parse
        /// If a property has a <see cref="JsonPropertyNameAttribute"/> it will use the value
        /// provided to that attribute, otherwise, it will use the property name parsed to
        /// camelCase.
        /// </param>
        public LinqExpressionParser(Expression expression)
        {
            ExtractExpressionComponents(expression);
        }

        private void ExtractExpressionComponents(Expression expression)
        {
            if (expression is BinaryExpression binaryExpression)
            {
                if (LogicalExpressionTypes.Contains(binaryExpression.NodeType))
                {
                    IsLogical = true;
                    LogicalExpressionType = binaryExpression.NodeType;
                    LeftLogicalExpression = binaryExpression.Left;
                    RightLogicalExpression = binaryExpression.Right;
                    return;
                }

                HandleBinaryExpression(binaryExpression);
            }

            if (expression is MethodCallExpression mec)
            {
                HandleInstanceMethodCallExpression(mec);
            }

            if (expression is TypeBinaryExpression typeBinaryExpression)
            {
                HandleTypeBinaryExpression(typeBinaryExpression);
            }

            ConvertIntToEnumIfNeeded();

            if (string.IsNullOrWhiteSpace(PropertyName))
            {
                throw new LinqExpressionNotSupportedException($"Failed to establish a PropertyName from the left or right side of the Operand. ");
            }

            if (IsScalar && ScalarOperator is ScalarBinaryOperator && Value is null)
            {
                throw new LinqExpressionNotSupportedException($"Failed to establish a Value from the left or right side of the Operand. ");
            }
        }

        private void HandleTypeBinaryExpression(TypeBinaryExpression expression)
        {
            ScalarOperator = GetScalarOperatorForType(expression.TypeOperand.Name);
            Handle(expression.Expression);
        }

        private void HandleBinaryExpression(BinaryExpression expression)
        {
            ComparisonOperator = GetComparisonOperator(expression);
            Handle(expression.Left);
            Handle(expression.Right);
        }

        private void HandleInstanceMethodCallExpression(MethodCallExpression expression)
        {
            Handle(expression.Arguments.FirstOrDefault());
            if (expression.Object != null)
            {
                Handle(expression.Object);
            }

            ScalarOperator = GetScalarOperatorForMethod(expression.Method.Name);
        }

        private void Handle(Expression expression)
        {
            if (expression is MemberExpression memberExpression)
            {
                HandleMemberExpression(memberExpression);
            }

            HandleConstantExpression(expression);
        }

        private void HandleConstantExpression(Expression expression, FieldInfo fieldInfo = null)
        {
            if (expression is ConstantExpression constantExpression)
            {
                if (IsDisplayClass(constantExpression.Value))
                {
                    Value = fieldInfo.GetValue(constantExpression.Value);
                    return;
                }

                Value = constantExpression.Value;
                if (Value == null)
                {
                    ScalarOperator = ScalarOperators.IS_NULL;
                }
            }

            if (expression is UnaryExpression unaryExpression)
            {
                HandleConstantExpression(unaryExpression.Operand);
            }

            if (expression is MemberExpression memberExpression)
            {
                HandleMemberExpression(memberExpression);
            }
        }

        private void HandleMemberExpression(MemberExpression expression)
        {
            if (expression.Member is PropertyInfo propInfo)
            {
                if (expression.Expression is MemberExpression me2 && me2.Member is FieldInfo fi && me2.Expression is ConstantExpression ce && IsDisplayClass(ce.Value))
                {
                    Value = propInfo.GetValue(fi.GetValue(ce.Value));
                }
                else if (string.IsNullOrWhiteSpace(PropertyName))
                {
                    PropertyName = GetPropertyName(expression);
                }
            }

            if (expression.Member is FieldInfo fieldInfo)
            {
                HandleConstantExpression(expression.Expression, fieldInfo);
            }
        }

        private static bool IsDisplayClass(object o)
        {
            if (o is null)
            {
                return false;
            }

            return o.GetType().Name.Contains("DisplayClass");
        }

        private string GetPropertyName(MemberExpression expression)
        {
            var propInfo = expression.Member as PropertyInfo;
            var propName = GetPropertyNameFromPropertyInfo(propInfo);

            // multi level property
            if (expression.Expression is MemberExpression me2)
            {
                var parentPropInfo = me2.Member as PropertyInfo;
                var parentPropName = GetPropertyNameFromPropertyInfo(parentPropInfo);
                PropertyType = parentPropInfo.PropertyType;
                return $"{parentPropName}.{propName}";
            }

            // single level property
            PropertyType = propInfo.PropertyType;
            return propName;
        }

        private string GetPropertyNameFromPropertyInfo(PropertyInfo propInfo)
        {
            var propAttribute = propInfo.GetPropertyAttributeValueL<JsonPropertyNameAttribute, string>(attr => attr.Name);
            return string.IsNullOrEmpty(propAttribute)
                ? string.Equals(propInfo.Name, DTID, StringComparison.OrdinalIgnoreCase) ? ADTDTID : propInfo.Name.ToLowerFirstChar()
                : propAttribute;
        }

        private void ConvertIntToEnumIfNeeded()
        {
            if (IsEnumDeepCheck(PropertyType, Value, out var enumType))
            {
                Value = Enum.GetName(enumType, Value);
            }
        }

        private static bool IsEnumDeepCheck(Type type, object value, out Type enumType)
        {
            enumType = null;
            if (type == null || type.BaseType == null)
            {
                return false;
            }

            var underlyingType = Nullable.GetUnderlyingType(type);
            if (underlyingType?.BaseType == typeof(Enum) && Enum.IsDefined(underlyingType, value))
            {
                enumType = underlyingType;
                return true;
            }

            return false;
        }

        private static ComparisonOperators GetComparisonOperator(Expression expression)
        {
            return expression.NodeType switch
            {
                ExpressionType.Equal => ComparisonOperators.IsEqualTo,
                ExpressionType.GreaterThan => ComparisonOperators.IsGreaterThan,
                ExpressionType.GreaterThanOrEqual => ComparisonOperators.IsGreaterThanOrEqualTo,
                ExpressionType.LessThan => ComparisonOperators.IsLessThan,
                ExpressionType.LessThanOrEqual => ComparisonOperators.IsLessThanOrEqualTo,
                ExpressionType.NotEqual => ComparisonOperators.NotEqualTo,
                _ => throw new LinqOperatorNotSupportedException($"Linq ExpressionType '{expression.NodeType}' is not supported by ADT.")
            };
        }

        private static ScalarOperator GetScalarOperatorForMethod(string methodName)
        {
            return methodName switch
            {
                "StartsWith" => ScalarOperators.STARTSWITH,
                "Contains" => ScalarOperators.CONTAINS,
                "EndsWith" => ScalarOperators.ENDSWITH,
                _ => throw new LinqOperatorNotSupportedException($"Method '{methodName}' is not a supported string function by ADT")
            };
        }

        private static ScalarUnaryOperator GetScalarOperatorForType(string typeName)
        {
            return typeName switch
            {
                "String" => ScalarOperators.IS_STRING,
                "Boolean" => ScalarOperators.IS_BOOL,
                "Double" => ScalarOperators.IS_NUMBER,
                "Int16" => ScalarOperators.IS_NUMBER,
                "Int32" => ScalarOperators.IS_NUMBER,
                "Int64" => ScalarOperators.IS_NUMBER,
                "Single" => ScalarOperators.IS_NUMBER,
                "Object" => ScalarOperators.IS_OBJECT,
                _ => throw new LinqOperatorNotSupportedException($"Type '{typeName}' has no supported type checking function by ADT")
            };
        }
    }

    internal static class ParserHelper
    {
        internal static string ToLowerFirstChar(this string input)
        {
            string newString = input;
            if (!string.IsNullOrEmpty(newString) && char.IsUpper(newString[0]))
            {
                newString = char.ToLower(newString[0]) + newString.Substring(1);
            }

            return newString;
        }

        internal static TValue GetPropertyAttributeValueL<TAttribute, TValue>(
                    this PropertyInfo propertyInfo,
                    Func<TAttribute, TValue> valueSelector)
        where TAttribute : Attribute
        {
            var attr = propertyInfo.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() as TAttribute;
            return attr != null ? valueSelector(attr) : default;
        }
    }
}