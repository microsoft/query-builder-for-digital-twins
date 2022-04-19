// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text.Json.Serialization;
    using global::Azure.DigitalTwins.Core;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Clauses;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Helpers;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Dynamic;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Dynamic.Statements;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Typed;
    using static Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Helpers.Terms;

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
            var rootAlias = string.IsNullOrEmpty(alias) ? AliasHelper.ExtractAliasFromType(typeof(TModel)) : alias;
            var aliasTypeMapping = new Dictionary<string, Type>
            {
                { rootAlias, typeof(TModel) }
            };

            var fromClause = new FromClause
            {
                Alias = rootAlias
            };

            var model = Activator.CreateInstance<TModel>().Metadata.ModelId;
            var whereClause = new WhereClause();
            whereClause.AddCondition(new WhereIsOfModelCondition
            {
                Alias = rootAlias,
                Model = model
            });

            var selectClause = new SelectClause();
            selectClause.Add(rootAlias);
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

        /// <summary>
        /// Sets the root of the query (the FROM clause) and adds a select clause.
        /// </summary>
        /// <param name="alias">Optional: alias that will override the default root alias of 'twin'.</param>
        /// <returns>A query for querying twins with 'JOIN' and 'WHERE' conditions and a single 'SELECT'.</returns>
        public static TwinDefaultQuery<TwinsWhereStatement> FromTwins(string alias = DefaultTwinAlias)
        {
            var fromClause = new FromClause
            {
                Alias = alias
            };

            var selectClause = new SelectClause();
            selectClause.Add(alias);
            var allowedSelects = new List<string> { alias };
            return new TwinDefaultQuery<TwinsWhereStatement>(alias, allowedSelects, selectClause, fromClause, new List<JoinClause>(), new WhereClause());
        }

        /// <summary>
        /// Sets the root of the query (the FROM clause) and adds a select clause.
        /// </summary>
        /// <param name="alias">Optional: alias that will override the default root alias of 'relationship'.</param>
        /// <returns>A query for querying relationship with 'WHERE' conditions and a single 'SELECT'.</returns>
        public static RelationshipDefaultQuery<RelationshipsWhereStatement> FromRelationships(string alias = DefaultRelationshipAlias)
        {
            var fromClause = new FromClause
            {
                Alias = alias,
                Collection = Relationships
            };

            var selectClause = new SelectClause();
            selectClause.Add(alias);
            var allowedSelects = new List<string> { alias };
            foreach (var prop in GetRelationshipPropertyKeys())
            {
                allowedSelects.Add($"{alias}.{prop}");
            }

            return new RelationshipDefaultQuery<RelationshipsWhereStatement>(alias, allowedSelects, selectClause, fromClause, new List<JoinClause>(), new WhereClause());
        }

        private static IEnumerable<string> GetRelationshipPropertyKeys()
        {
            return typeof(BasicRelationship).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .Where(p => !Attribute.IsDefined(p, typeof(JsonIgnoreAttribute)))
                    .Select(ConvertPropertyName);
        }

        private static string ConvertPropertyName(PropertyInfo prop)
        {
            return prop.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name ?? prop.Name;
        }
    }
}