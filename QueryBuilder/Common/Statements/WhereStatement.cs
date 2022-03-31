// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Statements
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Clauses;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Helpers;

    /// <summary>
    /// An abstract class that provides the base layer of methods that are common between
    /// WHERE statement classes that are applicable to either Twins or Relationships.
    /// </summary>
    /// <typeparam name="TWhereStatement">The type of WHERE statement supported.</typeparam>
    public abstract class WhereBaseStatement<TWhereStatement> where TWhereStatement : WhereBaseStatement<TWhereStatement>
    {
        /// <summary>
        /// The alias for the current WHERE statement.
        /// </summary>
        protected readonly string Alias;
        internal readonly WhereClause WhereClause;
        internal readonly JoinOptions JoinOptions;

        internal WhereBaseStatement(JoinOptions joinOptions, WhereClause clause, string alias) : this(clause, alias)
        {
            JoinOptions = joinOptions;
        }

        internal WhereBaseStatement(WhereClause whereClause, string alias)
        {
            WhereClause = whereClause;
            Alias = alias;
        }

        /// <summary>
        /// Adds the propertyName to filter against to the WHERE statement.
        /// </summary>
        /// <param name="propertyName">The name of the property to filter against.</param>
        /// <param name="forAlias">
        /// Optional: Alias to override the default alias in the current scope.
        /// This allows applying WHERE conditions to previous scopes of the query.
        /// I.e. It can allow applying a WHERE condition to a relationship outside the scope of a previous JOIN statement.
        /// </param>
        /// <returns>A statement class that contains various unary or binary comparison methods to finalize the WHERE statement.</returns>
        public WherePropertyStatement<TWhereStatement> Property(string propertyName, string forAlias = null)
        {
            var aliasOverride = string.IsNullOrWhiteSpace(forAlias) ? Alias : forAlias;
            return new WherePropertyStatement<TWhereStatement>(JoinOptions, WhereClause, propertyName, aliasOverride);
        }

        /// <summary>
        /// Wraps a given set of WHERE conditions in parenthesis to establish a precedence.
        /// </summary>
        /// <param name="nested">The functional logic of the WHERE statement containing one or more WHERE conditions.</param>
        /// <returns>A conjunction class that supports appending more conditions to the WHERE statements via OR or AND terms.</returns>
        public WhereCombineStatement<TWhereStatement> Precedence(Func<TWhereStatement, WhereCombineStatement<TWhereStatement>> nested)
        {
            var statement = ActivatorHelper.CreateInstance<TWhereStatement>(JoinOptions, new WhereClause(), Alias);
            var n = nested.Invoke(statement);
            WhereClause.AddCondition($"({n.WhereClause.GetConditionsText()})");
            return new WhereCombineStatement<TWhereStatement>(JoinOptions, WhereClause, Alias);
        }

        /// <summary>
        /// Prepends a NOT term to one or more WHERE clauses.
        /// </summary>
        /// <param name="nested">The functional logic of the WHERE statement containing one or more WHERE conditions.</param>
        /// <returns>A conjunction class that supports appending more conditions to the WHERE statements via OR or AND terms.</returns>
        public WhereCombineStatement<TWhereStatement> Not(Func<TWhereStatement, WhereCombineStatement<TWhereStatement>> nested)
        {
            var statement = ActivatorHelper.CreateInstance<TWhereStatement>(new WhereClause(), Alias);
            var w = nested.Invoke(statement);
            WhereClause.AddCondition($"{Terms.Not} {w.WhereClause.GetConditionsText()}");
            return new WhereCombineStatement<TWhereStatement>(JoinOptions, WhereClause, Alias);
        }
    }

    /// <summary>
    /// A WHERE statement class used for querying relationships.
    /// </summary>
    public class WhereRelationshipsStatement : WhereBaseStatement<WhereRelationshipsStatement>
    {
        internal WhereRelationshipsStatement(JoinOptions joinOptions, WhereClause clause, string alias) : base(joinOptions, clause, alias)
        {
        }

        internal WhereRelationshipsStatement(WhereClause clause, string alias) : base(clause, alias)
        {
        }
    }

    /// <summary>
    /// A WHERE statement class used for querying twins.
    /// </summary>
    public class WhereStatement : WhereBaseStatement<WhereStatement>
    {
        internal WhereStatement(JoinOptions joinOptions, WhereClause clause, string alias) : base(joinOptions, clause, alias)
        {
        }

        internal WhereStatement(WhereClause whereClause, string alias) : base(whereClause, alias)
        {
        }

        /// <summary>
        /// Adds an IS_OF_MODEL scalar function to the WHERE statement to filter by a model dtmi.
        /// </summary>
        /// <param name="dtmi">The model dtmi to use in the scalar function.</param>
        /// <returns>A conjunction class that supports appending more conditions to the WHERE statements via OR or AND terms.</returns>
        public WhereCombineStatement<WhereStatement> IsOfModel(string dtmi)
        {
            WhereClause.AddCondition(new WhereIsOfModelCondition { Alias = this.Alias, Model = dtmi });
            return new WhereCombineStatement<WhereStatement>(JoinOptions, WhereClause, Alias);
        }

        /// <summary>
        /// Adds the propertyName to filter against to the WHERE statement.
        /// </summary>
        /// <param name="propertyName">The name of the relationship property to filter against.</param>
        /// <returns>A statement class that contains various unary or binary comparison methods to finalize the WHERE statement.</returns>
        public WherePropertyStatement<WhereStatement> RelationshipProperty(string propertyName)
        {
            var relationshipAlias = string.IsNullOrWhiteSpace(JoinOptions.RelationshipAlias) ? $"{JoinOptions.RelationshipName.ToLowerInvariant()}relationship" : JoinOptions.RelationshipAlias;
            return new WherePropertyStatement<WhereStatement>(JoinOptions, WhereClause, propertyName, relationshipAlias);
        }
    }

    /// <summary>
    /// A conjunction class that supports appending more conditions to the WHERE statements via OR or AND terms.
    /// </summary>
    /// <typeparam name="TWhereStatement">The type of WHERE statement supported.</typeparam>
    public class WhereCombineStatement<TWhereStatement> where TWhereStatement : WhereBaseStatement<TWhereStatement>
    {
        internal JoinOptions JoinOptions { get; private set; }

        internal WhereClause WhereClause { get; private set; }

        private string alias;

        internal WhereCombineStatement(JoinOptions joinOptions, WhereClause whereClause, string alias)
        {
            JoinOptions = joinOptions;
            WhereClause = whereClause;
            this.alias = alias;
        }

        /// <summary>
        /// Appends an AND term to the current WHERE statement.
        /// </summary>
        /// <returns>he WHERE statement implementation supported in this statement. I.e. Either for twins or relationships.</returns>
        public TWhereStatement And()
        {
            WhereClause.AddCondition(Terms.And);
            return new WhereStatement(JoinOptions, WhereClause, alias) as TWhereStatement;
        }

        /// <summary>
        /// Adds one or more WHERE clauses preceded by an AND term to the WHERE statement.
        /// </summary>
        /// <param name="nested">The functional logic of the WHERE statement containing one or more WHERE conditions.</param>
        /// <returns>A conjunction class that supports appending more conditions to the WHERE statements via OR or AND terms.</returns>
        public WhereCombineStatement<TWhereStatement> And(Func<TWhereStatement, WhereCombineStatement<TWhereStatement>> nested)
        {
            var statement = ActivatorHelper.CreateInstance<TWhereStatement>(new WhereClause(), alias);
            var w = nested.Invoke(statement);
            And();
            WhereClause.AddCondition(w.WhereClause.Conditions.FirstOrDefault());
            return this;
        }

        /// <summary>
        /// Appends an OR term to the current WHERE statement.
        /// </summary>
        /// <returns>The WHERE statement implementation supported in this statement. I.e. Either for twins or relationships.</returns>
        public TWhereStatement Or()
        {
            WhereClause.AddCondition(Terms.Or);
            return new WhereStatement(JoinOptions, WhereClause, alias) as TWhereStatement;
        }

        /// <summary>
        /// Adds one or more WHERE clauses preceded by an OR term to the WHERE statement.
        /// </summary>
        /// <param name="nested">The functional logic of the WHERE statement containing one or more WHERE conditions.</param>
        /// <returns>A conjunction class that supports appending more conditions to the WHERE statements via OR or AND terms.</returns>
        public WhereCombineStatement<TWhereStatement> Or(Func<TWhereStatement, WhereCombineStatement<TWhereStatement>> nested)
        {
            var statement = ActivatorHelper.CreateInstance<TWhereStatement>(new WhereClause(), alias);
            var w = nested.Invoke(statement);
            Or();
            WhereClause.AddCondition(w.WhereClause.Conditions.FirstOrDefault());
            return this;
        }
    }

    /// <summary>
    /// A statement class that contains various unary or binary comparison methods to finalize the WHERE statement.
    /// </summary>
    /// <typeparam name="TWhereStatement">The type of WHERE statement supported.</typeparam>
    public class WherePropertyStatement<TWhereStatement> where TWhereStatement : WhereBaseStatement<TWhereStatement>
    {
        private readonly string propertyName;
        private readonly string alias;
        private readonly WhereClause clause;
        private readonly JoinOptions joinOptions;

        internal WherePropertyStatement(JoinOptions joinOptions, WhereClause clause, string propertyName, string alias)
        {
            this.joinOptions = joinOptions;
            this.propertyName = propertyName;
            this.clause = clause;
            this.alias = alias;
        }

        /// <summary>
        /// Adds an equals (=) comparison to the condition in the WHERE statement.
        /// </summary>
        /// <param name="value">The value to compare against.</param>
        /// <returns>A conjunction class that supports appending more conditions to the WHERE statements via OR or AND terms.</returns>
        public WhereCombineStatement<TWhereStatement> IsEqualTo(object value) => CreateAndAddWhereComparisonCondition(ComparisonOperators.IsEqualTo, value);

        /// <summary>
        /// Adds an IN comparison to the condition in the WHERE statement to check if a property's value is in a given set of values.
        /// </summary>
        /// <param name="values">The value to compare against.</param>
        /// <returns>A conjunction class that supports appending more conditions to the WHERE statements via OR or AND terms.</returns>
        public WhereCombineStatement<TWhereStatement> IsIn(string[] values)
        {
            var condition = new WhereInCondition
            {
                Column = propertyName,
                Value = values,
                Alias = alias
            };

            clause.AddCondition(condition);
            return new WhereCombineStatement<TWhereStatement>(joinOptions, clause, alias);
        }

        /// <summary>
        /// Adds a NOT IN comparison to the condition in the WHERE statement to check if a property's value is not in a given set of values.
        /// </summary>
        /// <param name="values">The value to compare against.</param>
        /// <returns>A conjunction class that supports appending more conditions to the WHERE statements via OR or AND terms.</returns>
        public WhereCombineStatement<TWhereStatement> IsNotIn(string[] values)
        {
            var condition = new WhereNotInCondition
            {
                Column = propertyName,
                Value = values,
                Alias = alias
            };

            clause.AddCondition(condition);
            return new WhereCombineStatement<TWhereStatement>(joinOptions, clause, alias);
        }

        /// <summary>
        /// Adds a greater than (>) comparison to the condition in the WHERE statement.
        /// </summary>
        /// <param name="value">The value to compare against.</param>
        /// <returns>A conjunction class that supports appending more conditions to the WHERE statements via OR or AND terms.</returns>
        public WhereCombineStatement<TWhereStatement> IsGreaterThan(int value) => CreateAndAddWhereComparisonCondition(ComparisonOperators.IsGreaterThan, value);

        /// <summary>
        /// Adds a greater than ore equal to (>=) comparison to the condition in the WHERE statement.
        /// </summary>
        /// <param name="value">The value to compare against.</param>
        /// <returns>A conjunction class that supports appending more conditions to the WHERE statements via OR or AND terms.</returns>
        public WhereCombineStatement<TWhereStatement> IsGreaterThanOrEqualTo(int value) => CreateAndAddWhereComparisonCondition(ComparisonOperators.IsGreaterThanOrEqualTo, value);

        /// <summary>
        /// Adds a less than (<![CDATA[<]]>) comparison to the condition in the WHERE statement.
        /// </summary>
        /// <param name="value">The value to compare against.</param>
        /// <returns>A conjunction class that supports appending more conditions to the WHERE statements via OR or AND terms.</returns>
        public WhereCombineStatement<TWhereStatement> IsLessThan(int value) => CreateAndAddWhereComparisonCondition(ComparisonOperators.IsLessThan, value);

        /// <summary>
        /// Adds a less than or equal to (<![CDATA[<=]]>) comparison to the condition in the WHERE statement.
        /// </summary>
        /// <param name="value">The value to compare against.</param>
        /// <returns>A conjunction class that supports appending more conditions to the WHERE statements via OR or AND terms.</returns>
        public WhereCombineStatement<TWhereStatement> IsLessThanOrEqualTo(int value) => CreateAndAddWhereComparisonCondition(ComparisonOperators.IsLessThanOrEqualTo, value);

        /// <summary>
        /// Adds a not equals (!=) comparison to the condition in the WHERE statement.
        /// </summary>
        /// <param name="value">The value to compare against.</param>
        /// <returns>A conjunction class that supports appending more conditions to the WHERE statements via OR or AND terms.</returns>
        public WhereCombineStatement<TWhereStatement> NotEqualTo(object value) => CreateAndAddWhereComparisonCondition(ComparisonOperators.NotEqualTo, value);

        /// <summary>
        /// Adds a scalar unary operator to check if the property is a bool type.
        /// </summary>
        /// <returns>A conjunction class that supports appending more conditions to the WHERE statements via OR or AND terms.</returns>
        public WhereCombineStatement<TWhereStatement> IsBool() => CreateAndAddWhereScalarCondition(ScalarOperators.IS_BOOL);

        /// <summary>
        /// Adds a scalar unary operator to check if the property is defined.
        /// </summary>
        /// <returns>A conjunction class that supports appending more conditions to the WHERE statements via OR or AND terms.</returns>
        public WhereCombineStatement<TWhereStatement> IsDefined() => CreateAndAddWhereScalarCondition(ScalarOperators.IS_DEFINED);

        /// <summary>
        /// Adds a scalar unary operator to check if the property is NULL.
        /// </summary>
        /// <returns>A conjunction class that supports appending more conditions to the WHERE statements via OR or AND terms.</returns>
        public WhereCombineStatement<TWhereStatement> IsNull() => CreateAndAddWhereScalarCondition(ScalarOperators.IS_NULL);

        /// <summary>
        /// Adds a scalar unary operator to check if the property is a number type.
        /// </summary>
        /// <returns>A conjunction class that supports appending more conditions to the WHERE statements via OR or AND terms.</returns>
        public WhereCombineStatement<TWhereStatement> IsNumber() => CreateAndAddWhereScalarCondition(ScalarOperators.IS_NUMBER);

        /// <summary>
        /// Adds a scalar unary operator to check if the property is an object type.
        /// </summary>
        /// <returns>A conjunction class that supports appending more conditions to the WHERE statements via OR or AND terms.</returns>
        public WhereCombineStatement<TWhereStatement> IsObject() => CreateAndAddWhereScalarCondition(ScalarOperators.IS_OBJECT);

        /// <summary>
        /// Adds a scalar unary operator to check if the property is a primitive type.
        /// </summary>
        /// <returns>A conjunction class that supports appending more conditions to the WHERE statements via OR or AND terms.</returns>
        public WhereCombineStatement<TWhereStatement> IsPrimitive() => CreateAndAddWhereScalarCondition(ScalarOperators.IS_PRIMITIVE);

        /// <summary>
        /// Adds a scalar unary operator to check if the property is a string type.
        /// </summary>
        /// <returns>A conjunction class that supports appending more conditions to the WHERE statements via OR or AND terms.</returns>
        public WhereCombineStatement<TWhereStatement> IsString() => CreateAndAddWhereScalarCondition(ScalarOperators.IS_STRING);

        /// <summary>
        /// Adds a scalar binary operator to check if the value of a property starts with the given value.
        /// </summary>
        /// <param name="value">The value to compare against.</param>
        /// <returns>A conjunction class that supports appending more conditions to the WHERE statements via OR or AND terms.</returns>
        public WhereCombineStatement<TWhereStatement> StartsWith(string value) => CreateAndAddScalarBinaryOperatorCondition(ScalarOperators.STARTSWITH, value);

        /// <summary>
        /// Adds a scalar binary operator to check if the value of a property contains the given value.
        /// </summary>
        /// <param name="value">The value to compare against.</param>
        /// <returns>A conjunction class that supports appending more conditions to the WHERE statements via OR or AND terms.</returns>
        public WhereCombineStatement<TWhereStatement> Contains(string value) => CreateAndAddScalarBinaryOperatorCondition(ScalarOperators.CONTAINS, value);

        /// <summary>
        /// Adds a scalar binary operator to check if the value of a property ends with the given value.
        /// </summary>
        /// <param name="value">The value to compare against.</param>
        /// <returns>A conjunction class that supports appending more conditions to the WHERE statements via OR or AND terms.</returns>
        public WhereCombineStatement<TWhereStatement> EndsWith(string value) => CreateAndAddScalarBinaryOperatorCondition(ScalarOperators.ENDSWITH, value);

        private WhereCombineStatement<TWhereStatement> CreateAndAddWhereScalarCondition(AdtScalarOperator op)
        {
            var condition = new WhereScalarFunctionCondition
            {
                Column = propertyName,
                ScalarFunction = op,
                Alias = alias
            };

            clause.AddCondition(condition);
            return new WhereCombineStatement<TWhereStatement>(joinOptions, clause, alias);
        }

        private WhereCombineStatement<TWhereStatement> CreateAndAddScalarBinaryOperatorCondition(AdtScalarOperator binaryOperator, string value)
        {
            var condition = new WhereScalarFunctionCondition
            {
                Column = propertyName,
                Alias = alias,
                Value = value,
                ScalarFunction = binaryOperator
            };

            clause.AddCondition(condition);
            return new WhereCombineStatement<TWhereStatement>(joinOptions, clause, alias);
        }

        private WhereCombineStatement<TWhereStatement> CreateAndAddWhereComparisonCondition(ComparisonOperators op, object value)
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
            return new WhereCombineStatement<TWhereStatement>(joinOptions, clause, alias);
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