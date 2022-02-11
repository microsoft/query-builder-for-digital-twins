// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.Azure.DigitalTwins.QueryBuilder
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Azure.DigitalTwins.QueryBuilder.Clauses;

    /// <summary>
    /// A query that has a count select clause.
    /// </summary>
    public class CountQuery : JoinQuery<CountQuery>
    {
        internal CountQuery(IDictionary<string, Type> aliasToTypeMapping, SelectClause select, FromClause fromClause, IList<JoinClause> joins, WhereClause where) : base(aliasToTypeMapping, select, fromClause, joins, where)
        {
        }
    }
}