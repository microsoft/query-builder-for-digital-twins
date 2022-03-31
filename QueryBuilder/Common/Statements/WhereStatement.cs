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
    ///
    /// </summary>
    /// <typeparam name="TWhereStatement"></typeparam>
    public abstract class WhereBaseStatement<TWhereStatement> where TWhereStatement : WhereBaseStatement<TWhereStatement>
    {
        /// <summary>
        ///
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
        ///
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="forAlias"></param>
        /// <returns></returns>
        public WherePropertyStatement<TWhereStatement> Property(string propertyName, string forAlias = null)
        {
            var aliasOverride = string.IsNullOrWhiteSpace(forAlias) ? Alias : forAlias;
            return new WherePropertyStatement<TWhereStatement>(JoinOptions, WhereClause, propertyName, aliasOverride);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="nested"></param>
        /// <returns></returns>
        public WhereCombineStatement<TWhereStatement> Precedence(Func<TWhereStatement, WhereCombineStatement<TWhereStatement>> nested)
        {
            var statement = ActivatorHelper.CreateInstance<TWhereStatement>(JoinOptions, new WhereClause(), Alias);
            var n = nested.Invoke(statement);
            WhereClause.AddCondition($"({n.WhereClause.GetConditionsText()})");
            return new WhereCombineStatement<TWhereStatement>(JoinOptions, WhereClause, Alias);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="nested"></param>
        /// <returns></returns>
        public WhereCombineStatement<TWhereStatement> Not(Func<TWhereStatement, WhereCombineStatement<TWhereStatement>> nested)
        {
            var statement = ActivatorHelper.CreateInstance<TWhereStatement>(new WhereClause(), Alias);
            var w = nested.Invoke(statement);
            WhereClause.AddCondition($"{Terms.Not} {w.WhereClause.GetConditionsText()}");
            return new WhereCombineStatement<TWhereStatement>(JoinOptions, WhereClause, Alias);
        }
    }

    /// <summary>
    ///
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
    ///
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
        ///
        /// </summary>
        /// <param name="dtmi"></param>
        /// <returns></returns>
        public WhereCombineStatement<WhereStatement> IsOfModel(string dtmi)
        {
            WhereClause.AddCondition(new WhereIsOfModelCondition { Alias = this.Alias, Model = dtmi });
            return new WhereCombineStatement<WhereStatement>(JoinOptions, WhereClause, Alias);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public WherePropertyStatement<WhereStatement> RelationshipProperty(string propertyName)
        {
            var relationshipAlias = string.IsNullOrWhiteSpace(JoinOptions.RelationshipAlias) ? $"{JoinOptions.RelationshipName.ToLowerInvariant()}relationship" : JoinOptions.RelationshipAlias;
            return new WherePropertyStatement<WhereStatement>(JoinOptions, WhereClause, propertyName, relationshipAlias);
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TWhereStatement"></typeparam>
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
        ///
        /// </summary>
        /// <returns></returns>
        public TWhereStatement And()
        {
            WhereClause.AddCondition(Terms.And);
            return new WhereStatement(JoinOptions, WhereClause, alias) as TWhereStatement;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="nested"></param>
        /// <returns></returns>
        public WhereCombineStatement<TWhereStatement> And(Func<TWhereStatement, WhereCombineStatement<TWhereStatement>> nested)
        {
            var statement = ActivatorHelper.CreateInstance<TWhereStatement>(new WhereClause(), alias);
            var w = nested.Invoke(statement);
            And();
            WhereClause.AddCondition(w.WhereClause.Conditions.FirstOrDefault());
            return this;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public TWhereStatement Or()
        {
            WhereClause.AddCondition(Terms.Or);
            return new WhereStatement(JoinOptions, WhereClause, alias) as TWhereStatement;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="nested"></param>
        /// <returns></returns>
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
    ///
    /// </summary>
    /// <typeparam name="TWhereStatement"></typeparam>
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
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public WhereCombineStatement<TWhereStatement> IsEqualTo(object value) => CreateAndAddWhereComparisonCondition(ComparisonOperators.IsEqualTo, value);

        /// <summary>
        ///
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
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
        ///
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
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
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public WhereCombineStatement<TWhereStatement> IsGreaterThan(int value) => CreateAndAddWhereComparisonCondition(ComparisonOperators.IsGreaterThan, value);

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public WhereCombineStatement<TWhereStatement> IsGreaterThanOrEqualTo(int value) => CreateAndAddWhereComparisonCondition(ComparisonOperators.IsGreaterThanOrEqualTo, value);

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public WhereCombineStatement<TWhereStatement> IsLessThan(int value) => CreateAndAddWhereComparisonCondition(ComparisonOperators.IsLessThan, value);

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public WhereCombineStatement<TWhereStatement> IsLessThanOrEqualTo(int value) => CreateAndAddWhereComparisonCondition(ComparisonOperators.IsLessThanOrEqualTo, value);

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public WhereCombineStatement<TWhereStatement> NotEqualTo(object value) => CreateAndAddWhereComparisonCondition(ComparisonOperators.NotEqualTo, value);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public WhereCombineStatement<TWhereStatement> IsBool() => CreateAndAddWhereScalarCondition(ScalarOperators.IS_BOOL);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public WhereCombineStatement<TWhereStatement> IsDefined() => CreateAndAddWhereScalarCondition(ScalarOperators.IS_DEFINED);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public WhereCombineStatement<TWhereStatement> IsNull() => CreateAndAddWhereScalarCondition(ScalarOperators.IS_NULL);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public WhereCombineStatement<TWhereStatement> IsNumber() => CreateAndAddWhereScalarCondition(ScalarOperators.IS_NUMBER);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public WhereCombineStatement<TWhereStatement> IsObject() => CreateAndAddWhereScalarCondition(ScalarOperators.IS_OBJECT);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public WhereCombineStatement<TWhereStatement> IsPrimitive() => CreateAndAddWhereScalarCondition(ScalarOperators.IS_PRIMITIVE);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public WhereCombineStatement<TWhereStatement> IsString() => CreateAndAddWhereScalarCondition(ScalarOperators.IS_STRING);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public WhereCombineStatement<TWhereStatement> StartsWith(string value) => CreateAndAddScalarBinaryOperatorCondition(ScalarOperators.STARTSWITH, value);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public WhereCombineStatement<TWhereStatement> Contains(string value) => CreateAndAddScalarBinaryOperatorCondition(ScalarOperators.CONTAINS, value);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
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