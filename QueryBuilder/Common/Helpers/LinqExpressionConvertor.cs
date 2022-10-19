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
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Typed;

    internal class LinqExpressionConverter<T> where T : BasicDigitalTwin
    {
        public string PropertyName { get; private set; }

        public object Value { get; private set; }

        public ComparisonOperators ComparisonOperator { get; private set; }

        public ScalarOperator ScalarOperator { get; private set; }

        public bool IsScalar => ScalarOperator != null;

        private bool isLogical;

        private Type propertyType = null;
        private Expression leftLogicalExpression = null;
        private Expression rightLogicalExpression = null;
        private ExpressionType logicalExpressionType;
        private const string DTID = "dtId";
        private const string ADTDTID = "$dtId";
        private readonly ExpressionType[] LogicalExpressionType = new ExpressionType[] { ExpressionType.AndAlso, ExpressionType.OrElse };

        /// <summary>
        /// Initializes a new instance of the <see cref="LinqExpressionConverter{T}" /> class.
        /// </summary>
        /// <param name="expression">
        /// The expression to extract the PropertyName, Operator, and Value from.
        /// If a property has a <see cref="JsonPropertyNameAttribute"/> it will use the value
        /// provided to that attribute, otherwise, it will use the property name converted to
        /// camelCase.
        /// </param>
        public LinqExpressionConverter(Expression<Func<T, bool>> expression)
        {
            ExtractExpressionComponents(expression.Body);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinqExpressionConverter{T}" /> class.
        /// </summary>
        /// <param name="expression">
        /// The expression to parse
        /// If a property has a <see cref="JsonPropertyNameAttribute"/> it will use the value
        /// provided to that attribute, otherwise, it will use the property name converted to
        /// camelCase.
        /// </param>
        public LinqExpressionConverter(Expression expression)
        {
            ExtractExpressionComponents(expression);
        }

        /// <summary>
        /// Extract expression components. In the case of the binary, method call, or type
        /// binary expressions, components are PropertyName, Operator, and value are extracted.
        /// In the case of logical expressions, legical expression type and sided are extracted.
        /// </summary>
        /// <param name="expression">
        /// The expression to extract the PropertyName, Operator, and Value from.
        /// If a property has a <see cref="JsonPropertyNameAttribute"/> it will use the value
        /// provided to that attribute, otherwise, it will use the property name converted to
        /// camelCase.
        /// </param>
        private void ExtractExpressionComponents(Expression expression)
        {
            if (expression is BinaryExpression binaryExpression)
            {
                if (LogicalExpressionType.Contains(binaryExpression.NodeType))
                {
                    isLogical = true;
                    logicalExpressionType = binaryExpression.NodeType;
                    leftLogicalExpression = binaryExpression.Left;
                    rightLogicalExpression = binaryExpression.Right;
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
                throw new Exception($"Failed to establish a PropertyName from the left or right side of the Operand. ");
            }

            if (IsScalar && ScalarOperator is ScalarBinaryOperator && Value is null)
            {
                throw new Exception($"Failed to establish a Value from the left or right side of the Operand. ");
            }
        }

        /// <summary>
        /// Adds filters to an existing <see cref="FilteredQuery{TQuery}" /> query using the extracted
        /// parts from the expression given to this class.
        /// </summary>
        /// <param name="query">The query to add the filters to.</param>
        /// <typeparam name="TQuery">The type of query to add the filters to.</typeparam>
        /// <returns>The original query passed in with the additional filters added to it.</returns>
        public TQuery AddToQuery<TQuery>(TQuery query) where TQuery : FilteredQuery<TQuery>
        {
            if (isLogical)
            {
                return AddLogicalExpressionToQuery(query);
            }

            return AddSimpleExpressionToQuery(query);
        }

        /// <summary>
        /// Adds filters to an existing <see cref="FilteredQuery{TQuery}" /> query using the extracted
        /// parts from the simple expression, with no and/or, given to this class.
        /// </summary>
        /// <param name="query">The query to add the filters to.</param>
        /// <typeparam name="TQuery">The type of query to add the filters to.</typeparam>
        /// <returns>The original query passed in with the additional filters added to it.</returns>
        private TQuery AddSimpleExpressionToQuery<TQuery>(TQuery query) where TQuery : FilteredQuery<TQuery>
        {
            if (!IsScalar)
            {
                return query.Where<T>(PropertyName, ComparisonOperator, Value);
            }

            if (ScalarOperator is ScalarUnaryOperator unaryOperator)
            {
                if (unaryOperator == ScalarOperators.IS_NULL && ComparisonOperator == ComparisonOperators.NotEqualTo)
                {
                    return query.Not(q => q.Where<T>(PropertyName, unaryOperator));
                }

                return query.Where<T>(PropertyName, unaryOperator);
            }

            // Scalar binary operator
            return ScalarOperator.Name switch
            {
                // Invariant: Scalar binary operator can only be 1 of the 3 following methods
                var _ when ScalarOperator.Name == ScalarOperators.STARTSWITH.Name => query.WhereStartsWith<T>(PropertyName, (string)Value),
                var _ when ScalarOperator.Name == ScalarOperators.ENDSWITH.Name => query.WhereEndsWith<T>(PropertyName, (string)Value),
                _ => query.WhereContains<T>(PropertyName, (string)Value),
            };
        }

        /// <summary>
        /// Adds filters to an existing <see cref="FilteredQuery{TQuery}" /> query using the extracted
        /// parts from the expression given to this class. Andded and Ored expressions are recursively
        /// added in a bfs fashion down the logical expression tree.
        /// </summary>
        /// <param name="query">The query to add the filters to.</param>
        /// <typeparam name="TQuery">The type of query to add the filters to.</typeparam>
        /// <returns>The original query passed in with the additional filters added to it.</returns>
        private TQuery AddLogicalExpressionToQuery<TQuery>(TQuery query) where TQuery : FilteredQuery<TQuery>
        {
            var leftLogicalExpressionConvertor = new LinqExpressionConverter<T>(leftLogicalExpression);
            var rightLogicalExpressionConvertor = new LinqExpressionConverter<T>(rightLogicalExpression);

            // left is logical, right is simple
            if (LogicalExpressionType.Contains(leftLogicalExpression.NodeType) && !LogicalExpressionType.Contains(rightLogicalExpression.NodeType))
            {
                return AddLogicalExpressionTypeToQuery(query, leftLogicalExpressionConvertor.AddLogicalExpressionToQuery, rightLogicalExpressionConvertor.AddToQuery);
            }

            // right is logical, left is simple
            if (LogicalExpressionType.Contains(rightLogicalExpression.NodeType) && !LogicalExpressionType.Contains(leftLogicalExpression.NodeType))
            {
                return AddLogicalExpressionTypeToQuery(query, rightLogicalExpressionConvertor.AddLogicalExpressionToQuery, leftLogicalExpressionConvertor.AddToQuery);
            }

            // both are logical
            if (LogicalExpressionType.Contains(leftLogicalExpression.NodeType) && LogicalExpressionType.Contains(rightLogicalExpression.NodeType))
            {
                return AddLogicalExpressionTypeToQuery(query, rightLogicalExpressionConvertor.AddLogicalExpressionToQuery, leftLogicalExpressionConvertor.AddLogicalExpressionToQuery);
            }

            return AddLogicalExpressionTypeToQuery(query, rightLogicalExpressionConvertor.AddToQuery, leftLogicalExpressionConvertor.AddToQuery);
        }

        private TQuery AddLogicalExpressionTypeToQuery<TQuery>(TQuery query, Func<TQuery, TQuery> outerFunction, Func<TQuery, TQuery> innerFunction) where TQuery : FilteredQuery<TQuery>
        {
            if (logicalExpressionType == ExpressionType.AndAlso)
            {
                return query.And(query => outerFunction(innerFunction((TQuery)query)));
            }

            // Invariant: logicalExpressionType is either AndAlso or OrElse.
            return query.Or(query => outerFunction(innerFunction((TQuery)query)));
        }

        private void HandleTypeBinaryExpression(TypeBinaryExpression expression)
        {
            ScalarOperator = GetScalarOperatorForType(expression.TypeOperand.Name);
            Handle(expression.Expression);
        }

        /// <summary>
        /// This method handles binary operation expressions, such as ==, !=, etc. It extracts the property and value from either the left or right hand side of the operand respectively, and the binary operator.
        /// </summary>
        /// <param name="expression">An expression representing a binary comparison.</param>
        private void HandleBinaryExpression(BinaryExpression expression)
        {
            ComparisonOperator = GetComparisonOperator(expression);
            Handle(expression.Left);
            Handle(expression.Right);
        }

        /// <summary>
        /// This method handles conversion of methods like: stringProperty.StartsWith("someValue").
        /// </summary>
        /// <param name="expression">An expression that is a method call type - containing the object the method was called on, the method that was called and the arguments passed into that method.</param>
        private void HandleInstanceMethodCallExpression(MethodCallExpression expression)
        {
            Handle(expression.Arguments.FirstOrDefault());
            if (expression.Object != null)
            {
                Handle(expression.Object);
            }

            ScalarOperator = GetScalarOperatorForMethod(expression.Method.Name);
        }

        /// <summary>
        /// This method handles multiple levels of the evaluation to extract the PropertyName and Value from the given expression.
        /// </summary>
        /// <param name="expression">The expression to evaluate, either a MemberExpression (usually containing the PropertyName) or a ConstantExpression (usually containing the Value).</param>
        private void Handle(Expression expression)
        {
            if (expression is MemberExpression memberExpression)
            {
                HandleMemberExpression(memberExpression);
            }

            HandleConstantExpression(expression);
        }

        /// <summary>
        /// This method handles extracting the value from a ConstantExpression. It handles recursive calls to itself to handle nested Expressions and
        /// various differences in operations such as (b => b.Name == "someValue") vs (b => b.Name == someAssignedVariableString).
        /// </summary>
        /// <param name="expression">The Expression to evaluate that should be of type ConstantExpression, otherwise, it will simply ignore evaluation.</param>
        /// <param name="fieldInfo">A fieldInfo object recursively passed in depending on whether a variable or constant value was used in the expression.</param>
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

        /// <summary>
        /// Checks wether the object is of type DisplayClass, which is a runtime generated class containing properties that are tracked within the Expression context chain.
        /// </summary>
        /// <param name="o">The object to evaluate if it is DisplayClass object.</param>
        private static bool IsDisplayClass(object o)
        {
            if (o is null)
            {
                return false;
            }

            return o.GetType().Name.Contains("DisplayClass");
        }

        /// <summary>
        /// Gets a property name from a MemberExpression.
        /// If a property has a <see cref="JsonPropertyNameAttribute"/> it will use the value provided to that attribute, otherwise, it will use the property name converted to camelCase.
        /// </summary>
        /// <param name="expression">The MemberExpression to evaluate.</param>
        /// <returns>The string property name in camelCase/json form.</returns>
        private string GetPropertyName(MemberExpression expression)
        {
            var propInfo = expression.Member as PropertyInfo;
            var attribute = propInfo.GetPropertyAttributeValueL<JsonPropertyNameAttribute, string>(attr => attr.Name);
            var propName = string.IsNullOrEmpty(attribute)
                    ? string.Equals(propInfo.Name, DTID, StringComparison.OrdinalIgnoreCase) ? ADTDTID : propInfo.Name.ToLowerFirstChar()
                    : attribute;

            // multi level property
            if (expression.Expression is MemberExpression me2)
            {
                var parentPropInfo = me2.Member as PropertyInfo;
                var parentAttribute = parentPropInfo.GetPropertyAttributeValueL<JsonPropertyNameAttribute, string>(attr => attr.Name);
                var parentPropName = string.IsNullOrEmpty(parentAttribute)
                        ? string.Equals(propInfo.Name, DTID, StringComparison.OrdinalIgnoreCase) ? ADTDTID : propInfo.Name.ToLowerFirstChar()
                        : parentAttribute;
                propertyType = parentPropInfo.PropertyType;
                return $"{parentPropName}.{propName}";
            }

            // single level property
            propertyType = propInfo.PropertyType;
            return propName;
        }

        private void ConvertIntToEnumIfNeeded()
        {
            if (IsEnumDeepCheck(propertyType, Value, out var enumType))
            {
                Value = Enum.GetName(enumType, Value);
            }
        }

        /// <summary>
        /// This does a more thorough check to see if a property type is an Enum. The reason this is needed
        /// is because Enums that are nullable are actually created as a ValueType, and thus the actual Enum is moved
        /// to a GenericTypeArgument of the ValueType.
        /// </summary>
        /// <param name="type">The initial type to check.</param>
        /// <param name="value">The value we're trying to check is an Enum or not.</param>
        /// <param name="enumType">The actual enum type extracted from the deep check process.</param>
        /// <returns>True or false if the deep check determine the type is an Enum.</returns>
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
                _ => throw new Exception($"Linq ExpressionType '{expression.NodeType}' is not supported by ADT.")
            };
        }

        private static ScalarOperator GetScalarOperatorForMethod(string methodName)
        {
            return methodName switch
            {
                "StartsWith" => ScalarOperators.STARTSWITH,
                "Contains" => ScalarOperators.CONTAINS,
                "EndsWith" => ScalarOperators.ENDSWITH,
                _ => throw new Exception($"Method '{methodName}' is not supported for conversion into a ScalarBinaryOperator")
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
                _ => throw new Exception($"Type '{typeName}' is not supported for conversion into a ScalarUnaryOperator")
            };
        }
    }

    internal static class ConverterHelper
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