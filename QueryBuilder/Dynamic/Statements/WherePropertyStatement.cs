// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Dynamic.Statements
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.Serialization;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Clauses;

    /// <summary>
    /// A statement class that contains various unary or binary comparison methods to finalize the WHERE statement.
    /// </summary>
    /// <typeparam name="TWhereStatement">The type of WHERE statement supported.</typeparam>
    public class WherePropertyStatement<TWhereStatement> where TWhereStatement : WhereBaseStatement<TWhereStatement>
    {
        private readonly string propertyName;
        private readonly string alias;
        private readonly WhereClause clause;
        private readonly IList<JoinClause> joinClauses;

        internal WherePropertyStatement(IList<JoinClause> joinClauses, WhereClause clause, string propertyName, string alias)
        {
            this.joinClauses = joinClauses;
            this.propertyName = propertyName;
            this.clause = clause;
            this.alias = alias;
        }

        /// <summary>
        /// Adds an equals (=) comparison to the condition in the WHERE statement.
        /// </summary>
        /// <param name="value">The value to compare against.</param>
        /// <returns>A conjunction class that supports appending more conditions to the WHERE statements via OR or AND terms.</returns>
        public CompoundWhereStatement<TWhereStatement> IsEqualTo(object value) => CreateAndAddWhereComparisonCondition(ComparisonOperators.IsEqualTo, value);

        /// <summary>
        /// Adds an IN comparison to the condition in the WHERE statement to check if a property's value is in a given set of values.
        /// </summary>
        /// <param name="values">The value to compare against.</param>
        /// <returns>A conjunction class that supports appending more conditions to the WHERE statements via OR or AND terms.</returns>
        public CompoundWhereStatement<TWhereStatement> IsIn(string[] values)
        {
            var condition = new WhereInCondition
            {
                Column = propertyName,
                Value = values,
                Alias = alias
            };

            clause.AddCondition(condition);
            return new CompoundWhereStatement<TWhereStatement>(joinClauses, clause, alias);
        }

        /// <summary>
        /// Adds a NIN comparison to the condition in the WHERE statement to check if a property's value is not in a given set of values.
        /// </summary>
        /// <param name="values">The value to compare against.</param>
        /// <returns>A conjunction class that supports appending more conditions to the WHERE statements via OR or AND terms.</returns>
        public CompoundWhereStatement<TWhereStatement> IsNotIn(string[] values)
        {
            var condition = new WhereNotInCondition
            {
                Column = propertyName,
                Value = values,
                Alias = alias
            };

            clause.AddCondition(condition);
            return new CompoundWhereStatement<TWhereStatement>(joinClauses, clause, alias);
        }

        /// <summary>
        /// Adds a greater than (>) comparison to the condition in the WHERE statement.
        /// </summary>
        /// <param name="value">The value to compare against.</param>
        /// <returns>A conjunction class that supports appending more conditions to the WHERE statements via OR or AND terms.</returns>
        public CompoundWhereStatement<TWhereStatement> IsGreaterThan(int value) => CreateAndAddWhereComparisonCondition(ComparisonOperators.IsGreaterThan, value);

        /// <summary>
        /// Adds a greater than ore equal to (>=) comparison to the condition in the WHERE statement.
        /// </summary>
        /// <param name="value">The value to compare against.</param>
        /// <returns>A conjunction class that supports appending more conditions to the WHERE statements via OR or AND terms.</returns>
        public CompoundWhereStatement<TWhereStatement> IsGreaterThanOrEqualTo(int value) => CreateAndAddWhereComparisonCondition(ComparisonOperators.IsGreaterThanOrEqualTo, value);

        /// <summary>
        /// Adds a less than (<![CDATA[<]]>) comparison to the condition in the WHERE statement.
        /// </summary>
        /// <param name="value">The value to compare against.</param>
        /// <returns>A conjunction class that supports appending more conditions to the WHERE statements via OR or AND terms.</returns>
        public CompoundWhereStatement<TWhereStatement> IsLessThan(int value) => CreateAndAddWhereComparisonCondition(ComparisonOperators.IsLessThan, value);

        /// <summary>
        /// Adds a less than or equal to (<![CDATA[<=]]>) comparison to the condition in the WHERE statement.
        /// </summary>
        /// <param name="value">The value to compare against.</param>
        /// <returns>A conjunction class that supports appending more conditions to the WHERE statements via OR or AND terms.</returns>
        public CompoundWhereStatement<TWhereStatement> IsLessThanOrEqualTo(int value) => CreateAndAddWhereComparisonCondition(ComparisonOperators.IsLessThanOrEqualTo, value);

        /// <summary>
        /// Adds a not equals (!=) comparison to the condition in the WHERE statement.
        /// </summary>
        /// <param name="value">The value to compare against.</param>
        /// <returns>A conjunction class that supports appending more conditions to the WHERE statements via OR or AND terms.</returns>
        public CompoundWhereStatement<TWhereStatement> NotEqualTo(object value) => CreateAndAddWhereComparisonCondition(ComparisonOperators.NotEqualTo, value);

        /// <summary>
        /// Adds a scalar unary operator to check if the property is a bool type.
        /// </summary>
        /// <returns>A conjunction class that supports appending more conditions to the WHERE statements via OR or AND terms.</returns>
        public CompoundWhereStatement<TWhereStatement> IsBool() => CreateAndAddWhereScalarCondition(ScalarOperators.IS_BOOL);

        /// <summary>
        /// Adds a scalar unary operator to check if the property is defined.
        /// </summary>
        /// <returns>A conjunction class that supports appending more conditions to the WHERE statements via OR or AND terms.</returns>
        public CompoundWhereStatement<TWhereStatement> IsDefined() => CreateAndAddWhereScalarCondition(ScalarOperators.IS_DEFINED);

        /// <summary>
        /// Adds a scalar unary operator to check if the property is NULL.
        /// </summary>
        /// <returns>A conjunction class that supports appending more conditions to the WHERE statements via OR or AND terms.</returns>
        public CompoundWhereStatement<TWhereStatement> IsNull() => CreateAndAddWhereScalarCondition(ScalarOperators.IS_NULL);

        /// <summary>
        /// Adds a scalar unary operator to check if the property is a number type.
        /// </summary>
        /// <returns>A conjunction class that supports appending more conditions to the WHERE statements via OR or AND terms.</returns>
        public CompoundWhereStatement<TWhereStatement> IsNumber() => CreateAndAddWhereScalarCondition(ScalarOperators.IS_NUMBER);

        /// <summary>
        /// Adds a scalar unary operator to check if the property is an object type.
        /// </summary>
        /// <returns>A conjunction class that supports appending more conditions to the WHERE statements via OR or AND terms.</returns>
        public CompoundWhereStatement<TWhereStatement> IsObject() => CreateAndAddWhereScalarCondition(ScalarOperators.IS_OBJECT);

        /// <summary>
        /// Adds a scalar unary operator to check if the property is a primitive type.
        /// </summary>
        /// <returns>A conjunction class that supports appending more conditions to the WHERE statements via OR or AND terms.</returns>
        public CompoundWhereStatement<TWhereStatement> IsPrimitive() => CreateAndAddWhereScalarCondition(ScalarOperators.IS_PRIMITIVE);

        /// <summary>
        /// Adds a scalar unary operator to check if the property is a string type.
        /// </summary>
        /// <returns>A conjunction class that supports appending more conditions to the WHERE statements via OR or AND terms.</returns>
        public CompoundWhereStatement<TWhereStatement> IsString() => CreateAndAddWhereScalarCondition(ScalarOperators.IS_STRING);

        /// <summary>
        /// Adds a scalar binary operator to check if the value of a property starts with the given value.
        /// </summary>
        /// <param name="value">The value to compare against.</param>
        /// <returns>A conjunction class that supports appending more conditions to the WHERE statements via OR or AND terms.</returns>
        public CompoundWhereStatement<TWhereStatement> StartsWith(string value) => CreateAndAddScalarBinaryOperatorCondition(ScalarOperators.STARTSWITH, value);

        /// <summary>
        /// Adds a scalar binary operator to check if the value of a property contains the given value.
        /// </summary>
        /// <param name="value">The value to compare against.</param>
        /// <returns>A conjunction class that supports appending more conditions to the WHERE statements via OR or AND terms.</returns>
        public CompoundWhereStatement<TWhereStatement> Contains(string value) => CreateAndAddScalarBinaryOperatorCondition(ScalarOperators.CONTAINS, value);

        /// <summary>
        /// Adds a scalar binary operator to check if the value of a property ends with the given value.
        /// </summary>
        /// <param name="value">The value to compare against.</param>
        /// <returns>A conjunction class that supports appending more conditions to the WHERE statements via OR or AND terms.</returns>
        public CompoundWhereStatement<TWhereStatement> EndsWith(string value) => CreateAndAddScalarBinaryOperatorCondition(ScalarOperators.ENDSWITH, value);

        private CompoundWhereStatement<TWhereStatement> CreateAndAddWhereScalarCondition(ScalarOperator op)
        {
            var condition = new WhereScalarFunctionCondition
            {
                Column = propertyName,
                ScalarFunction = op,
                Alias = alias
            };

            clause.AddCondition(condition);
            return new CompoundWhereStatement<TWhereStatement>(joinClauses, clause, alias);
        }

        private CompoundWhereStatement<TWhereStatement> CreateAndAddScalarBinaryOperatorCondition(ScalarOperator binaryOperator, string value)
        {
            var condition = new WhereScalarFunctionCondition
            {
                Column = propertyName,
                Alias = alias,
                Value = value,
                ScalarFunction = binaryOperator
            };

            clause.AddCondition(condition);
            return new CompoundWhereStatement<TWhereStatement>(joinClauses, clause, alias);
        }

        private CompoundWhereStatement<TWhereStatement> CreateAndAddWhereComparisonCondition(ComparisonOperators op, object value)
        {
            if (value is Enum e)
            {
                value = GetEnumValue(e);
            }

            var condition = new WhereComparisonCondition
            {
                Column = propertyName,
                Value = value,
                Operator = op,
                Alias = alias
            };

            clause.AddCondition(condition);
            return new CompoundWhereStatement<TWhereStatement>(joinClauses, clause, alias);
        }

        private string GetEnumValue(Enum value)
        {
            var enumName = value.GetType().GetEnumName(value);
            var enumField = value.GetType().GetField(enumName);
            var customValue = enumField.GetCustomAttribute<EnumMemberAttribute>();
            var defaultValue = Convert.ChangeType(value, value.GetTypeCode());
            return customValue is null ? defaultValue.ToString() : customValue.Value;
        }
    }
}