// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder
{
    using System;
    using System.Collections.Generic;
    using global::Azure.DigitalTwins.Core;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Clauses;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Helpers;

    /// <summary>
    /// A factory class used to create ADT query builder.
    /// </summary>
    public static class QueryBuilder
    {
        /// <summary>
        /// Sets the root type of the query (the FROM clause), filters the correct type, and adds a select clause.
        /// </summary>
        /// <typeparam name="TModel">The root type of the query.</typeparam>
        /// <param name="alias">Optional string alias to map to the root type.</param>
        /// <returns>A query with single select.</returns>
        public static DefaultQuery<TModel> From<TModel>(string alias = null)
            where TModel : BasicDigitalTwin
        {
            var rootTwinAlias = string.IsNullOrEmpty(alias) ? AliasHelper.ExtractAliasFromType(typeof(TModel)) : alias;

            var aliasTypeMapping = new Dictionary<string, Type>
            {
                { rootTwinAlias, typeof(TModel) }
            };

            var fromClause = new FromClause
            {
                Alias = rootTwinAlias
            };

            var model = Activator.CreateInstance<TModel>().Metadata.ModelId;

            var whereClause = new WhereClause();
            if (!typeof(TModel).Equals(typeof(BasicDigitalTwin)))
            {
                whereClause.AddCondition(ConditionHelper.CreateWhereIsOfModelCondition(rootTwinAlias, model));
            }

            var selectClause = new SelectClause();
            selectClause.Add(rootTwinAlias);

            return new DefaultQuery<TModel>(aliasTypeMapping, selectClause, fromClause, new List<JoinClause>(), whereClause);
        }

        /// <summary>
        /// Creates a query to count all digital twins.
        /// </summary>
        /// <returns>A query that counts all digital twins.</returns>
        public static CountAllQuery CountAllDigitalTwins()
        {
            return new CountAllQuery();
        }
    }
}