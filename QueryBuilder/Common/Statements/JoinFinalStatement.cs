// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Statements
{
    using System;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Clauses;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Helpers;

    /// <summary>
    /// A class that contains various unary and binary comparison methods to finalize a JOIN statement.
    /// </summary>
    /// <typeparam name="TWhereStatement">The type of WHERE statement supported for the JOIN statement.</typeparam>
    public class JoinFinalStatement<TWhereStatement> where TWhereStatement : WhereBaseStatement<TWhereStatement>
    {
        internal JoinOptions Options { get; private set; }

        private WhereClause whereClause;

        internal JoinFinalStatement(WhereClause whereClause, JoinOptions options)
        {
            this.whereClause = whereClause;
            this.Options = options;
        }

        /// <summary>
        /// Optional: Adds an alias to the JOIN statement to override the default.
        /// This should mostly not be needed, because by default it will use whatever
        /// the default/root alias for the query is, even if a custom one was provided in the FROM clause.
        /// </summary>
        /// <param name="sourceTwin">The alias to override with in the JOIN statement.</param>
        /// <returns>A statement class that contains various unary or binary comparison methods to finalize the JOIN statement.</returns>
        public JoinFinalStatement<TWhereStatement> On(string sourceTwin)
        {
            Options.Source = sourceTwin;
            return this;
        }

        /// <summary>
        /// Optional: Overrides the default relationship alias for this JOIN statement.
        /// </summary>
        /// <param name="relationshipAlias">The relationship alias to override with in the JOIN statement.</param>
        /// <returns>A statement class that contains various unary or binary comparison methods to finalize the JOIN statement.</returns>
        public JoinFinalStatement<TWhereStatement> WithAlias(string relationshipAlias)
        {
            Options.RelationshipAlias = relationshipAlias;
            return this;
        }

        /// <summary>
        /// A function to add WHERE conditions to the current JOIN statement.
        /// </summary>
        /// <param name="whereLogic">The functional logic of the WHERE statement containing one or more WHERE conditions.</param>
        /// <returns>An extendible part of a WHERE statement to continue adding WHERE conditions to.</returns>
        public CompoundWhereStatement<TWhereStatement> Where(Func<TWhereStatement, CompoundWhereStatement<TWhereStatement>> whereLogic)
        {
            var statement = WhereStatementFactory.CreateInstance<TWhereStatement>(Options, whereClause, Options.With);
            return whereLogic.Invoke(statement);
        }
    }
}