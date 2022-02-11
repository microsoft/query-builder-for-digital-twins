// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.Azure.DigitalTwins.QueryBuilder
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// Exception thrown when select * is used with join clause.
    /// </summary>
    public class NoJsonPropertyException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NoJsonPropertyException"/> class.
        /// </summary>
        public NoJsonPropertyException(Expression expression) : base($"Expression {expression} does not map to a JSON property.")
        {
        }
    }
}