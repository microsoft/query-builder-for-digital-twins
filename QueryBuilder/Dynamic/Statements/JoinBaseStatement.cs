// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Dynamic.Statements
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Clauses;

    /// <summary>
    /// A base Join statement class for the common properties carried through the Join process.
    /// </summary>
    public abstract class JoinBaseStatement
    {
        internal IList<JoinClause> Clauses { get; private set; }

        internal JoinClause Current => Clauses.LastOrDefault();

        internal WhereClause WhereClause { get; private set; }

        internal JoinBaseStatement(IList<JoinClause> clauses, WhereClause whereClause)
        {
            Clauses = clauses;
            WhereClause = whereClause;
        }
    }
}