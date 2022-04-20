// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Dynamic.Statements
{
    using System;
    using System.Collections.Generic;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Clauses;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Helpers;

    /// <summary>
    /// A class that contains various unary and binary comparison methods to finalize a JOIN statement.
    /// </summary>
    /// <typeparam name="TWhereStatement">The type of WHERE statement supported for the JOIN statement.</typeparam>
    public class JoinFinalStatement<TWhereStatement> : JoinBaseStatement where TWhereStatement : WhereBaseStatement<TWhereStatement>
    {
        internal JoinFinalStatement(IList<JoinClause> joinClauses, WhereClause whereClause) : base(joinClauses, whereClause)
        {
        }

        /// <summary>
        /// Optional: Overrides the default relationship alias for this JOIN statement.
        /// </summary>
        /// <param name="relationshipAlias">The relationship alias to override with in the JOIN statement.</param>
        /// <returns>A statement class that contains various unary or binary comparison methods to finalize the JOIN statement.</returns>
        public JoinFinalStatement<TWhereStatement> As(string relationshipAlias)
        {
            Current.RelationshipAlias = relationshipAlias;
            return this;
        }

        /// <summary>
        /// A function to add WHERE conditions to the current JOIN statement.
        /// </summary>
        /// <param name="whereLogic">The functional logic of the WHERE statement containing one or more WHERE conditions.</param>
        /// <returns>An extendible part of a WHERE statement to continue adding WHERE conditions to.</returns>
        public CompoundWhereStatement<TWhereStatement> Where(Func<TWhereStatement, CompoundWhereStatement<TWhereStatement>> whereLogic)
        {
            var statement = WhereStatementFactory.CreateInstance<TWhereStatement>(Clauses, WhereClause, Current.JoinWith);
            return whereLogic.Invoke(statement);
        }

#pragma warning disable SA1629
        /// <summary>
        /// Adds a nested JOIN to the existing one. This will contextually bind this JOIN to a parent one.
        /// Example:
        /// Floor -> hasChildren
        /// Area -> hasDevices
        /// .Join(f => f
        ///     .With("area")
        ///     .RelatedBy("hasChildren")
        ///     .Join(a => a.With("device").RelatedBy("hasDevices)))
        /// </summary>
        /// <param name="joinLogic">The functional logic of the JOIN statement containing the required components to join twins via a relationship.</param>
        /// <returns>A statement class that contains various unary or binary comparison methods to finalize the JOIN statement.</returns>
        public JoinFinalStatement<TWhereStatement> Join(Func<JoinWithStatement<TWhereStatement>, JoinFinalStatement<TWhereStatement>> joinLogic)
        {
            joinLogic.Invoke(new JoinWithStatement<TWhereStatement>(Clauses, WhereClause, Current.JoinWith));
            return this;
        }
#pragma warning restore SA1629

        /// <summary>
        /// Adds a nested JOIN and WHERE statements to the existing JOIN statement.
        /// WHERE statements added at this level will be contextually bound to the twin that was JOINed.
        /// </summary>
        /// <param name="joinAndWhereLogic">The functional logic of the JOIN and WHERE statement containing one or more WHERE conditions.</param>
        /// <returns>>An extendible part of a WHERE statement to continue adding WHERE conditions to.</returns>
        public CompoundWhereStatement<TWhereStatement> Join(Func<JoinWithStatement<TWhereStatement>, CompoundWhereStatement<TWhereStatement>> joinAndWhereLogic)
        {
            return joinAndWhereLogic.Invoke(new JoinWithStatement<TWhereStatement>(Clauses, WhereClause, Current.JoinWith));
        }
    }
}