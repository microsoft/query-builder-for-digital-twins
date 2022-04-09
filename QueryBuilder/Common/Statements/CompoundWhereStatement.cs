// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Statements
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Clauses;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Helpers;

    /// <summary>
    /// A conjunction class that supports appending more conditions to the WHERE statements via OR or AND terms.
    /// </summary>
    /// <typeparam name="TWhereStatement">The type of WHERE statement supported.</typeparam>
    public class CompoundWhereStatement<TWhereStatement> where TWhereStatement : WhereBaseStatement<TWhereStatement>
    {
        internal JoinOptions JoinOptions { get; private set; }

        internal WhereClause WhereClause { get; private set; }

        private string alias;

        internal CompoundWhereStatement(JoinOptions joinOptions, WhereClause whereClause, string alias)
        {
            JoinOptions = joinOptions;
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
            return WhereStatementFactory.CreateInstance<TWhereStatement>(JoinOptions, WhereClause, alias);
        }

        /// <summary>
        /// Adds one or more WHERE clauses preceded by an AND term to the WHERE statement.
        /// </summary>
        /// <param name="nested">The functional logic of the WHERE statement containing one or more WHERE conditions.</param>
        /// <returns>A conjunction class that supports appending more conditions to the WHERE statements via OR or AND terms.</returns>
        public CompoundWhereStatement<TWhereStatement> And(Func<TWhereStatement, CompoundWhereStatement<TWhereStatement>> nested)
        {
            var statement = WhereStatementFactory.CreateInstance<TWhereStatement>(new WhereClause(), alias);
            var w = nested.Invoke(statement);
            And();
            WhereClause.AddCondition(w.WhereClause.Conditions.FirstOrDefault());
            return this;
        }

        /// <summary>
        /// Appends an OR term to the current WHERE statement.
        /// </summary>
        /// <returns>The WHERE statement implementation supported in this statement. I.e. Either for twins or relationships.</returns>
        public TWhereStatement Or()
        {
            WhereClause.AddCondition(Terms.Or);
            return WhereStatementFactory.CreateInstance<TWhereStatement>(JoinOptions, WhereClause, alias);
        }

        /// <summary>
        /// Adds one or more WHERE clauses preceded by an OR term to the WHERE statement.
        /// </summary>
        /// <param name="nested">The functional logic of the WHERE statement containing one or more WHERE conditions.</param>
        /// <returns>A conjunction class that supports appending more conditions to the WHERE statements via OR or AND terms.</returns>
        public CompoundWhereStatement<TWhereStatement> Or(Func<TWhereStatement, CompoundWhereStatement<TWhereStatement>> nested)
        {
            var statement = WhereStatementFactory.CreateInstance<TWhereStatement>(JoinOptions, new WhereClause(), alias);
            var w = nested.Invoke(statement);
            Or();
            WhereClause.AddCondition(w.WhereClause.Conditions.FirstOrDefault());
            return this;
        }
    }
}