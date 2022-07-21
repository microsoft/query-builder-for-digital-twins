// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Typed
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using global::Azure.DigitalTwins.Core;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Clauses;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Helpers;

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
        /// Select a property on a type and optionally provide an alias.
        /// </summary>
        /// <typeparam name="TSelect">The type from which to select a property.</typeparam>
        /// <typeparam name="TOut">The type of the property.</typeparam>
        /// <param name="propertySelector">The property to select on the type.</param>
        /// <param name="propertyAlias">Optional alias to map to the selected property.</param>
        /// <param name="typeAlias">Optional alias to map to the selected type.</param>
        /// <returns>ADT query instance with one select clause.</returns>
        public Query<TOut> Select<TSelect, TOut>(Expression<Func<TSelect, TOut>> propertySelector, string propertyAlias = null, string typeAlias = null)
        {
            QueryValidator.ValidateType<TSelect>(Types);

            ClearSelects();

            ValidateAndAddSelect<TSelect, TOut>(propertySelector, propertyAlias, typeAlias);
            return new Query<TOut>(aliasToTypeMapping, selectClause, fromClause, joinClauses, whereClause);
        }

        /// <summary>
        /// Select a type and optionally provide an alias.
        /// </summary>
        /// <typeparam name="TSelect">The type to be selected.</typeparam>
        /// <param name="alias">Optional alias to map to the selected type.</param>
        /// <returns>ADT query instance with one select clause.</returns>
        public Query<TSelect> Select<TSelect>(string alias = null)
        {
            QueryValidator.ValidateType<TSelect>(Types);

            ClearSelects();

            ValidateAndAddSelect<TSelect>(alias);
            return new Query<TSelect>(aliasToTypeMapping, selectClause, fromClause, joinClauses, whereClause);
        }

        /// <summary>
        /// Select Top(N) records.
        /// </summary>
        /// <param name="numberOfRecords">Positive number.</param>
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