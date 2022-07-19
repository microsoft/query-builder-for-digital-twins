﻿// Copyright (c) Microsoft Corporation.
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
        /// Select a property on a type.
        /// </summary>
        /// <typeparam name="TSelect">The type to be selected.</typeparam>
        /// <param name="propertySelector">The property to select on the type.</param>
        /// <returns>ADT query instance with one select clause.</returns>
        public Query<TSelect> Select<TSelect>(Expression<Func<TSelect, object>> propertySelector)
        {
            return Select(null, propertySelector);
        }

        /// <summary>
        /// Select a type and optionally provide an alias.
        /// </summary>
        /// <typeparam name="TSelect">The type to be selected.</typeparam>
        /// <param name="alias">Optional alias to map to the selected type.</param>
        /// <param name="propertySelector">Optional property to select on the type.</param>
        /// <returns>ADT query instance with one select clause.</returns>
        public Query<TSelect> Select<TSelect>(string alias = null, Expression<Func<TSelect, object>> propertySelector = null)
        {
            QueryValidator.ValidateType<TSelect>(Types);

            ClearSelects();

            ValidateAndAddSelect<TSelect>(alias, propertySelector);
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