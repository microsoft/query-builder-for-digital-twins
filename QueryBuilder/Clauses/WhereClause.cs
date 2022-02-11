// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.Azure.DigitalTwins.QueryBuilder.Clauses
{
    using System.Collections.Generic;
    using static Microsoft.Azure.DigitalTwins.QueryBuilder.Helpers.Terms;

    internal class WhereClause
    {
        internal IList<Condition> Conditions { get; }

        internal WhereClause()
        {
            Conditions = new List<Condition>();
        }

        internal void AddCondition(Condition condition)
        {
            Conditions.Add(condition);
        }

        public override string ToString()
        {
            var sql = string.Join($" {And} ", Conditions);

            return $"{Where} {sql}";
        }
    }
}