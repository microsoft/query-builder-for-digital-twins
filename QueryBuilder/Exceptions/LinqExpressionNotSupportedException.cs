// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder
{
    using System;

    /// <summary>
    /// Exception thrown when LINQ expression cannot be mapped to a valid ADT query.
    /// </summary>
    public class LinqExpressionNotSupportedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LinqExpressionNotSupportedException"/> class.
        /// </summary>
        public LinqExpressionNotSupportedException(string message) : base(message)
        {
        }
    }
}