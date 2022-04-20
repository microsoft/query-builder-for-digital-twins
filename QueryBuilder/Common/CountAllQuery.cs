// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder
{
    /// <summary>
    /// A query that counts all digital twins.
    /// </summary>
    public class CountAllQuery : QueryBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CountAllQuery"/> class.
        /// </summary>
        public CountAllQuery() : base(null, null, null, null)
        {
        }

        /// <inheritdoc/>
        public override string BuildAdtQuery()
        {
            return "SELECT COUNT() FROM DIGITALTWINS";
        }
    }
}