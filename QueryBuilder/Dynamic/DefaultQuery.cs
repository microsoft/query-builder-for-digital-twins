// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Dynamic
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Clauses;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Statements;

    /// <summary>
    /// A default query that has no count, top, or select clauses.
    /// </summary>
    public class DefaultQuery<TWhereStatement> : JoinQuery<DefaultQuery<TWhereStatement>, TWhereStatement>
    where TWhereStatement : WhereBaseStatement<TWhereStatement>
    {
        internal DefaultQuery(string rootTwinAlias, IList<string> allowedAliases, SelectClause selectClause, FromClause fromClause, IList<JoinClause> joinClauses, WhereClause whereClause) : base(rootTwinAlias, allowedAliases, selectClause, fromClause, joinClauses, whereClause)
        {
        }

        /// <summary>
        /// Overrides the default SELECT statement with a custom select alias or aliases.
        /// </summary>
        /// <param name="aliases">Optional: One or more aliases to apply to the SELECT clause.</param>
        /// <returns>A query instance with one SELECT clause.</returns>
        public Query<TWhereStatement> Select(params string[] aliases)
        {
            ClearSelects();
            foreach (var name in aliases.Where(n => !string.IsNullOrWhiteSpace(n)))
            {
                ValidateAndAddSelect(name);
            }

            return new Query<TWhereStatement>(RootTwinAlias, allowedAliases, selectClause, fromClause, joinClauses, whereClause);
        }

        /// <summary>
        /// Select Top(N) records.
        /// </summary>
        /// <param name="numberOfRecords">Postive number.</param>
        /// <returns>The ADT Query with TOP clause.</returns>
        public DefaultSelectQuery<TWhereStatement> Top(ushort numberOfRecords)
        {
            selectClause.NumberOfRecords = numberOfRecords;
            return new DefaultSelectQuery<TWhereStatement>(RootTwinAlias, allowedAliases, selectClause, fromClause, joinClauses, whereClause);
        }

        /// <summary>
        /// Count records.
        /// </summary>
        /// <returns>The ADT Query with the Count clause added.</returns>
        public CountQuery<TWhereStatement> Count()
        {
            return new CountQuery<TWhereStatement>(RootTwinAlias, allowedAliases, new CountClause(), fromClause, joinClauses, whereClause);
        }
    }

    /// <summary>
    /// A default query that has no count, top, or select clauses and does not support Joins.
    /// </summary>
    /// <typeparam name="TWhereStatement">The type of WHERE statement supported.</typeparam>
    public class DefaultNonJoinQuery<TWhereStatement> : FilteredQuery<DefaultNonJoinQuery<TWhereStatement>, TWhereStatement>
        where TWhereStatement : WhereBaseStatement<TWhereStatement>
    {
        internal DefaultNonJoinQuery(string rootTwinAlias, IList<string> allowedAliases, SelectClause selectClause, FromClause fromClause, IList<JoinClause> joinClauses, WhereClause whereClause) : base(rootTwinAlias, allowedAliases, selectClause, fromClause, joinClauses, whereClause)
        {
        }

        /// <summary>
        /// Overrides the default SELECT statement with a custom select alias or aliases.
        /// Because relationships cannot join on anything, the Select method narrows to specific relationship properties.
        /// </summary>
        /// <param name="propertyNames">Optional: One or more relationship properties to apply to the SELECT clause.</param>
        /// <returns>A query instance with one SELECT clause.</returns>
        public NonJoinQuery<TWhereStatement> Select(params string[] propertyNames)
        {
            ClearSelects();
            foreach (var name in propertyNames.Where(n => !string.IsNullOrWhiteSpace(n)))
            {
                var alias = name == RootTwinAlias ? name : $"{RootTwinAlias}.{name}";
                ValidateAndAddSelect(alias);
            }

            return new NonJoinQuery<TWhereStatement>(RootTwinAlias, allowedAliases, selectClause, fromClause, joinClauses, whereClause);
        }

        /// <summary>
        /// Select Top(N) records.
        /// </summary>
        /// <param name="numberOfRecords">Postive number.</param>
        /// <returns>The ADT Query with TOP clause.</returns>
        public DefaultSelectNonJoinQuery<TWhereStatement> Top(ushort numberOfRecords)
        {
            selectClause.NumberOfRecords = numberOfRecords;
            return new DefaultSelectNonJoinQuery<TWhereStatement>(RootTwinAlias, allowedAliases, selectClause, fromClause, joinClauses, whereClause);
        }

        /// <summary>
        /// Count records.
        /// </summary>
        /// <returns>The ADT Query with the Count clause added.</returns>
        public CountQuery<TWhereStatement> Count()
        {
            return new CountQuery<TWhereStatement>(RootTwinAlias, allowedAliases, new CountClause(), fromClause, joinClauses, whereClause);
        }
    }
}