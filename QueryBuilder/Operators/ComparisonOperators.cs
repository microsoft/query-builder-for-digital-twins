// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.Azure.DigitalTwins.QueryBuilder
{
    using static Microsoft.Azure.DigitalTwins.QueryBuilder.Helpers.Terms;

    /// <summary>
    /// White List Collection of ADT comparison operator.
    /// </summary>
    public class ComparisonOperators
    {
        private ComparisonOperators(string name) => Operator = name;

        /// <summary>
        /// Gets value of the Comparison operator.
        /// </summary>
        public string Operator { get; private set; }

        /// <summary>
        /// Gets equals to operator.
        /// </summary>
        public static ComparisonOperators IsEqualTo { get; } = new ComparisonOperators(Equal);

        /// <summary>
        /// Gets less than operator.
        /// </summary>
        public static ComparisonOperators IsLessThan { get; } = new ComparisonOperators(Less);

        /// <summary>
        /// Gets greater than operator.
        /// </summary>
        public static ComparisonOperators IsGreaterThan { get; } = new ComparisonOperators(Greater);

        /// <summary>
        /// Gets less than equal to operator.
        /// </summary>
        public static ComparisonOperators IsLessThanOrEqualTo { get; } = new ComparisonOperators(LessOrEqual);

        /// <summary>
        /// Gets greater than equal to operator.
        /// </summary>
        public static ComparisonOperators IsGreaterThanOrEqualTo { get; } = new ComparisonOperators(GreaterOrEqual);

        /// <summary>
        /// Gets not equals to operator.
        /// </summary>
        public static ComparisonOperators NotEqualTo { get; } = new ComparisonOperators(NotEqual);
    }
}