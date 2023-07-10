// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Dynamic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Clauses;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Dynamic.Statements;

    /// <summary>
    /// A wrapper for the common functionalities of query builder.
    /// </summary>
    public abstract class JoinQuery<TQuery, TWhereStatement> : FilterQuery<TQuery, TWhereStatement>
        where TQuery : JoinQuery<TQuery, TWhereStatement>
        where TWhereStatement : WhereBaseStatement<TWhereStatement>
    {
        private readonly JoinWithStatement<TWhereStatement> joinStatement;

        internal JoinQuery(string rootAlias, IList<string> definedAliases, SelectClause selectClause, FromClause fromClause, IList<JoinClause> joinClauses, WhereClause whereClause) : base(rootAlias, definedAliases, selectClause, fromClause, joinClauses, whereClause)
        {
            joinStatement = new JoinWithStatement<TWhereStatement>(new List<JoinClause>(), whereClause, rootAlias);
        }

        /// <summary>
        /// Adds a JOIN statement to the query.
        /// </summary>
        /// <param name="joinLogic">The functional logic of the JOIN statement containing the required components to join twins via a relationship.</param>
        /// <returns>The query instance.</returns>
        public TQuery Join(Func<JoinWithStatement<TWhereStatement>, JoinFinalStatement<TWhereStatement>> joinLogic)
        {
            var computed = joinLogic.Invoke(joinStatement);
            return Join(computed.Clauses);
        }

        /// <summary>
        /// Adds a JOIN statement to the query with an included set of WHERE clauses.
        /// </summary>
        /// <param name="joinAndWhereLogic">The functional logic of the JOIN statement containing the required components to join twins via a relationship.</param>
        /// <returns>The query instance.</returns>
        public TQuery Join(Func<JoinWithStatement<TWhereStatement>, CompoundWhereStatement<TWhereStatement>> joinAndWhereLogic)
        {
            var computed = joinAndWhereLogic.Invoke(joinStatement);
            return Join(computed.JoinClauses);
        }

        private TQuery Join(IList<JoinClause> joinClause)
        {
            foreach (var clause in joinClause)
            {
                if (joinClauses.Any(jc => jc.Id == clause.Id))
                {
                    continue;
                }

                ValidateAliasIsDefined(clause.JoinFrom);
                ValidateAndAddAlias(clause.JoinWith);
                SetRelationshipAliasIfNeeded(clause);
                ValidateAndAddAlias(clause.RelationshipAlias);
                joinClauses.Add(clause);
            }

            return (TQuery)this;
        }

        private void SetRelationshipAliasIfNeeded(JoinClause clause)
        {
            if (string.IsNullOrWhiteSpace(clause.RelationshipAlias))
            {
                clause.RelationshipAlias = $"{clause.Relationship.ToLowerInvariant()}relationship";
            }
        }
    }
}