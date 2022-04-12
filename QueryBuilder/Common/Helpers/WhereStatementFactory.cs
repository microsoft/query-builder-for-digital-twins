// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Clauses;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Statements;

    [ExcludeFromCodeCoverage]
    internal static class WhereStatementFactory
    {
        internal static TWhereStatement CreateInstance<TWhereStatement>(IEnumerable<JoinOptions> joinOptions, WhereClause clause, string alias) where TWhereStatement : WhereBaseStatement<TWhereStatement>
        {
            var type = typeof(TWhereStatement);
            if (type == typeof(RelationshipsWhereStatement))
            {
                return (TWhereStatement)(new RelationshipsWhereStatement(joinOptions, clause, alias) as WhereBaseStatement<TWhereStatement>);
            }

            if (type == typeof(TwinsWhereStatement))
            {
                return (TWhereStatement)(new TwinsWhereStatement(joinOptions, clause, alias) as WhereBaseStatement<TWhereStatement>);
            }

            throw new Exception($"Unsupported generic type passed in for {nameof(TWhereStatement)}");
        }

        internal static TWhereStatement CreateInstance<TWhereStatement>(WhereClause clause, string alias) where TWhereStatement : WhereBaseStatement<TWhereStatement>
        {
            var type = typeof(TWhereStatement);
            if (type == typeof(RelationshipsWhereStatement))
            {
                return (TWhereStatement)(new RelationshipsWhereStatement(clause, alias) as WhereBaseStatement<TWhereStatement>);
            }

            if (type == typeof(TwinsWhereStatement))
            {
                return (TWhereStatement)(new TwinsWhereStatement(clause, alias) as WhereBaseStatement<TWhereStatement>);
            }

            throw new Exception($"Unsupported generic type passed in for {nameof(TWhereStatement)}");
        }
    }
}