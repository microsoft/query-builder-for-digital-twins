// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Dynamic
{
    using System;
    using System.Collections.Generic;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Clauses;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Statements;

    /// <summary>
    /// A wrapper for the common functionalities of query builder.
    /// </summary>
    public abstract class JoinQuery<TQuery, TWhereStatement> : FilterQuery<TQuery, TWhereStatement>
        where TQuery : JoinQuery<TQuery, TWhereStatement>
        where TWhereStatement : WhereBaseStatement<TWhereStatement>
    {
        private readonly JoinWithStatement<TWhereStatement> joinStatement;

        internal JoinQuery(string rootTwinAlias, IList<string> definedAliases, SelectClause selectClause, FromClause fromClause, IList<JoinClause> joinClauses, WhereClause whereClause) : base(rootTwinAlias, definedAliases, selectClause, fromClause, joinClauses, whereClause)
        {
            joinStatement = new JoinWithStatement<TWhereStatement>(whereClause, rootTwinAlias);
        }

        /// <summary>
        /// Adds a JOIN statement to the query.
        /// </summary>
        /// <param name="joinLogic">The functional logic of the JOIN statement containing the required components to join twins via a relationship.</param>
        /// <returns>The query instance.</returns>
        public TQuery Join(Func<JoinWithStatement<TWhereStatement>, JoinFinalStatement<TWhereStatement>> joinLogic)
        {
            var join = joinLogic.Invoke(joinStatement);
            return Join(join.Options);
        }

        /// <summary>
        /// Adds a JOIN statement to the query with an included set of WHERE clauses.
        /// </summary>
        /// <param name="joinAndWhereLogic">The functional logic of the JOIN statement containing the required components to join twins via a relationship.</param>
        /// <returns>The query instance.</returns>
        public TQuery Join(Func<JoinWithStatement<TWhereStatement>, CompoundWhereStatement<TWhereStatement>> joinAndWhereLogic)
        {
            var joinWithWhere = joinAndWhereLogic.Invoke(joinStatement);
            return Join(joinWithWhere.JoinOptions);
        }

        private TQuery Join(JoinOptions options)
        {
            ValidateSelectAlias(options.Source);
            ValidateSelectAlias(options.With);
            definedAliases.Add(options.With);
            if (string.IsNullOrWhiteSpace(options.RelationshipAlias))
            {
                options.RelationshipAlias = $"{options.RelationshipName.ToLowerInvariant()}relationship";
            }

            if (!string.IsNullOrWhiteSpace(options.RelationshipAlias) && definedAliases.Contains(options.RelationshipAlias))
            {
                throw new ArgumentException($"Cannot use the alias: {options.RelationshipAlias}, because its already assigned!");
            }

            definedAliases.Add(options.RelationshipAlias);
            joinClauses.Add(new JoinClause
            {
                JoinWith = options.With,
                Relationship = options.RelationshipName,
                RelationshipAlias = options.RelationshipAlias,
                JoinFrom = options.Source
            });

            return (TQuery)this;
        }
    }
}