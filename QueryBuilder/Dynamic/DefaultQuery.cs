// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Dynamic
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Clauses;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Dynamic.Statements;

    /// <summary>
    /// A default query that has supports count, top, or select clauses.
    /// </summary>
    public class TwinDefaultQuery<TWhereStatement> : JoinQuery<TwinDefaultQuery<TWhereStatement>, TWhereStatement>
    where TWhereStatement : WhereBaseStatement<TWhereStatement>
    {
        internal TwinDefaultQuery(string rootTwinAlias, IList<string> definedAliases, SelectClause selectClause, FromClause fromClause, IList<JoinClause> joinClauses, WhereClause whereClause) : base(rootTwinAlias, definedAliases, selectClause, fromClause, joinClauses, whereClause)
        {
        }

        /// <summary>
        /// Overrides the default SELECT statement with a custom select alias or aliases.
        /// </summary>
        /// <param name="aliases">Optional: One or more aliases to apply to the SELECT clause.</param>
        /// <returns>A query instance with one SELECT clause.</returns>
        public TwinQuery<TWhereStatement> Select(params string[] aliases)
        {
            ClearSelects();
            foreach (var name in aliases.Where(n => !string.IsNullOrWhiteSpace(n)))
            {
                ValidateAndAddSelect(name);
            }

            return new TwinQuery<TWhereStatement>(RootTwinAlias, definedAliases, selectClause, fromClause, joinClauses, whereClause);
        }

        /// <summary>
        /// Select Top(N) records.
        /// </summary>
        /// <param name="numberOfRecords">Postive number.</param>
        /// <returns>The ADT Query with TOP clause.</returns>
        public TwinDefaultSelectQuery<TWhereStatement> Top(ushort numberOfRecords)
        {
            selectClause.NumberOfRecords = numberOfRecords;
            return new TwinDefaultSelectQuery<TWhereStatement>(RootTwinAlias, definedAliases, selectClause, fromClause, joinClauses, whereClause);
        }

        /// <summary>
        /// Count records.
        /// </summary>
        /// <returns>The ADT Query with the Count clause added.</returns>
        public CountQuery<TWhereStatement> Count()
        {
            return new CountQuery<TWhereStatement>(RootTwinAlias, definedAliases, new CountClause(), fromClause, joinClauses, whereClause);
        }
    }

    /// <summary>
    /// A default query that has no count, top, or select clauses and does not support Joins.
    /// </summary>
    /// <typeparam name="TWhereStatement">The type of WHERE statement supported.</typeparam>
    public class RelationshipDefaultQuery<TWhereStatement> : FilterQuery<RelationshipDefaultQuery<TWhereStatement>, TWhereStatement>
        where TWhereStatement : WhereBaseStatement<TWhereStatement>
    {
        internal RelationshipDefaultQuery(string rootTwinAlias, IList<string> definedAliases, SelectClause selectClause, FromClause fromClause, IList<JoinClause> joinClauses, WhereClause whereClause) : base(rootTwinAlias, definedAliases, selectClause, fromClause, joinClauses, whereClause)
        {
        }

        /// <summary>
        /// Overrides the default SELECT statement with a custom select alias or aliases.
        /// Because relationships cannot join on anything, the Select method narrows to specific relationship properties.
        /// </summary>
        /// <param name="propertyNames">Optional: One or more relationship properties to apply to the SELECT clause.</param>
        /// <returns>A query instance with one SELECT clause.</returns>
        public RelationshipQuery<TWhereStatement> Select(params string[] propertyNames)
        {
            ClearSelects();
            foreach (var name in propertyNames.Where(n => !string.IsNullOrWhiteSpace(n)))
            {
                var alias = name == RootTwinAlias ? name : $"{RootTwinAlias}.{name}";
                ValidateAndAddSelect(alias);
            }

            return new RelationshipQuery<TWhereStatement>(RootTwinAlias, definedAliases, selectClause, fromClause, joinClauses, whereClause);
        }

        /// <summary>
        /// Select Top(N) records.
        /// </summary>
        /// <param name="numberOfRecords">Postive number.</param>
        /// <returns>The ADT Query with TOP clause.</returns>
        public RelationshipDefaultSelectQuery<TWhereStatement> Top(ushort numberOfRecords)
        {
            selectClause.NumberOfRecords = numberOfRecords;
            return new RelationshipDefaultSelectQuery<TWhereStatement>(RootTwinAlias, definedAliases, selectClause, fromClause, joinClauses, whereClause);
        }

        /// <summary>
        /// Count records.
        /// </summary>
        /// <returns>The ADT Query with the Count clause added.</returns>
        public CountQuery<TWhereStatement> Count()
        {
            return new CountQuery<TWhereStatement>(RootTwinAlias, definedAliases, new CountClause(), fromClause, joinClauses, whereClause);
        }
    }
}