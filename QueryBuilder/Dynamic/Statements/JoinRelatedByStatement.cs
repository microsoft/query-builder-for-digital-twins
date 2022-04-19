// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Dynamic.Statements
{
    using System.Collections.Generic;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Clauses;

    /// <summary>
    /// A class that contains the methods for providing a relationship name as a part of a Join statement.
    /// </summary>
    /// <typeparam name="TWhereStatement">The type of WHERE statement supported for the JOIN statement.</typeparam>
    public class JoinRelatedByStatement<TWhereStatement> : JoinBaseStatement where TWhereStatement : WhereBaseStatement<TWhereStatement>
    {
        internal JoinRelatedByStatement(IList<JoinClause> joinClauses, WhereClause whereClause) : base(joinClauses, whereClause)
        {
        }

        /// <summary>
        /// Adds the name of the relationship to use in the JOIN statement.
        /// </summary>
        /// <param name="relationshipName">The name of the relationship.</param>
        /// <returns>A statement class that contains various unary and binary comparison methods to finalize a JOIN statement.</returns>
        public JoinFinalStatement<TWhereStatement> RelatedBy(string relationshipName)
        {
            Current.Relationship = relationshipName;
            return new JoinFinalStatement<TWhereStatement>(Clauses, WhereClause);
        }
    }
}