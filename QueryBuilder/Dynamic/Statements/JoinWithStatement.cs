// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Dynamic.Statements
{
    using System.Collections.Generic;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Clauses;
    using static Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Helpers.Terms;

    /// <summary>
    /// A class that represents the starting point for query JOINS.
    /// </summary>
    /// <typeparam name="TWhereStatement">The type of WHERE statement supported for the JOIN statement.</typeparam>
    public class JoinWithStatement<TWhereStatement> : JoinBaseStatement where TWhereStatement : WhereBaseStatement<TWhereStatement>
    {
        private readonly string source;

        internal JoinWithStatement(IList<JoinClause> joinClauses, WhereClause whereClause, string source = DefaultTwinAlias) : base(joinClauses, whereClause)
        {
            this.source = source;
        }

        /// <summary>
        /// Adds the alias for the twin to join to in the JOIN statement.
        /// </summary>
        /// <param name="with">The alias to use in the statement.</param>
        /// <returns>A statement class with the continuing methods to form the JOIN statement.</returns>
        public JoinRelatedByStatement<TWhereStatement> With(string with)
        {
            Clauses.Add(new JoinClause { JoinWith = with, JoinFrom = source });
            return new JoinRelatedByStatement<TWhereStatement>(Clauses, WhereClause);
        }
    }
}