// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Dynamic.Statements
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Clauses;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Helpers;

    /// <summary>
    /// A conjunction class that supports appending more conditions to the WHERE statements via OR or AND terms.
    /// </summary>
    /// <typeparam name="TWhereStatement">The type of WHERE statement supported.</typeparam>
    public class CompoundWhereStatement<TWhereStatement> where TWhereStatement : WhereBaseStatement<TWhereStatement>
    {
        internal IList<JoinClause> JoinClauses { get; set; }

        internal WhereClause WhereClause { get; private set; }

        private string alias;

        internal CompoundWhereStatement(IList<JoinClause> joinClauses, WhereClause whereClause, string alias)
        {
            JoinClauses = joinClauses;
            WhereClause = whereClause;
            this.alias = alias;
        }

        /// <summary>
        /// Appends an AND term to the current WHERE statement.
        /// </summary>
        /// <returns>he WHERE statement implementation supported in this statement. I.e. Either for twins or relationships.</returns>
        public TWhereStatement And()
        {
            WhereClause.AddCondition(Terms.And);
            return WhereStatementFactory.CreateInstance<TWhereStatement>(JoinClauses, WhereClause, alias);
        }

        /// <summary>
        /// Appends an OR term to the current WHERE statement.
        /// </summary>
        /// <returns>The WHERE statement implementation supported in this statement. I.e. Either for twins or relationships.</returns>
        public TWhereStatement Or()
        {
            WhereClause.AddCondition(Terms.Or);
            return WhereStatementFactory.CreateInstance<TWhereStatement>(JoinClauses, WhereClause, alias);
        }
    }
}