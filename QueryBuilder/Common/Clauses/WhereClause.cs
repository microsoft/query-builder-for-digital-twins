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

        /// <summary>
        /// This method takes several conditions and forms them into a single string of conditions.
        /// There's an important distinction of behavior between the Typed Query Builder and the Dynamic Query Builder.
        /// Below are some examples comparing what a list of conditions may look like from the Typed vs Dynamic Builder
        /// to better illustrate how the behavior of this method works.
        ///
        /// Example:
        /// Typed Builder Conditions -> [ ("twin.name = 'sometwin' AND twin.value > 4"), "IS_OF_MODEL(twin, 'dtmi:somemodel;1')"]
        /// Dynamic Builder Conditions -> [ "twin.name = 'sometwin'", "AND", "twin.value > 4", "AND", "IS_OF_MODEL(twin, 'dtmi:somemodel;1')" ]
        ///
        /// Note that the Typed builder has explicit AND or OR conditions that establish a precedence with parenthesis vs the implicit AND
        /// that would be added between the 2 conditions above. In contrast, the Dynamic builder establishes precedence with a more verbose method
        /// and there is no implicit AND injected into the statement.
        /// </summary>
        /// <returns>A string representation of a collection of conditions.</returns>
        internal string GetConditionsText()
        {
            var builder = new StringBuilder();
            for (var i = 0; i < Conditions.Count; i++)
            {
                var next = i + 1;
                if (TryHandleCompoundStatement(builder, ref next, ref i))
                {
                    continue;
                }
                else
                {
                    var and = i == 0 ? string.Empty : $"{And} ";
                    builder.Append($"{and}{Conditions[i]}");
                }

                TryAppendSpace(builder, i);
            }

            return builder.ToString();
        }

        public override string ToString()
        {
            var serializedConditions = GetConditionsText();
            return string.IsNullOrWhiteSpace(serializedConditions) ? string.Empty : $"{Where} {serializedConditions}";
        }

        private bool TryHandleCompoundStatement(StringBuilder builder, ref int next, ref int i)
        {
            if (i == 0)
            {
                if (Conditions[i] == Not)
                {
                    HandleNots(builder, $"{Not} ", ref next);
                    i = next;
                    TryAppendSpace(builder, i);
                    return true;
                }

                builder.Append($"{Conditions[i]}");
                TryAppendSpace(builder, i);
                return true;
            }
            else if (next < Conditions.Count && (Conditions[i] == Or || Conditions[i] == And))
            {
                var compound = Conditions[i];

                if (Conditions[next] == Not)
                {
                    HandleNots(builder, $"{compound} ", ref next);
                    i = next;
                    TryAppendSpace(builder, i);
                    return true;
                }

                builder.Append($"{compound} {Conditions[next]}");
                i++;
                TryAppendSpace(builder, i);

                return true;
            }

            return false;
        }

        private void TryAppendSpace(StringBuilder builder, int i)
        {
            if (i + 1 < Conditions.Count)
            {
                builder.Append(" ");
            }
        }

        /// <summary>
        /// Since multiple NOTs can be added and it's not illegal syntax we need to handle that.
        /// After the NOTs are handled, the actual conditions is appended to the string.
        /// </summary>
        /// <param name="builder">The StringBuilder to apply the text to.</param>
        /// <param name="preLoopText">The initial string condition(s) to append before handling any continuous NOTs.</param>
        /// <param name="next">The forward-looking position in the Conditions array.</param>
        private void HandleNots(StringBuilder builder, string preLoopText, ref int next)
        {
            builder.Append(preLoopText);
            while (next < Conditions.Count && Conditions[next] == Not)
            {
                builder.Append($"{Not} ");
                next++;
            }

            builder.Append(Conditions[next]);
        }
    }
}