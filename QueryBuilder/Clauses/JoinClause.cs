// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Clauses
{
    using static Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Helpers.Terms;

    internal class JoinClause
    {
        internal string JoinWith { get; set; }

        internal string JoinFrom { get; set; }

        internal string Relationship { get; set; }

        internal string RelationshipAlias { get; set; }

        public override string ToString()
        {
            return $"{Join} {JoinWith} {Related} {JoinFrom}.{Relationship} {RelationshipAlias}";
        }
    }
}