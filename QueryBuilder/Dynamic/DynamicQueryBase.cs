// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Dynamic
{
    using System;
    using System.Collections.Generic;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Clauses;

    /// <summary>
    /// A class for the basic common functionality.
    /// </summary>
    public abstract class DynamicQueryBase : QueryBase
    {
        /// <summary>
        /// Gets or Sets the root alias used in the query.
        /// </summary>
        protected string RootAlias { get; set; }

        internal IList<string> definedAliases { get; private set; }

        internal DynamicQueryBase(string rootAlias, IList<string> definedAliases, SelectClause selectClause, FromClause fromClause, IList<JoinClause> joinClauses, WhereClause whereClause) : base(selectClause, fromClause, joinClauses, whereClause)
        {
            this.RootAlias = rootAlias;
            this.definedAliases = definedAliases;
        }

        /// <summary>
        /// Validates the alias is assigned to a model, or the type is not ambiguous (isn't joined with the same type), and then adds the select clause.
        /// </summary>
        /// <param name="alias">A nullable alias string for selecting types that are mapped to aliases.</param>
        internal void ValidateAndAddSelect(string alias)
        {
            ValidateAliasIsDefined(alias);
            ValidateSelectAlias(alias);
            selectClause.Add(alias);
        }

        internal void ValidateAndAddAlias(string alias)
        {
            ValidateAliasNotNullOrWhiteSpace(alias);
            if (definedAliases.Contains(alias))
            {
                throw new ArgumentException($"Cannot use the alias: {alias}, because it's already assigned!");
            }

            definedAliases.Add(alias);
        }

        internal void ValidateAliasIsDefined(string alias)
        {
            ValidateAliasNotNullOrWhiteSpace(alias);
            if (!definedAliases.Contains(alias))
            {
                throw new ArgumentException($"Alias '{alias}' is not assigned!");
            }
        }

        internal static void ValidateAliasNotNullOrWhiteSpace(string alias)
        {
            if (string.IsNullOrWhiteSpace(alias))
            {
                throw new ArgumentNullException(nameof(alias));
            }
        }
    }
}