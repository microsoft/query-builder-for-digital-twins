// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Clauses;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Dynamic.Statements;

    [ExcludeFromCodeCoverage]
    internal static class WhereStatementFactory
    {
        internal static TWhereStatement CreateInstance<TWhereStatement>(IList<JoinClause> joinClauses, WhereClause clause, string alias) where TWhereStatement : WhereBaseStatement<TWhereStatement>
        {
            var type = typeof(TWhereStatement);
            if (type == typeof(RelationshipsWhereStatement))
            {
                return new RelationshipsWhereStatement(joinClauses, clause, alias) as TWhereStatement;
            }

            if (type == typeof(TwinsWhereStatement))
            {
                return new TwinsWhereStatement(joinClauses, clause, alias) as TWhereStatement;
            }

            throw new Exception($"Unsupported generic type passed in for {nameof(TWhereStatement)}");
        }
    }
}