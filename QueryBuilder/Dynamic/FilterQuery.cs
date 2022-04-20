// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Dynamic
{
    using System;
    using System.Collections.Generic;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Clauses;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Helpers;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Dynamic.Statements;

    /// <summary>
    /// A wrapper for the common functionalities of query builder.
    /// </summary>
    public class FilterQuery<TQuery, TWhereStatement> : DynamicQueryBase
        where TQuery : FilterQuery<TQuery, TWhereStatement>
        where TWhereStatement : WhereBaseStatement<TWhereStatement>
    {
        internal FilterQuery(string rootAlias, IList<string> definedAliases, SelectClause selectClause, FromClause fromClause, IList<JoinClause> joinClauses, WhereClause whereClause) : base(rootAlias, definedAliases, selectClause, fromClause, joinClauses, whereClause)
        {
        }

        /// <summary>
        /// A method to add WHERE conditions to the query.
        /// </summary>
        /// <param name="whereLogic">The functional logic of the WHERE statement containing one or more WHERE conditions.</param>
        /// <returns>An extendible part of a WHERE statement to continue adding WHERE conditions to.</returns>
        public TQuery Where(Func<TWhereStatement, CompoundWhereStatement<TWhereStatement>> whereLogic)
        {
            var statement = WhereStatementFactory.CreateInstance<TWhereStatement>(joinClauses, whereClause, RootAlias);
            whereLogic.Invoke(statement);
            return (TQuery)this;
        }
    }
}