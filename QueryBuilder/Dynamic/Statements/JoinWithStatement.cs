// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Dynamic.Statements
{
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Clauses;
    using static Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Helpers.Terms;

    internal class JoinOptions
    {
        internal string With { get; set; }

        internal string Source { get; set; }

        internal string RelationshipName { get; set; }

        internal string RelationshipAlias { get; set; }
    }

    /// <summary>
    /// A class that represents the starting point for query JOINS.
    /// </summary>
    /// <typeparam name="TWhereStatement">The type of WHERE statement supported for the JOIN statement.</typeparam>
    public class JoinWithStatement<TWhereStatement> where TWhereStatement : WhereBaseStatement<TWhereStatement>
    {
        private string source;
        private readonly WhereClause whereClause;

        internal JoinWithStatement(WhereClause whereClause, string source = DefaultTwinAlias)
        {
            this.whereClause = whereClause;
            this.source = source;
        }

        /// <summary>
        /// Adds the alias for the twin to join to in the JOIN statement.
        /// </summary>
        /// <param name="with">The alias to use in the statement.</param>
        /// <returns>A statement class with the continuing methods to form the JOIN statement.</returns>
        public JoinRelatedByStatement<TWhereStatement> With(string with)
        {
            return new JoinRelatedByStatement<TWhereStatement>(whereClause, new JoinOptions { With = with, Source = source });
        }
    }
}