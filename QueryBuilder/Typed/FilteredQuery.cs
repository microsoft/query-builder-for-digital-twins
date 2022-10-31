// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Typed
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Runtime.Serialization;
    using global::Azure.DigitalTwins.Core;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Clauses;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Helpers;

    /// <summary>
    /// A wrapper for the common functionalities of query builder.
    /// </summary>
    public abstract class FilteredQuery<TQuery> : TypedQueryBase
        where TQuery : FilteredQuery<TQuery>
    {
        internal FilteredQuery(IDictionary<string, Type> aliasToTypeMapping, SelectClause selectClause, FromClause fromClause, IList<JoinClause> joinClauses, WhereClause whereClause) : base(aliasToTypeMapping, selectClause, fromClause, joinClauses, whereClause)
        {
        }

        /// <summary>
        /// Add Where condition to ADT Query Builder.
        /// </summary>
        /// <param name="expression">LINQ Expression.</param>
        /// <returns>ADT query instance.</returns>
        public TQuery Where<TModel>(Expression<Func<TModel, bool>> expression)
            where TModel : BasicDigitalTwin
        {
            AddConditions(expression);
            return (TQuery)this;
        }

        /// <summary>
        /// Add Where condition to ADT Query Builder.
        /// </summary>
        /// <param name="propertySelector">Expression to select property of TModel.</param>
        /// <param name="scalarFunction">ADT comparison operator.</param>
        /// <param name="alias"> Optional - Model Alias.</param>
        /// <returns>ADT query instance.</returns>
        public TQuery Where<TModel>(Expression<Func<TModel, object>> propertySelector, ScalarUnaryOperator scalarFunction, string alias = null)
            where TModel : BasicDigitalTwin
        {
            QueryValidator.ExtractModelAndPropertyName(propertySelector, out Type type, out string propertyName);
            whereClause.AddCondition(CreateWhereScalarCondition<TModel>(propertyName, scalarFunction, type, alias));

            return (TQuery)this;
        }

        /// <summary>
        /// Add Where condition to ADT Query Builder.
        /// </summary>
        /// <param name="propertyName">String property name of TModel type to apply Adt scalar function.</param>
        /// <param name="scalarFunction">ADT comparison operator.</param>
        /// <param name="alias">Optional - Model Alias.</param>
        /// <returns>ADT query instance.</returns>
        public TQuery Where<TModel>(string propertyName, ScalarUnaryOperator scalarFunction, string alias = null)
        {
            whereClause.AddCondition(CreateWhereScalarCondition<TModel>(propertyName, scalarFunction, typeof(TModel), alias));

            return (TQuery)this;
        }

        /// <summary>
        /// Add Where condition to ADT Query Builder.
        /// </summary>
        /// <param name="propertySelector">Expression to select property of TModel type.</param>
        /// <param name="operation">ADT comparison operator.</param>
        /// <param name="value">Value on which comparison needs to be done.</param>
        /// <param name="alias">Optional - Model Alias.</param>
        /// <returns>ADT query instance.</returns>
        public TQuery Where<TModel>(Expression<Func<TModel, object>> propertySelector, ComparisonOperators operation, object value, string alias = null)
            where TModel : BasicDigitalTwin
        {
            QueryValidator.ExtractModelAndPropertyName(propertySelector, out Type type, out string propertyName);
            whereClause.AddCondition(CreateWhereComparisonCondition<TModel>(propertyName, operation, value, type, alias));

            return (TQuery)this;
        }

        /// <summary>
        /// Add Where condition to ADT Query Builder.
        /// </summary>
        /// <param name="propertyName">String property name of TModel type on which where condition is applied on.</param>
        /// <param name="operation">ADT comparison operator.</param>
        /// <param name="value">Value on which comparison needs to be done.</param>
        /// <param name="alias">Optional - Model Alias.</param>
        /// <returns>ADT query instance.</returns>
        public TQuery Where<TModel>(string propertyName, ComparisonOperators operation, object value, string alias = null)
        {
            whereClause.AddCondition(CreateWhereComparisonCondition<TModel>(propertyName, operation, value, typeof(TModel), alias));

            return (TQuery)this;
        }

        /// <summary>
        /// Add where in operator.
        /// </summary>
        /// <param name="propertySelector">Expression to select property of TModel type.</param>
        /// <param name="values">Values to compare against.</param>
        /// <param name="alias">Optional - Model Alias.</param>
        /// <returns>ADT query instance.</returns>
        public TQuery WhereIn<TModel>(Expression<Func<TModel, object>> propertySelector, string[] values, string alias = null)
            where TModel : BasicDigitalTwin
        {
            QueryValidator.ExtractModelAndPropertyName(propertySelector, out Type type, out string propertyName);
            whereClause.AddCondition(CreateWhereInCondition<TModel>(propertyName, values, type, alias));

            return (TQuery)this;
        }

        /// <summary>
        /// Add where in operator.
        /// </summary>
        /// <param name="propertyName">String property name of TModel type on which scalar function applied on.</param>
        /// <param name="values">Values to compare against.</param>
        /// <param name="alias">Optional - Model Alias.</param>
        /// <returns>ADT query instance.</returns>
        public TQuery WhereIn<TModel>(string propertyName, string[] values, string alias = null)
        {
            whereClause.AddCondition(CreateWhereInCondition<TModel>(propertyName, values, typeof(TModel), alias));

            return (TQuery)this;
        }

        /// <summary>
        /// Add where not in operator.
        /// </summary>
        /// <param name="propertySelector">Expression to select property of TModel type.</param>
        /// <param name="values">Values to compare against.</param>
        /// <param name="alias">Optional - Model Alias.</param>
        /// <returns>ADT query instance.</returns>
        public TQuery WhereNotIn<TModel>(Expression<Func<TModel, object>> propertySelector, string[] values, string alias = null)
            where TModel : BasicDigitalTwin
        {
            QueryValidator.ExtractModelAndPropertyName(propertySelector, out Type type, out string propertyName);
            whereClause.AddCondition(CreateWhereNotInCondition<TModel>(propertyName, values, type, alias));

            return (TQuery)this;
        }

        /// <summary>
        /// Add where not in operator.
        /// </summary>
        /// <param name="propertyName">String property name of TModel type on which where condition is applied on.</param>
        /// <param name="values">Values to compare against.</param>
        /// <param name="alias">Optional - Model Alias.</param>
        /// <returns>ADT query instance.</returns>
        public TQuery WhereNotIn<TModel>(string propertyName, string[] values, string alias = null)
        {
            whereClause.AddCondition(CreateWhereNotInCondition<TModel>(propertyName, values, typeof(TModel), alias));

            return (TQuery)this;
        }

        /// <summary>
        /// Add an isOfModel operator.
        /// It is recommended only to use the isOfModel operator when there is no model inferred from the FROM clause. No model is inferred when using the root type BasicDigitalTwin in the FROM clause.
        /// </summary>
        /// <typeparam name="TBase">Base model type.</typeparam>
        /// <typeparam name="TDerived">Derived model type.</typeparam>
        /// <param name="alias">Optional - Model Alias.</param>
        /// <returns>ADT query instance.</returns>
        public TQuery WhereIsOfModel<TBase, TDerived>(string alias = null)
            where TBase : BasicDigitalTwin
            where TDerived : TBase
        {
            whereClause.AddCondition(CreateWhereIsOfModelCondition<TBase, TDerived>(alias));

            return (TQuery)this;
        }

        /// <summary>
        /// Add a Start with where clause.
        /// </summary>
        /// <typeparam name="TModel">Model type.</typeparam>
        /// <param name="propertySelector">Expression to select property of TModel type.</param>
        /// <param name="value">String value to compare.</param>
        /// <param name="alias">Optional - Model Alias.</param>
        /// <returns>ADT query instance.</returns>
        public TQuery WhereStartsWith<TModel>(Expression<Func<TModel, object>> propertySelector, string value, string alias = null)
            where TModel : BasicDigitalTwin
        {
            QueryValidator.ExtractModelAndPropertyName(propertySelector, out Type type, out string propertyName);
            whereClause.AddCondition(CreateAdtScalarBinaryOperatorCondition<TModel>(propertyName, value, type, alias, ScalarOperators.STARTSWITH));

            return (TQuery)this;
        }

        /// <summary>
        /// Add a Start with where clause.
        /// </summary>
        /// <typeparam name="TModel">Model type.</typeparam>
        /// <param name="propertyName">String property name of TModel type on which where condition is applied on.</param>
        /// <param name="value">String value to compare.</param>
        /// <param name="alias">Optional - Model Alias.</param>
        /// <returns>ADT query instance.</returns>
        public TQuery WhereStartsWith<TModel>(string propertyName, string value, string alias = null)
        {
            whereClause.AddCondition(CreateAdtScalarBinaryOperatorCondition<TModel>(propertyName, value, typeof(TModel), alias, ScalarOperators.STARTSWITH));

            return (TQuery)this;
        }

        /// <summary>
        /// Add a Ends with where clause.
        /// </summary>
        /// <typeparam name="TModel">Model type.</typeparam>
        /// <param name="propertySelector">Expression to select property of TModel type.</param>
        /// <param name="value">String value to compare.</param>
        /// <param name="alias">Optional - Model Alias.</param>
        /// <returns>ADT query instance.</returns>
        public TQuery WhereEndsWith<TModel>(Expression<Func<TModel, object>> propertySelector, string value, string alias = null)
            where TModel : BasicDigitalTwin
        {
            QueryValidator.ExtractModelAndPropertyName(propertySelector, out Type type, out string propertyName);
            whereClause.AddCondition(CreateAdtScalarBinaryOperatorCondition<TModel>(propertyName, value, type, alias, ScalarOperators.ENDSWITH));

            return (TQuery)this;
        }

        /// <summary>
        /// Add a Ends with where clause.
        /// </summary>
        /// <typeparam name="TModel">Model type.</typeparam>
        /// <param name="propertyName">String property name of TModel type on which where condition is applied on.</param>
        /// <param name="value">String value to compare.</param>
        /// <param name="alias">Optional - Model Alias.</param>
        /// <returns>ADT query instance.</returns>
        public TQuery WhereEndsWith<TModel>(string propertyName, string value, string alias = null)
        {
            whereClause.AddCondition(CreateAdtScalarBinaryOperatorCondition<TModel>(propertyName, value, typeof(TModel), alias, ScalarOperators.ENDSWITH));

            return (TQuery)this;
        }

        /// <summary>
        /// Add a Contains where clause.
        /// </summary>
        /// <typeparam name="TModel">Model type.</typeparam>
        /// <param name="propertySelector">Expression to select property of TModel type.</param>
        /// <param name="value">String value to compare.</param>
        /// <param name="alias">Optional - Model Alias.</param>
        /// <returns>ADT query instance.</returns>
        public TQuery WhereContains<TModel>(Expression<Func<TModel, object>> propertySelector, string value, string alias = null)
            where TModel : BasicDigitalTwin
        {
            QueryValidator.ExtractModelAndPropertyName(propertySelector, out Type type, out string propertyName);
            whereClause.AddCondition(CreateAdtScalarBinaryOperatorCondition<TModel>(propertyName, value, type, alias, ScalarOperators.CONTAINS));

            return (TQuery)this;
        }

        /// <summary>
        /// Add a Contains where clause.
        /// </summary>
        /// <typeparam name="TModel">Model type.</typeparam>
        /// <param name="propertyName">String property name of TModel type on which where condition is applied on.</param>
        /// <param name="value">String value to compare.</param>
        /// <param name="alias">Optional - Model Alias.</param>
        /// <returns>ADT query instance.</returns>
        public TQuery WhereContains<TModel>(string propertyName, string value, string alias = null)
        {
            whereClause.AddCondition(CreateAdtScalarBinaryOperatorCondition<TModel>(propertyName, value, typeof(TModel), alias, ScalarOperators.CONTAINS));

            return (TQuery)this;
        }

        /// <summary>
        /// Adds an OR block of conditions.
        /// </summary>
        /// <param name="conditions">A lambda expression chaining OR-ed where conditions.</param>
        /// <returns>ADT query instance.</returns>
        public TQuery Or(Action<FilteredQuery<TQuery>> conditions)
        {
            var tempQuery = (TQuery)MemberwiseClone();
            tempQuery.whereClause = new WhereClause();
            conditions.Invoke(tempQuery);
            whereClause.AddCondition(new OrCondition
            {
                Conditions = tempQuery.whereClause.Conditions
            });

            return (TQuery)this;
        }

        /// <summary>
        /// Adds an AND block of conditions.
        /// </summary>
        /// <param name="conditions">A lambda expression chaining AND-ed where conditions.</param>
        /// <returns>ADT query instance.</returns>
        public TQuery And(Action<FilteredQuery<TQuery>> conditions)
        {
            var tempQuery = (TQuery)MemberwiseClone();
            tempQuery.whereClause = new WhereClause();
            conditions.Invoke(tempQuery);
            whereClause.AddCondition(new AndCondition
            {
                Conditions = tempQuery.whereClause.Conditions
            });

            return (TQuery)this;
        }

        /// <summary>
        /// Adds a NOT block of conditions (NAND). If more than one condition is provided, they're all AND-ed inside the NOT block.
        /// </summary>
        /// <param name="conditions">A lambda expression chaining NAND-ed where conditions.</param>
        /// <returns>ADT query instance.</returns>
        public TQuery Not(Action<FilteredQuery<TQuery>> conditions)
        {
            var tempQuery = (TQuery)MemberwiseClone();
            tempQuery.whereClause = new WhereClause();
            conditions.Invoke(tempQuery);
            if (tempQuery.whereClause.Conditions.Count > 1)
            {
                whereClause.AddCondition(new NotCondition
                {
                    Condition = new AndCondition
                    {
                        Conditions = tempQuery.whereClause.Conditions
                    }.ToString()
                });
            }
            else if (tempQuery.whereClause.Conditions.Count == 1)
            {
                whereClause.AddCondition(new NotCondition
                {
                    Condition = tempQuery.whereClause.Conditions.First()
                });
            }

            return (TQuery)this;
        }

        private WhereScalarFunctionCondition CreateWhereScalarCondition<TModel>(string propertyName, ScalarOperator op, Type type, string alias)
        {
            var modelAlias = ValidateAndGetAlias<TModel>(type, alias);

            return new WhereScalarFunctionCondition
            {
                Column = propertyName,
                ScalarFunction = op,
                Alias = modelAlias
            };
        }

        internal WhereComparisonCondition CreateWhereComparisonCondition<TModel>(string propertyName, ComparisonOperators op, object value, Type type, string alias)
        {
            var modelAlias = ValidateAndGetAlias<TModel>(type, alias);
            if (value is Enum e)
            {
                value = GetEnumValue(e);
            }

            return new WhereComparisonCondition
            {
                Column = propertyName,
                Value = value,
                Operator = op,
                Alias = modelAlias
            };
        }

        private WhereInCondition CreateWhereInCondition<TModel>(string propertyName, string[] values, Type type, string alias)
        {
            var modelAlias = ValidateAndGetAlias<TModel>(type, alias);

            return new WhereInCondition
            {
                Column = propertyName,
                Value = values,
                Alias = modelAlias
            };
        }

        private WhereNotInCondition CreateWhereNotInCondition<TModel>(string propertyName, string[] values, Type type, string alias)
        {
            var modelAlias = ValidateAndGetAlias<TModel>(type, alias);

            return new WhereNotInCondition
            {
                Column = propertyName,
                Value = values,
                Alias = modelAlias
            };
        }

        private WhereIsOfModelCondition CreateWhereIsOfModelCondition<TBase, TDerived>(string alias = null)
            where TBase : BasicDigitalTwin
            where TDerived : BasicDigitalTwin
        {
            var modelAlias = ValidateAndGetAlias<TBase>(typeof(TBase), alias);
            var model = Activator.CreateInstance<TDerived>().Metadata.ModelId;

            return new WhereIsOfModelCondition(modelAlias, model);
        }

        private WhereScalarFunctionCondition CreateAdtScalarBinaryOperatorCondition<TModel>(string propertyName, string value, Type type, string alias, ScalarOperator binaryOperator)
        {
            var modelAlias = ValidateAndGetAlias<TModel>(type, alias);

            return new WhereScalarFunctionCondition
            {
                Column = propertyName,
                Alias = modelAlias,
                Value = value,
                ScalarFunction = binaryOperator
            };
        }

        private string ValidateAndGetAlias<TModel>(Type type, string alias)
        {
            QueryValidator.ValidateAlias(alias, aliasToTypeMapping);
            QueryValidator.ValidateType<TModel>(Types);
            return string.IsNullOrEmpty(alias) ? GetAssignedAlias(type) : alias;
        }

        private static string GetEnumValue(Enum value)
        {
            var enumName = value.GetType().GetEnumName(value);
            var enumField = value.GetType().GetField(enumName);
            var customValue = enumField.GetCustomAttribute<EnumMemberAttribute>();
            var defaultValue = Convert.ChangeType(value, value.GetTypeCode());
            return customValue is null ? defaultValue.ToString() : customValue.Value;
        }

        private void AddConditions<TModel>(Expression<Func<TModel, bool>> expression)
            where TModel : BasicDigitalTwin
        {
            var linqExpressionParser = new LinqExpressionParser<TModel>(expression);
            if (linqExpressionParser.IsLogical)
            {
                var compoundCondition = CreateCompoundCondition(linqExpressionParser.LogicalExpressionType, true);
                CompileCompoundConditions<TModel>(linqExpressionParser, compoundCondition);
                whereClause.AddCondition(compoundCondition);
            }
            else
            {
                var simpleCondition = CompileSimpleCondition<TModel>(linqExpressionParser);
                whereClause.AddCondition(simpleCondition);
            }
        }

        private Condition CompileSimpleCondition<TModel>(LinqExpressionParser<TModel> linqExpressionParser)
            where TModel : BasicDigitalTwin
        {
            var propertyName = linqExpressionParser.PropertyName;
            var value = linqExpressionParser.Value;
            var scalarOperator = linqExpressionParser.ScalarOperator;
            var comparisonOperator = linqExpressionParser.ComparisonOperator;
            if (!linqExpressionParser.IsScalar)
            {
                return CreateWhereComparisonCondition<TModel>(propertyName, comparisonOperator, value, typeof(TModel), null);
            }

            if (scalarOperator is ScalarUnaryOperator unaryOperator)
            {
                if (unaryOperator == ScalarOperators.IS_NULL && linqExpressionParser.ComparisonOperator == ComparisonOperators.NotEqualTo)
                {
                    return new NotCondition
                    {
                        Condition = CreateWhereScalarCondition<TModel>(propertyName, unaryOperator, typeof(TModel), null).ToString()
                    };
                }

                return CreateWhereScalarCondition<TModel>(propertyName, unaryOperator, typeof(TModel), null);
            }

            // scalar binary operator
            return CreateAdtScalarBinaryOperatorCondition<TModel>(propertyName, (string)value, typeof(TModel), null, scalarOperator);
        }

        private void CompileCompoundConditions<TModel>(LinqExpressionParser<TModel> linqExpressionParser, CompoundCondition compoundCondition)
            where TModel : BasicDigitalTwin
        {
            var logicalExpressionTypes = linqExpressionParser.LogicalExpressionTypes;
            var expressionType = linqExpressionParser.LogicalExpressionType;
            var leftExpression = linqExpressionParser.LeftLogicalExpression;
            var rightExpression = linqExpressionParser.RightLogicalExpression;
            var leftLogicalExpressionParser = new LinqExpressionParser<TModel>(linqExpressionParser.LeftLogicalExpression);
            var rightLogicalExpressionParser = new LinqExpressionParser<TModel>(linqExpressionParser.RightLogicalExpression);
            var leftExpressionType = leftLogicalExpressionParser.LogicalExpressionType;
            var rightExpressionType = rightLogicalExpressionParser.LogicalExpressionType;

            if (logicalExpressionTypes.Contains(leftExpressionType) && !logicalExpressionTypes.Contains(rightExpressionType))
            {
                // left is logical, right is simple
                CompileChildCompoundExpression(linqExpressionParser, leftLogicalExpressionParser, compoundCondition);
                compoundCondition.Conditions.Add(CompileSimpleCondition<TModel>(rightLogicalExpressionParser).ToString());
            }
            else if (logicalExpressionTypes.Contains(rightExpressionType) && !logicalExpressionTypes.Contains(leftExpressionType))
            {
                // right is logical, left is simple
                compoundCondition.Conditions.Add(CompileSimpleCondition<TModel>(leftLogicalExpressionParser).ToString());
                CompileChildCompoundExpression(linqExpressionParser, rightLogicalExpressionParser, compoundCondition);
            }
            else if (logicalExpressionTypes.Contains(leftExpressionType) && logicalExpressionTypes.Contains(rightExpressionType))
            {
                // both are logical
                CompileChildCompoundExpression(linqExpressionParser, leftLogicalExpressionParser, compoundCondition);
                CompileChildCompoundExpression(linqExpressionParser, rightLogicalExpressionParser, compoundCondition);
            }
            else
            {
                // add simple comdition to conditions
                compoundCondition.Conditions.Add(CompileSimpleCondition<TModel>(leftLogicalExpressionParser).ToString());
                compoundCondition.Conditions.Add(CompileSimpleCondition<TModel>(rightLogicalExpressionParser).ToString());
            }
        }

        private void CompileChildCompoundExpression<TModel>(LinqExpressionParser<TModel> parentExpressionParser, LinqExpressionParser<TModel> childExpressionParser, CompoundCondition compoundCondition)
            where TModel : BasicDigitalTwin
        {
            if (childExpressionParser.LogicalExpressionType == parentExpressionParser.LogicalExpressionType)
            {
                CompileCompoundConditions(childExpressionParser, compoundCondition);
            }
            else
            {
                CompoundCondition childNodeCompoundCondition = CreateCompoundCondition(childExpressionParser.LogicalExpressionType, false);
                CompileCompoundConditions(childExpressionParser, childNodeCompoundCondition);
                compoundCondition.Conditions.Add(childNodeCompoundCondition.ToString());
            }
        }

        private static CompoundCondition CreateCompoundCondition(ExpressionType expressionType, bool isRootCompoundCondition)
        {
            return expressionType == ExpressionType.AndAlso ?
            new AndCondition
            {
                Conditions = new List<string>(),
                IsRoot = isRootCompoundCondition
            }
            : new OrCondition
            {
                Conditions = new List<string>(),
                IsRoot = isRootCompoundCondition
            };
        }
    }
}