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
    /// Base query for all select queries.
    /// </summary>
    public abstract class SelectQueryBase<TQuery> : JoinQuery<TQuery>
        where TQuery : SelectQueryBase<TQuery>
    {
        internal SelectQueryBase(IDictionary<string, Type> aliasToTypeMapping, SelectClause selectClause, FromClause fromClause, IList<JoinClause> joinClauses, WhereClause whereClause) : base(aliasToTypeMapping, selectClause, fromClause, joinClauses, whereClause)
        {
        }

        /// <summary>
        /// Select Top(N) records.
        /// </summary>
        /// <param name="numberOfRecords">Positive number.</param>
        /// <returns>The ADT Query with TOP clause.</returns>
        public TQuery Top(ushort numberOfRecords)
        {
            selectClause.NumberOfRecords = numberOfRecords;
            return (TQuery)this;
        }
    }

    /// <summary>
    /// The query that has the default select that was generated by the FROM clause.
    /// </summary>
    public class DefaultSelectQuery<T> : SelectQueryBase<DefaultSelectQuery<T>>
    {
        internal DefaultSelectQuery(IDictionary<string, Type> aliasToTypeMapping, SelectClause selectClause, FromClause fromClause, IList<JoinClause> joinClauses, WhereClause whereClauses) : base(aliasToTypeMapping, selectClause, fromClause, joinClauses, whereClauses)
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
    }

    /// <summary>
    /// The query that has one select clause.
    /// </summary>
    /// <typeparam name="T">The selected type.</typeparam>
    public class Query<T> : SelectQueryBase<Query<T>>
    {
        internal Query(IDictionary<string, Type> aliasToTypeMapping, SelectClause selectClause, FromClause fromClause, IList<JoinClause> joinClauses, WhereClause whereClauses) : base(aliasToTypeMapping, selectClause, fromClause, joinClauses, whereClauses)
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
        public Query<T, TOut> Select<TSelect, TOut>(Expression<Func<TSelect, TOut>> propertySelector, string propertyAlias = null, string typeAlias = null)
        {
            QueryValidator.ValidateType<TSelect>(Types);

            ValidateAndAddSelect<TSelect, TOut>(propertySelector, propertyAlias, typeAlias);
            return new Query<T, TOut>(aliasToTypeMapping, selectClause, fromClause, joinClauses, whereClause);
        }

        /// <summary>
        /// Select a type and optionally provide an alias.
        /// </summary>
        /// <typeparam name="TSelect">The type to be selected.</typeparam>
        /// <param name="alias">Optional alias to map to the selected type.</param>
        /// <returns>ADT query instance with two select clauses.</returns>
        public Query<T, TSelect> Select<TSelect>(string alias = null)
        {
            QueryValidator.ValidateType<TSelect>(Types);

            ValidateAndAddSelect<TSelect>(alias);
            return new Query<T, TSelect>(aliasToTypeMapping, selectClause, fromClause, joinClauses, whereClause);
        }
    }

    /// <summary>
    /// The query that has two select clauses.
    /// </summary>
    /// <typeparam name="T1">The first selected type.</typeparam>
    /// <typeparam name="T2">The second selected type.</typeparam>
    public class Query<T1, T2> : SelectQueryBase<Query<T1, T2>>
    {
        internal Query(IDictionary<string, Type> aliasToTypeMapping, SelectClause selectClause, FromClause fromClause, IList<JoinClause> joinClauses, WhereClause whereClauses) : base(aliasToTypeMapping, selectClause, fromClause, joinClauses, whereClauses)
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
        public Query<T1, T2, TOut> Select<TSelect, TOut>(Expression<Func<TSelect, TOut>> propertySelector, string propertyAlias = null, string typeAlias = null)
        {
            QueryValidator.ValidateType<TSelect>(Types);

            ValidateAndAddSelect<TSelect, TOut>(propertySelector, propertyAlias, typeAlias);
            return new Query<T1, T2, TOut>(aliasToTypeMapping, selectClause, fromClause, joinClauses, whereClause);
        }

        /// <summary>
        /// Select a type and optionally provide an alias.
        /// </summary>
        /// <typeparam name="TSelect">The type to be selected.</typeparam>
        /// <param name="alias">Optional alias to map to the selected type.</param>
        /// <returns>ADT query instance with three select clauses.</returns>
        public Query<T1, T2, TSelect> Select<TSelect>(string alias = null)
        {
            QueryValidator.ValidateType<TSelect>(Types);

            ValidateAndAddSelect<TSelect>(alias);
            return new Query<T1, T2, TSelect>(aliasToTypeMapping, selectClause, fromClause, joinClauses, whereClause);
        }
    }

    /// <summary>
    /// The query that has three select clauses.
    /// </summary>
    /// <typeparam name="T1">The first selected type.</typeparam>
    /// <typeparam name="T2">The second selected type.</typeparam>
    /// <typeparam name="T3">The third selected type.</typeparam>
    public class Query<T1, T2, T3> : SelectQueryBase<Query<T1, T2, T3>>
    {
        internal Query(IDictionary<string, Type> aliasToTypeMapping, SelectClause selectClause, FromClause fromClause, IList<JoinClause> joinClauses, WhereClause whereClauses) : base(aliasToTypeMapping, selectClause, fromClause, joinClauses, whereClauses)
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
        public Query<T1, T2, T3, TOut> Select<TSelect, TOut>(Expression<Func<TSelect, TOut>> propertySelector, string propertyAlias = null, string typeAlias = null)
        {
            QueryValidator.ValidateType<TSelect>(Types);

            ValidateAndAddSelect<TSelect, TOut>(propertySelector, propertyAlias, typeAlias);
            return new Query<T1, T2, T3, TOut>(aliasToTypeMapping, selectClause, fromClause, joinClauses, whereClause);
        }

        /// <summary>
        /// Select a type and optionally provide an alias.
        /// </summary>
        /// <typeparam name="TSelect">The type to be selected.</typeparam>
        /// <param name="alias">Optional alias to map to the selected type.</param>
        /// <returns>ADT query instance with four select clauses.</returns>
        public Query<T1, T2, T3, TSelect> Select<TSelect>(string alias = null)
        {
            QueryValidator.ValidateType<TSelect>(Types);

            ValidateAndAddSelect<TSelect>(alias);
            return new Query<T1, T2, T3, TSelect>(aliasToTypeMapping, selectClause, fromClause, joinClauses, whereClause);
        }
    }

    /// <summary>
    /// The query that has four select clauses.
    /// </summary>
    /// <typeparam name="T1">The first selected type.</typeparam>
    /// <typeparam name="T2">The second selected type.</typeparam>
    /// <typeparam name="T3">The third selected type.</typeparam>
    /// <typeparam name="T4">The fourth selected type.</typeparam>
    public class Query<T1, T2, T3, T4> : SelectQueryBase<Query<T1, T2, T3, T4>>
    {
        internal Query(IDictionary<string, Type> aliasToTypeMapping, SelectClause selectClause, FromClause fromClause, IList<JoinClause> joinClauses, WhereClause whereClauses) : base(aliasToTypeMapping, selectClause, fromClause, joinClauses, whereClauses)
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
        public Query<T1, T2, T3, T4, TOut> Select<TSelect, TOut>(Expression<Func<TSelect, TOut>> propertySelector, string propertyAlias = null, string typeAlias = null)
        {
            QueryValidator.ValidateType<TSelect>(Types);

            ValidateAndAddSelect<TSelect, TOut>(propertySelector, propertyAlias, typeAlias);
            return new Query<T1, T2, T3, T4, TOut>(aliasToTypeMapping, selectClause, fromClause, joinClauses, whereClause);
        }

        /// <summary>
        /// Select a type and optionally provide an alias.
        /// </summary>
        /// <typeparam name="TSelect">The type to be selected.</typeparam>
        /// <param name="alias">Optional alias to map to the selected type.</param>
        /// <returns>ADT query instance with five select clauses.</returns>
        public Query<T1, T2, T3, T4, TSelect> Select<TSelect>(string alias = null)
        {
            QueryValidator.ValidateType<TSelect>(Types);

            ValidateAndAddSelect<TSelect>(alias);
            return new Query<T1, T2, T3, T4, TSelect>(aliasToTypeMapping, selectClause, fromClause, joinClauses, whereClause);
        }
    }

    /// <summary>
    /// The query that has five select clauses.
    /// </summary>
    /// <typeparam name="T1">The first selected type.</typeparam>
    /// <typeparam name="T2">The second selected type.</typeparam>
    /// <typeparam name="T3">The third selected type.</typeparam>
    /// <typeparam name="T4">The fourth selected type.</typeparam>
    /// <typeparam name="T5">The fifth selected type.</typeparam>
    public class Query<T1, T2, T3, T4, T5> : SelectQueryBase<Query<T1, T2, T3, T4, T5>>
    {
        internal Query(IDictionary<string, Type> aliasToTypeMapping, SelectClause selectClause, FromClause fromClause, IList<JoinClause> joinClauses, WhereClause whereClauses) : base(aliasToTypeMapping, selectClause, fromClause, joinClauses, whereClauses)
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
        public Query<T1, T2, T3, T4, T5, TOut> Select<TSelect, TOut>(Expression<Func<TSelect, TOut>> propertySelector, string propertyAlias = null, string typeAlias = null)
        {
            QueryValidator.ValidateType<TSelect>(Types);

            ValidateAndAddSelect<TSelect, TOut>(propertySelector, propertyAlias, typeAlias);
            return new Query<T1, T2, T3, T4, T5, TOut>(aliasToTypeMapping, selectClause, fromClause, joinClauses, whereClause);
        }

        /// <summary>
        /// Select a type and optionally provide an alias.
        /// </summary>
        /// <typeparam name="TSelect">The type to be selected.</typeparam>
        /// <param name="alias">Optional alias to map to the selected type.</param>
        /// <returns>ADT query instance with six select clauses.</returns>
        public Query<T1, T2, T3, T4, T5, TSelect> Select<TSelect>(string alias = null)
        {
            QueryValidator.ValidateType<TSelect>(Types);

            ValidateAndAddSelect<TSelect>(alias);
            return new Query<T1, T2, T3, T4, T5, TSelect>(aliasToTypeMapping, selectClause, fromClause, joinClauses, whereClause);
        }
    }

    /// <summary>
    /// The query that has six select clauses.
    /// </summary>
    /// <typeparam name="T1">The first selected type.</typeparam>
    /// <typeparam name="T2">The second selected type.</typeparam>
    /// <typeparam name="T3">The third selected type.</typeparam>
    /// <typeparam name="T4">The fourth selected type.</typeparam>
    /// <typeparam name="T5">The fifth selected type.</typeparam>
    /// <typeparam name="T6">The sixth selected type.</typeparam>
    public class Query<T1, T2, T3, T4, T5, T6> : SelectQueryBase<Query<T1, T2, T3, T4, T5, T6>>
    {
        internal Query(IDictionary<string, Type> aliasToTypeMapping, SelectClause selectClause, FromClause fromClause, IList<JoinClause> joinClauses, WhereClause whereClauses) : base(aliasToTypeMapping, selectClause, fromClause, joinClauses, whereClauses)
        {
        }
    }
}