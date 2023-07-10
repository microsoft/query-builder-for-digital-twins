// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Clauses
{
    using System;
    using static Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Helpers.Terms;

    internal class JoinClause
    {
        internal Guid Id { get; set; } = Guid.NewGuid();

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