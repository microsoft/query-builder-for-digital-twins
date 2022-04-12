// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Statements
{
    using System.Collections.Generic;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Clauses;

    /// <summary>
    /// A WHERE statement class used for querying relationships.
    /// </summary>
    public class RelationshipsWhereStatement : WhereBaseStatement<RelationshipsWhereStatement>
    {
        internal RelationshipsWhereStatement(IEnumerable<JoinOptions> joinOptions, WhereClause clause, string alias) : base(joinOptions, clause, alias)
        {
        }

        internal RelationshipsWhereStatement(WhereClause clause, string alias) : base(clause, alias)
        {
        }

        /// <summary>
        /// Adds the propertyName to filter against to the WHERE statement.
        /// </summary>
        /// <param name="propertyName">The name of the property to filter against.</param>
        /// <returns>A statement class that contains various unary or binary comparison methods to finalize the WHERE statement.</returns>
        public WherePropertyStatement<RelationshipsWhereStatement> Property(string propertyName)
        {
            return new WherePropertyStatement<RelationshipsWhereStatement>(JoinOptions, WhereClause, propertyName, Alias);
        }
    }
}