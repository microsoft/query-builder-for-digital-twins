// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Clauses
{
    using System.Collections.Generic;
    using System.Text;
    using static Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Helpers.Terms;

    internal class WhereClause
    {
        internal IList<string> Conditions { get; }

        internal WhereClause()
        {
            Conditions = new List<string>();
        }

        internal void AddCondition(Condition condition)
        {
            AddCondition(condition.ToString());
        }

        internal void AddCondition(string condition)
        {
            Conditions.Add(condition);
        }

        public string GetConditionsText()
        {
            var builder = new StringBuilder();
            for (var i = 0; i < Conditions.Count; i++)
            {
                var next = i + 1;
                if (next < Conditions.Count && Conditions[next] == Or)
                {
                    builder.Append($"{Conditions[i]} {Or} {Conditions[next + 1]}");
                    i += 2;
                }
                else if (next < Conditions.Count && Conditions[next] == And)
                {
                    builder.Append($"{Conditions[i]} {And} {Conditions[next + 1]}");
                    i += 2;
                }
                else
                {
                    var and = i == 0 ? string.Empty : $" {And} ";
                    builder.Append($"{and}{Conditions[i]}");
                }
            }

            return builder.ToString();
        }

        public override string ToString()
        {
            var sql = GetConditionsText();
            return string.IsNullOrWhiteSpace(sql) ? string.Empty : $"{Where} {sql}";
        }
    }
}