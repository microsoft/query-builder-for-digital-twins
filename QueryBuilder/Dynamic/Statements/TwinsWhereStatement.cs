// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Dynamic.Statements
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Clauses;

    /// <summary>
    /// A WHERE statement class used for querying twins.
    /// </summary>
    public class TwinsWhereStatement : WhereBaseStatement<TwinsWhereStatement>
    {
        internal TwinsWhereStatement(IEnumerable<JoinOptions> joinOptions, WhereClause clause, string alias) : base(joinOptions, clause, alias)
        {
        }

        internal TwinsWhereStatement(WhereClause whereClause, string alias) : base(whereClause, alias)
        {
        }

        /// <summary>
        /// Adds the propertyName to filter against to the WHERE statement.
        /// </summary>
        /// <param name="propertyName">The name of the property to filter against.</param>
        /// <param name="forAlias">
        /// Optional: Alias to override the default alias in the current scope.
        /// This allows applying WHERE conditions to previous scopes of the query.
        /// I.e. It can allow applying a WHERE condition to a relationship outside the scope of a previous JOIN statement.
        /// </param>
        /// <returns>A statement class that contains various unary or binary comparison methods to finalize the WHERE statement.</returns>
        public WherePropertyStatement<TwinsWhereStatement> Property(string propertyName, string forAlias = null)
        {
            var aliasOverride = string.IsNullOrWhiteSpace(forAlias) ? Alias : forAlias;
            return new WherePropertyStatement<TwinsWhereStatement>(JoinOptions, WhereClause, propertyName, aliasOverride);
        }

        /// <summary>
        /// Adds an IS_OF_MODEL scalar function to the WHERE statement to filter by a model dtmi.
        /// </summary>
        /// <param name="dtmi">The model dtmi to use in the scalar function.</param>
        /// <returns>A conjunction class that supports appending more conditions to the WHERE statements via OR or AND terms.</returns>
        public CompoundWhereStatement<TwinsWhereStatement> IsOfModel(string dtmi)
        {
            WhereClause.AddCondition(new WhereIsOfModelCondition { Alias = this.Alias, Model = dtmi });
            return new CompoundWhereStatement<TwinsWhereStatement>(JoinOptions, WhereClause, Alias);
        }

        /// <summary>
        /// Adds the propertyName to filter against to the WHERE statement.
        /// </summary>
        /// <param name="propertyName">The name of the relationship property to filter against.</param>
        /// <returns>A statement class that contains various unary or binary comparison methods to finalize the WHERE statement.</returns>
        public WherePropertyStatement<TwinsWhereStatement> RelationshipProperty(string propertyName)
        {
            var latestJoinOptions = JoinOptions.LastOrDefault();
            var relationshipAlias = string.IsNullOrWhiteSpace(latestJoinOptions.RelationshipAlias) ? $"{latestJoinOptions.RelationshipName.ToLowerInvariant()}relationship" : latestJoinOptions.RelationshipAlias;
            return new WherePropertyStatement<TwinsWhereStatement>(JoinOptions, WhereClause, propertyName, relationshipAlias);
        }
    }
}