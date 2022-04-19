// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Clauses
{
    using static Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Helpers.Terms;

    internal class FromClause
    {
        internal string Collection { get; set; } = DigitalTwins;

        internal string Alias { get; set; }

        public override string ToString()
        {
            return $"{From} {Collection} {Alias}";
        }
    }
}