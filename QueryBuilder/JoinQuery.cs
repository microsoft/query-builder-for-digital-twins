// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::Azure.DigitalTwins.Core;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Clauses;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Helpers;

    /// <summary>
    /// A wrapper for the common functionalities of query builder.
    /// </summary>
    public abstract class JoinQuery<TQuery> : FilteredQuery<TQuery>
        where TQuery : JoinQuery<TQuery>
    {
        internal JoinQuery(IDictionary<string, Type> aliasToTypeMapping, SelectClause selectClause, FromClause fromClause, IList<JoinClause> joinClauses, WhereClause whereClause) : base(aliasToTypeMapping, selectClause, fromClause, joinClauses, whereClause)
        {
        }

        /// <summary>
        /// Add join Condition to ADT Query Builder.
        /// </summary>
        /// <typeparam name="TJoinFrom">Model to join From.</typeparam>
        /// <typeparam name="TJoinWith">Model to join with.</typeparam>
        /// <param name="relationshipSelector">A function to select the relationship from the TJoinFrom model.</param>
        /// <returns>ADT query instance.</returns>
        public TQuery Join<TJoinFrom, TJoinWith>(Func<TJoinFrom, BasicRelationship> relationshipSelector)
            where TJoinFrom : BasicDigitalTwin
            where TJoinWith : BasicDigitalTwin
        {
            var joinFromAlias = GetAssignedAlias(typeof(TJoinFrom));
            var joinWithAlias = GenerateTypeAlias(typeof(TJoinWith));
            var relationship = relationshipSelector.Invoke((TJoinFrom)Activator.CreateInstance(typeof(TJoinFrom)));
            var relationshipType = relationship.GetType();
            var relationshipAlias = GenerateTypeAlias(relationshipType);
            return Join<TJoinFrom, TJoinWith>(relationshipSelector, joinFromAlias, joinWithAlias, relationshipAlias);
        }

        /// <summary>
        /// Add join Condition to ADT Query Builder.
        /// </summary>
        /// <typeparam name="TJoinFrom">Model to join From.</typeparam>
        /// <typeparam name="TJoinWith">Model to join with.</typeparam>
        /// <param name="relationshipSelector">A function to select the relationship from the TJoinFrom model.</param>
        /// <param name="joinFromAlias">Query Alias for JoinFrom type.</param>
        /// <param name="joinWithAlias">Query Alias for JoinWith type.</param>
        /// <param name="relationshipAlias">Query Alias for relationship.</param>
        /// <returns>ADT query instance.</returns>
        public TQuery Join<TJoinFrom, TJoinWith>(Func<TJoinFrom, BasicRelationship> relationshipSelector, string joinFromAlias, string joinWithAlias, string relationshipAlias = null)
            where TJoinFrom : BasicDigitalTwin
            where TJoinWith : BasicDigitalTwin
        {
            var relationship = relationshipSelector.Invoke((TJoinFrom)Activator.CreateInstance(typeof(TJoinFrom)));

            if (!aliasToTypeMapping.ContainsKey(joinFromAlias))
            {
                throw new ArgumentException($"Alias '{joinFromAlias}' is not assigned!");
            }

            if (aliasToTypeMapping.ContainsKey(joinWithAlias))
            {
                throw new ArgumentException($"Cannot use the alias: {joinWithAlias}, because its already assigned!");
            }

            aliasToTypeMapping.Add(joinWithAlias, typeof(TJoinWith));

            if (string.IsNullOrEmpty(relationshipAlias))
            {
                relationshipAlias = GenerateTypeAlias(relationship.GetType());
            }
            else
            {
                if (aliasToTypeMapping.ContainsKey(relationshipAlias))
                {
                    throw new ArgumentException($"Cannot use the alias: {relationshipAlias}, because its already assigned!");
                }
            }

            aliasToTypeMapping.Add(relationshipAlias, relationship.GetType());
            joinClauses.Add(new JoinClause
            {
                JoinWith = joinWithAlias,
                Relationship = relationship.Name,
                RelationshipAlias = relationshipAlias,
                JoinFrom = joinFromAlias
            });
            var twinModel = Activator.CreateInstance<TJoinWith>().Metadata.ModelId;
            if (!string.IsNullOrEmpty(twinModel))
            {
                whereClause.AddCondition(new WhereIsOfModelCondition(joinWithAlias, twinModel));
            }

            return (TQuery)this;
        }

        private string GenerateTypeAlias(Type type)
        {
            var alias = AliasHelper.ExtractAliasFromType(type);
            if (aliasToTypeMapping.ContainsKey(alias))
            {
                return $"{alias}{aliasToTypeMapping.Values.Count(v => v.Equals(type))}";
            }

            return alias;
        }
    }
}