// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder
{
    using System;

    /// <summary>
    /// Exception thrown when LINQ expression has an operator that's not supported by ADT.
    /// </summary>
    public class LinqOperatorNotSupportedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LinqOperatorNotSupportedException"/> class.
        /// </summary>
        public LinqOperatorNotSupportedException(string message) : base(message)
        {
        }
    }
}