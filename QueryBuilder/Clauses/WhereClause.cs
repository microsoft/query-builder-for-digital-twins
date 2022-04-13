// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Clauses
{
    using System.Collections.Generic;
    using System.Linq;
    using static Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Helpers.Terms;

    internal class WhereClause
    {
        internal HashSet<Condition> Conditions { get; }

        internal WhereClause()
        {
            Conditions = new HashSet<Condition>();
        }

        internal void AddCondition(Condition condition)
        {
            Conditions.Add(condition);
        }

        public override string ToString()
        {
            if (!Conditions.Any())
            {
                return string.Empty;
            }

            var sql = string.Join($" {And} ", Conditions);
            return $"{Where} {sql}";
        }
    }
}