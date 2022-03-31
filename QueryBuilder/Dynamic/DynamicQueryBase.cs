// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Dynamic
{
    using System;
    using System.Collections.Generic;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Clauses;
    using static Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Helpers.Terms;

    /// <summary>
    /// A class for the basic common functionality.
    /// </summary>
    public abstract class DynamicQueryBase : QueryBase
    {
        /// <summary>
        ///
        /// </summary>
        /// <value></value>
        protected string RootTwinAlias { get; set; } = DefaultTwinAlias;

        internal IList<string> allowedAliases { get; private set; }

        internal DynamicQueryBase(string rootTwinAlias, IList<string> allowedAliases, SelectClause selectClause, FromClause fromClause, IList<JoinClause> joinClauses, WhereClause whereClause) : base(selectClause, fromClause, joinClauses, whereClause)
        {
            this.RootTwinAlias = rootTwinAlias;
            this.allowedAliases = allowedAliases;
        }

        /// <summary>
        /// Validates the alias is assigned to a model, or the type is not ambiguous (isn't joined with the same type), and then adds the select clause.
        /// </summary>
        /// <param name="alias">A nullable alias string for selecting types that are mapped to aliases.</param>
        internal void ValidateAndAddSelect(string alias)
        {
            if (!string.IsNullOrEmpty(alias))
            {
                ValidateSelectAlias(alias);
                selectClause.Add(alias);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="alias"></param>
        protected override void ValidateSelectAlias(string alias)
        {
            base.ValidateSelectAlias(alias);
            if (!allowedAliases.Contains(alias))
            {
                throw new ArgumentException($"Alias: '{alias}' does not exist!");
            }
        }
    }
}