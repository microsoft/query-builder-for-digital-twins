// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Clauses
{
    using System.Collections.Generic;
    using System.Linq;
    using static Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Helpers.Terms;

    internal class SelectClause
    {
        internal IList<KeyValuePair<string, string>> Aliases { get; }

        internal ushort NumberOfRecords { get; set; } = 0;

        internal SelectClause()
        {
            Aliases = new List<KeyValuePair<string, string>>();
        }

        internal void Add(string select)
        {
            Aliases.Add(new KeyValuePair<string, string>(select, select));
        }

        internal void Add(string select, string alias)
        {
            Aliases.Add(new KeyValuePair<string, string>(alias, select));
        }

        internal void Clear()
        {
            Aliases.Clear();
        }

        public override string ToString()
        {
            var top = NumberOfRecords > 0 ? $"{Top}({NumberOfRecords}) " : string.Empty;
            return $"{Select} {top}{string.Join(", ", Aliases.Select(a => a.Value))}";
        }
    }
}