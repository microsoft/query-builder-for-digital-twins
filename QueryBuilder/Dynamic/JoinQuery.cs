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
    public abstract class JoinQuery<TQuery, TWhereStatement> : FilteredQuery<TQuery, TWhereStatement>
        where TQuery : JoinQuery<TQuery, TWhereStatement>
        where TWhereStatement : WhereBaseStatement<TWhereStatement>
    {
        private readonly JoinWithStatement<TWhereStatement> joinStatement;

        internal JoinQuery(string rootTwinAlias, IList<string> allowedAliases, SelectClause selectClause, FromClause fromClause, IList<JoinClause> joinClauses, WhereClause whereClause) : base(rootTwinAlias, allowedAliases, selectClause, fromClause, joinClauses, whereClause)
        {
            joinStatement = new JoinWithStatement<TWhereStatement>(whereClause, rootTwinAlias);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="joinLogic"></param>
        /// <returns></returns>
        public TQuery Join(Func<JoinWithStatement<TWhereStatement>, JoinFinalStatement<TWhereStatement>> joinLogic)
        {
            var join = joinLogic.Invoke(joinStatement);
            return Join(join.Options);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="joinLogic"></param>
        /// <returns></returns>
        public TQuery Join(Func<JoinWithStatement<TWhereStatement>, WhereCombineStatement<TWhereStatement>> joinLogic)
        {
            var joinWithWhere = joinLogic.Invoke(joinStatement);
            return Join(joinWithWhere.JoinOptions);
        }

        private TQuery Join(JoinOptions options)
        {
            if (!allowedAliases.Contains(options.Source))
            {
                throw new ArgumentException($"Alias '{options.Source}' is not assigned!");
            }

            if (allowedAliases.Contains(options.With))
            {
                throw new ArgumentException($"Cannot use the alias: {options.With}, because its already assigned!");
            }

            allowedAliases.Add(options.With);
            if (string.IsNullOrWhiteSpace(options.RelationshipAlias))
            {
                options.RelationshipAlias = $"{options.RelationshipName.ToLowerInvariant()}relationship";
            }

            if (!string.IsNullOrWhiteSpace(options.RelationshipAlias) && allowedAliases.Contains(options.RelationshipAlias))
            {
                throw new ArgumentException($"Cannot use the alias: {options.RelationshipAlias}, because its already assigned!");
            }

            allowedAliases.Add(options.RelationshipAlias);
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