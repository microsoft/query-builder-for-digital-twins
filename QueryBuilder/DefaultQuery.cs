// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.Azure.DigitalTwins.QueryBuilder
{
    using System;
    using System.Collections.Generic;
    using global::Azure.DigitalTwins.Core;
    using Microsoft.Azure.DigitalTwins.QueryBuilder.Clauses;
    using Microsoft.Azure.DigitalTwins.QueryBuilder.Helpers;

    /// <summary>
    /// A default query that has no count, top, or select clauses.
    /// </summary>
    public class DefaultQuery<T> : JoinQuery<DefaultQuery<T>>
        where T : BasicDigitalTwin
    {
        internal DefaultQuery(IDictionary<string, Type> aliasToTypeMapping, SelectClause selectClause, FromClause fromClause, IList<JoinClause> joinClauses, WhereClause whereClause) : base(aliasToTypeMapping, selectClause, fromClause, joinClauses, whereClause)
        {
        }

        /// <summary>
        /// Select a type and optionally provide an alias.
        /// </summary>
        /// <typeparam name="TSelect">The type to be selected.</typeparam>
        /// <param name="alias">Optional alias to map to the selected type.</param>
        /// <returns>ADT query instance with one select clause.</returns>
        public Query<TSelect> Select<TSelect>(string alias = null)
            where TSelect : BasicDigitalTwin
        {
            QueryValidator.ValidateType<TSelect>(GetTypes());

            ClearSelects();

            ValidateAndAddSelect<TSelect>(alias);
            return new Query<TSelect>(aliasToTypeMapping, selectClause, fromClause, joinClauses, whereClause);
        }

        /// <summary>
        /// Select Top(N) records.
        /// </summary>
        /// <param name="numberOfRecords">Postive number.</param>
        /// <returns>The ADT Query with TOP clause.</returns>
        public DefaultSelectQuery<T> Top(ushort numberOfRecords)
        {
            selectClause.NumberOfRecords = numberOfRecords;
            return new DefaultSelectQuery<T>(aliasToTypeMapping, selectClause, fromClause, joinClauses, whereClause);
        }

        /// <summary>
        /// Count records.
        /// </summary>
        /// <returns>The ADT Query with the Count clause added.</returns>
        public CountQuery Count()
        {
            return new CountQuery(aliasToTypeMapping, new CountClause(), fromClause, joinClauses, whereClause);
        }
    }
}