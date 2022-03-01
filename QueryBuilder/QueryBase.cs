// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::Azure.DigitalTwins.Core;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Clauses;

    /// <summary>
    /// A class for the basic common functionality.
    /// </summary>
    public abstract class QueryBase
    {
        internal IDictionary<string, Type> aliasToTypeMapping { get; private set; }

        internal SelectClause selectClause { get; private protected set; }

        internal FromClause fromClause { get; private protected set; }

        internal IList<JoinClause> joinClauses { get; private protected set; }

        internal WhereClause whereClause { get; private protected set; }

        internal QueryBase(IDictionary<string, Type> aliasToTypeMapping, SelectClause selectClause, FromClause fromClause, IList<JoinClause> joinClauses, WhereClause whereClause)
        {
            this.aliasToTypeMapping = aliasToTypeMapping;
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
        public IEnumerable<string> GetSelectedAliases()
        {
            return selectClause.Aliases;
        }

        /// <summary>
        /// Get a Set of Types used in the query.
        /// </summary>
        /// <returns>All the Types used in the query.</returns>
        internal ISet<Type> GetTypes()
        {
            return new HashSet<Type>(aliasToTypeMapping.Values.Distinct());
        }

        internal string GetAssignedAlias(Type type)
        {
            if (aliasToTypeMapping.Values.Count(v => v.Equals(type)) > 1)
            {
                throw new ArgumentException($"Ambiguous Expression! There is more than one of '{type}' model. Please, use aliases.");
            }

            return aliasToTypeMapping.Where(entry => entry.Value.Equals(type)).Select(e => e.Key).FirstOrDefault();
        }

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
        /// Validates the alias is assigned to a model, or the type is not ambiguous (isn't joined with the same type), and then adds the select clause.
        /// </summary>
        /// <typeparam name="TSelect">The type to be selected.</typeparam>
        /// <param name="alias">A nullable alias string for selecting types that are mapped to aliases.</param>
        internal void ValidateAndAddSelect<TSelect>(string alias = null)
            where TSelect : BasicDigitalTwin
        {
            if (!string.IsNullOrEmpty(alias))
            {
                ValidateSelectAlias(alias);
            }
            else if (aliasToTypeMapping.Count(entry => entry.Value.Equals(typeof(TSelect))) > 1)
            {
                throw new ArgumentException($"Ambiguous select: there is more than one {typeof(TSelect)} in the query. Please use alias instead!");
            }

            var generatedAlias = string.IsNullOrEmpty(alias) ? GetAssignedAlias(typeof(TSelect)) : alias;
            selectClause.Add(generatedAlias);
        }

        private void ValidateSelectAlias(string alias)
        {
            if (GetSelectedAliases().Contains(alias))
            {
                throw new ArgumentException($"Alias: '{alias}' cannot be selected twice!");
            }

            if (!aliasToTypeMapping.ContainsKey(alias))
            {
                throw new ArgumentException($"Alias: '{alias}' does not exist!");
            }
        }
    }
}