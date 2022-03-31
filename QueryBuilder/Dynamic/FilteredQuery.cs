// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Dynamic
{
    using System;
    using System.Collections.Generic;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Clauses;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Helpers;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Statements;

    /// <summary>
    /// A wrapper for the common functionalities of query builder.
    /// </summary>
    public class FilteredQuery<TQuery, TWhereStatement> : DynamicQueryBase
        where TQuery : FilteredQuery<TQuery, TWhereStatement>
        where TWhereStatement : WhereBaseStatement<TWhereStatement>
    {
        internal FilteredQuery(string rootTwinAlias, IList<string> allowedAliases, SelectClause selectClause, FromClause fromClause, IList<JoinClause> joinClauses, WhereClause whereClause) : base(rootTwinAlias, allowedAliases, selectClause, fromClause, joinClauses, whereClause)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="whereLogic"></param>
        /// <returns></returns>
        public TQuery Where(Func<TWhereStatement, WhereCombineStatement<TWhereStatement>> whereLogic)
        {
            var statement = ActivatorHelper.CreateInstance<TWhereStatement>(whereClause, RootTwinAlias);
            whereLogic.Invoke(statement);
            return (TQuery)this;
        }
    }
}