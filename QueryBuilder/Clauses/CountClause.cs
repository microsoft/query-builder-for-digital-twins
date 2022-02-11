// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Clauses
{
    using static Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Helpers.Terms;

    internal class CountClause : SelectClause
    {
        public override string ToString()
        {
            return $"{Select} {Count}()";
        }
    }
}