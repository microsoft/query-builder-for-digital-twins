// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Dynamic.Statements
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Clauses;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Helpers;

    /// <summary>
    /// An abstract class that provides the base layer of methods that are common between
    /// WHERE statement classes that are applicable to either Twins or Relationships.
    /// </summary>
    /// <typeparam name="TWhereStatement">The type of WHERE statement supported.</typeparam>
    public abstract class WhereBaseStatement<TWhereStatement> where TWhereStatement : WhereBaseStatement<TWhereStatement>
    {
        /// <summary>
        /// The alias for the current WHERE statement.
        /// </summary>
        protected readonly string Alias;
        internal readonly WhereClause WhereClause;
        internal IList<JoinClause> JoinClauses;

        [ExcludeFromCodeCoverage]
        internal WhereBaseStatement()
        {
        }

        internal WhereBaseStatement(IList<JoinClause> clauses, WhereClause clause, string alias)
        {
            JoinClauses = clauses;
            WhereClause = clause;
            Alias = alias;
        }

        /// <summary>
        /// Wraps a given set of WHERE conditions in parenthesis to establish a precedence.
        /// </summary>
        /// <param name="nested">The functional logic of the WHERE statement containing one or more WHERE conditions.</param>
        /// <returns>A conjunction class that supports appending more conditions to the WHERE statements via OR or AND terms.</returns>
        public CompoundWhereStatement<TWhereStatement> Precedence(Func<TWhereStatement, CompoundWhereStatement<TWhereStatement>> nested)
        {
            var statement = WhereStatementFactory.CreateInstance<TWhereStatement>(JoinClauses, new WhereClause(), Alias);
            var n = nested.Invoke(statement);
            WhereClause.AddCondition($"({n.WhereClause.GetConditionsText()})");
            return new CompoundWhereStatement<TWhereStatement>(JoinClauses, WhereClause, Alias);
        }

        /// <summary>
        /// Prepends a NOT term to one or more WHERE clauses.
        /// </summary>
        /// <returns>The WHERE statement implementation supported in this statement. I.e. Either for twins or relationships.</returns>
        public TWhereStatement Not()
        {
            WhereClause.AddCondition(Terms.Not);
            return WhereStatementFactory.CreateInstance<TWhereStatement>(JoinClauses, WhereClause, Alias);
        }
    }
}