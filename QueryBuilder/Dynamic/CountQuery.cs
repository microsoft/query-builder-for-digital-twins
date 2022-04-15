// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Dynamic
{
    using System.Collections.Generic;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Clauses;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Dynamic.Statements;

    /// <summary>
    /// A query that has a count select clause.
    /// </summary>
    public class CountQuery<TWhereStatement> : JoinQuery<CountQuery<TWhereStatement>, TWhereStatement>
    where TWhereStatement : WhereBaseStatement<TWhereStatement>
    {
        internal CountQuery(string rootAlias, IList<string> definedAliases, SelectClause select, FromClause fromClause, IList<JoinClause> joins, WhereClause where) : base(rootAlias, definedAliases, select, fromClause, joins, where)
        {
        }
    }
}