// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Typed
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using global::Azure.DigitalTwins.Core;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Clauses;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Helpers;

    /// <summary>
    /// A class for the basic common functionality.
    /// </summary>
    public abstract class TypedQueryBase : QueryBase
    {
        internal IDictionary<string, Type> aliasToTypeMapping { get; private set; }

        internal TypedQueryBase(IDictionary<string, Type> aliasToTypeMapping, SelectClause selectClause, FromClause fromClause, IList<JoinClause> joinClauses, WhereClause whereClause) : base(selectClause, fromClause, joinClauses, whereClause)
        {
            this.aliasToTypeMapping = aliasToTypeMapping;
        }

        /// <summary>
        /// Gets a Set of Types used in the query.
        /// </summary>
        /// <returns>All the Types used in the query.</returns>
        internal ISet<Type> Types => new HashSet<Type>(aliasToTypeMapping.Values);

        internal string GetAssignedAlias(Type type)
        {
            if (aliasToTypeMapping.Values.Count(v => v.Equals(type)) > 1)
            {
                throw new ArgumentException($"Ambiguous Expression! There is more than one of '{type}' model. Please, use aliases.");
            }

            return aliasToTypeMapping.FirstOrDefault(entry => entry.Value.Equals(type)).Key;
        }

        /// <summary>
        /// Validates the alias is assigned to a model, or the type is not ambiguous (isn't joined with the same type), and then adds the select clause.
        /// </summary>
        /// <typeparam name="TSelect">The type to be selected.</typeparam>
        /// <param name="alias">A nullable alias string for selecting types that are mapped to aliases.</param>
        internal void ValidateAndAddSelect<TSelect>(string alias = null, Expression<Func<TSelect, object>> propertySelector = null)
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

            if (propertySelector is not null)
            {
                QueryValidator.ExtractModelAndPropertyName(propertySelector, out _, out var propertyName);
                selectClause.Add($"{generatedAlias}.{propertyName}");
            }
            else
            {
                selectClause.Add(generatedAlias);
            }
        }

        /// <inheritdoc/>
        protected override void ValidateSelectAlias(string alias)
        {
            base.ValidateSelectAlias(alias);
            QueryValidator.ValidateAlias(alias, aliasToTypeMapping);
        }
    }
}