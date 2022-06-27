﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Clauses;

    internal static class FilterHelper
    {
        private static readonly IDictionary<string, string> binaryOperatorMap = new Dictionary<string, string>
        {
            { " eq ", " = " },
            { " equals ", " = " },
            { " == ", " = " },
            { " notequals ", " != " },
            { " ne ", " != " },
            { " neq ", " != " },
            { " !== ", " != " },
            { " lt ", " < " },
            { " le ", " <= " },
            { " lte ", " <= " },
            { " gt ", " > " },
            { " ge ", " >= " },
            { " gte ", " >= " }
        };

        private static readonly IDictionary<string, string> scalarFunctionMap = new Dictionary<string, string>
        {
            { "isofmodel(", "IS_OF_MODEL(" },
            { "is_of_model(", "IS_OF_MODEL(" },
            { "contains(", "CONTAINS(" },
            { "startswith(", "STARTSWITH(" },
            { "endswith(", "ENDSWITH(" },
            { " in [", " IN [" },
            { " notin [", " NIN [" },
            { " nin [", " NIN [" }
        };

        private static readonly IDictionary<string, string> compoundOperatorMap = new Dictionary<string, string>
        {
            { " || ", " OR " },
            { " or ", " OR " },
            { " and ", " AND " },
            { " && ", " AND " }
        };

        internal static string ReplaceOperators(string queryText) => ReplaceWithMap(binaryOperatorMap, queryText);

        internal static string ReplaceScalarFunctions(string queryText) => ReplaceWithMap(scalarFunctionMap, queryText);

        internal static string ReplaceCompoundOperators(string queryText) => ReplaceWithMap(compoundOperatorMap, queryText);

        internal static string FixRelationshipNames(string queryText, IEnumerable<JoinClause> joinClauses)
        {
            if (!joinClauses.Any())
            {
                return queryText;
            }

            var newString = queryText;
            var names = queryText.Split(' ');
            foreach (var name in names)
            {
                if (name.Contains('.'))
                {
                    var left = name.Split('.').First();
                    foreach (var clause in joinClauses)
                    {
                        if (clause.Relationship.Equals(left, StringComparison.OrdinalIgnoreCase))
                        {
                            newString = newString.Replace($"{left}.", $"{clause.RelationshipAlias}.");
                        }
                    }
                }
            }

            return newString;
        }

        private static string ReplaceWithMap(IDictionary<string, string> map, string queryText)
        {
            var newQueryText = queryText;
            foreach (var key in map.Keys)
            {
                if (newQueryText.Contains(key))
                {
                    newQueryText = newQueryText.Replace(key, map[key]);
                }
            }

            return newQueryText;
        }
    }
}