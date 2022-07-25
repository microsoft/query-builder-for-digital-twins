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
        internal void ValidateAndAddSelect<TSelect>(string alias = null)
        {
            var generatedAlias = GenerateTypeAlias<TSelect>(alias);
            selectClause.Add(generatedAlias);
        }

        /// <summary>
        /// Validates the alias is assigned to a model, or the type is not ambiguous (isn't joined with the same type), and then adds the select clause.
        /// </summary>
        /// <typeparam name="TSelect">The type to be selected.</typeparam>
        /// <typeparam name="TOut">The type of the property.</typeparam>
        /// <param name="propertySelector">A nullable expression for selecting an individual property on a type.</param>
        /// <param name="propertyAlias">An alias string for assigning an alias to the property.</param>
        /// <param name="typeAlias">An alias string for selecting types that are mapped to aliases.</param>
        internal void ValidateAndAddSelect<TSelect, TOut>(Expression<Func<TSelect, TOut>> propertySelector, string propertyAlias, string typeAlias)
        {
            var generatedAlias = GenerateTypeAlias<TSelect>(typeAlias);

            QueryValidator.ExtractModelAndPropertyName(propertySelector, out _, out var propertyName);

            if (string.IsNullOrEmpty(propertyAlias))
            {
                if (selectClause.Aliases.Any(s => s.Value.EndsWith($".{propertyName}")))
                {
                    throw new ArgumentException($"Duplicate property name: there is already a property with the name '{propertyName}' in the select clause. Please add a property alias.");
                }

                selectClause.Add($"{generatedAlias}.{propertyName}", propertyName);
            }
            else
            {
                selectClause.Add($"{generatedAlias}.{propertyName} AS {propertyAlias}", propertyAlias);
            }
        }

        private string GenerateTypeAlias<TSelect>(string typeAlias)
        {
            if (!string.IsNullOrEmpty(typeAlias))
            {
                ValidateSelectAlias(typeAlias);
            }
            else if (aliasToTypeMapping.Count(entry => entry.Value.Equals(typeof(TSelect))) > 1)
            {
                throw new ArgumentException($"Ambiguous select: there is more than one {typeof(TSelect)} in the query. Please use alias instead!");
            }

            return string.IsNullOrEmpty(typeAlias) ? GetAssignedAlias(typeof(TSelect)) : typeAlias;
        }

        /// <inheritdoc/>
        protected override void ValidateSelectAlias(string alias)
        {
            base.ValidateSelectAlias(alias);
            QueryValidator.ValidateAlias(alias, aliasToTypeMapping);
        }
    }
}