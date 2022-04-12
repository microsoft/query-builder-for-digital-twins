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
        internal static TWhereStatement CreateInstance<TWhereStatement>(IEnumerable<JoinOptions> joinOptions, WhereClause clause, string alias) where TWhereStatement : WhereBaseStatement<TWhereStatement>
        {
            var type = typeof(TWhereStatement);
            if (type == typeof(RelationshipsWhereStatement))
            {
                return new RelationshipsWhereStatement(joinOptions, clause, alias) as TWhereStatement;
            }

            if (type == typeof(TwinsWhereStatement))
            {
                return new TwinsWhereStatement(joinOptions, clause, alias) as TWhereStatement;
            }

            throw new Exception($"Unsupported generic type passed in for {nameof(TWhereStatement)}");
        }

        internal static TWhereStatement CreateInstance<TWhereStatement>(WhereClause clause, string alias) where TWhereStatement : WhereBaseStatement<TWhereStatement>
        {
            var type = typeof(TWhereStatement);
            if (type == typeof(RelationshipsWhereStatement))
            {
                return new RelationshipsWhereStatement(clause, alias) as TWhereStatement;
            }

            if (type == typeof(TwinsWhereStatement))
            {
                return new TwinsWhereStatement(clause, alias) as TWhereStatement;
            }

            throw new Exception($"Unsupported generic type passed in for {nameof(TWhereStatement)}");
        }
    }
}