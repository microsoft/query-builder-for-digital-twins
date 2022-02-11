// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.Azure.DigitalTwins.QueryBuilder.Clauses
{
    using static Microsoft.Azure.DigitalTwins.QueryBuilder.Helpers.Terms;

    internal class CountClause : SelectClause
    {
        public override string ToString()
        {
            return $"{Select} {Count}()";
        }
    }
}