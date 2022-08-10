// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Clauses;

    /// <summary>
    /// A class for the basic common functionality.
    /// </summary>
    public abstract class QueryBase
    {
        internal SelectClause selectClause { get; private protected set; }

        internal FromClause fromClause { get; private protected set; }

        internal IList<JoinClause> joinClauses { get; private protected set; }

        internal WhereClause whereClause { get; private protected set; }

        internal QueryBase(SelectClause selectClause, FromClause fromClause, IList<JoinClause> joinClauses, WhereClause whereClause)
        {
            this.selectClause = selectClause;
            this.fromClause = fromClause;
            this.joinClauses = joinClauses;
            this.whereClause = whereClause;
        }

        internal void ClearSelects()
        {
            selectClause.Clear();
        }

        /// <summary>
        /// Gets the list of aliases in the query.
        /// </summary>
        /// <returns>All the aliases used in the query.</returns>
        public IEnumerable<string> SelectedAliases => selectClause.Aliases.Select(a => a.Key);

        /// <summary>
        /// Build the ADT query.
        /// </summary>
        /// <returns>ADT query string.</returns>
        public virtual string BuildAdtQuery()
        {
            var clauses = new List<string>
            {
                CompileSelect(),
                CompileFrom(),
                CompileJoins(),
                CompileWhere()
            }
            .Where(c => !string.IsNullOrEmpty(c));

            return string.Join(" ", clauses);
        }

        private string CompileSelect()
        {
            return selectClause.ToString();
        }

        private string CompileFrom()
        {
            return fromClause.ToString();
        }

        private string CompileJoins()
        {
            return string.Join(" ", joinClauses.Select(j => j.ToString()));
        }

        private string CompileWhere()
        {
            return whereClause.ToString();
        }

        /// <summary>
        /// Validates that a given alias can be used in the SELECT clause of the query.
        /// </summary>
        /// <param name="alias">The alias to validate.</param>
        protected virtual void ValidateSelectAlias(string alias)
        {
            if (SelectedAliases.Contains(alias))
            {
                throw new ArgumentException($"Alias: '{alias}' cannot be selected twice!");
            }
        }
    }
}