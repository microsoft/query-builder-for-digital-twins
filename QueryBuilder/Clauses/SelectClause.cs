// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.Azure.DigitalTwins.QueryBuilder.Clauses
{
    using System.Collections.Generic;
    using static Microsoft.Azure.DigitalTwins.QueryBuilder.Helpers.Terms;

    internal class SelectClause
    {
        internal IList<string> Aliases { get; }

        internal ushort NumberOfRecords { get; set; } = 0;

        internal SelectClause()
        {
            Aliases = new List<string>();
        }

        internal void Add(string select)
        {
            Aliases.Add(select);
        }

        internal void Clear()
        {
            Aliases.Clear();
        }

        public override string ToString()
        {
            var top = NumberOfRecords > 0 ? $"{Top}({NumberOfRecords}) " : string.Empty;
            return $"{Select} {top}{string.Join(", ", Aliases)}";
        }
    }
}