// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Dynamic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text.Json.Serialization;
    using Azure.DigitalTwins.Core;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Clauses;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Statements;
    using static Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Helpers.Terms;

    /// <summary>
    /// A factory class used to create ADT query builder.
    /// </summary>
    public static class DynamicQueryBuilder
    {
        /// <summary>
        /// Sets the root of the query (the FROM clause) and adds a select clause.
        /// </summary>
        /// <param name="alias">Optional: alias that will override the default root alias of 'twin'.</param>
        /// <returns>A query for querying twins with 'JOIN' and 'WHERE' conditions and a single 'SELECT'.</returns>
        public static DefaultQuery<WhereStatement> FromTwins(string alias = DefaultTwinAlias)
        {
            var fromClause = new FromClause
            {
                Alias = alias
            };

            var selectClause = new SelectClause();
            selectClause.Add(alias);
            var allowedSelects = new List<string> { alias };
            return new DefaultQuery<WhereStatement>(alias, allowedSelects, selectClause, fromClause, new List<JoinClause>(), new WhereClause());
        }

        /// <summary>
        /// Sets the root of the query (the FROM clause) and adds a select clause.
        /// </summary>
        /// <param name="alias">Optional: alias that will override the default root alias of 'relationship'.</param>
        /// <returns>A query for querying relationship with 'WHERE' conditions and a single 'SELECT'.</returns>
        public static DefaultNonJoinQuery<WhereRelationshipsStatement> FromRelationships(string alias = DefaultRelationshipAlias)
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

            return new DefaultNonJoinQuery<WhereRelationshipsStatement>(alias, allowedSelects, selectClause, fromClause, new List<JoinClause>(), new WhereClause());
        }

        /// <summary>
        /// Creates a query to count all digital twins.
        /// </summary>
        /// <returns>A query that counts all digital twins.</returns>
        public static CountAllQuery CountAllDigitalTwins()
        {
            return new CountAllQuery();
        }

        private static IEnumerable<string> GetRelationshipPropertyKeys()
        {
            return typeof(BasicRelationship).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .Where(p => !Attribute.IsDefined(p, typeof(JsonIgnoreAttribute)))
                    .Select(ConvertPropertyName);
        }

        private static string ConvertPropertyName(PropertyInfo prop)
        {
            var attr = prop.GetCustomAttributes(typeof(JsonPropertyNameAttribute), true).FirstOrDefault() as JsonPropertyNameAttribute;
            var propName = attr != null ? attr.Name : default;
            return string.IsNullOrWhiteSpace(propName) ? prop.Name : propName;
        }
    }
}