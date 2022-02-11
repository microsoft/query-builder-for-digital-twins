// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.Azure.DigitalTwins.QueryBuilder.Clauses
{
    using static Microsoft.Azure.DigitalTwins.QueryBuilder.Helpers.Terms;

    internal class FromClause
    {
        internal string Alias { get; set; }

        public override string ToString()
        {
            return $"{From} {DigitalTwins} {Alias}";
        }
    }
}