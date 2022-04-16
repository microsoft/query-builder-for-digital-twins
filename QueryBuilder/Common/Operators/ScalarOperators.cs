// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder
{
    using static Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Helpers.Terms;

    /// <summary>
    /// Model for Scalar operators.
    /// </summary>
    public abstract class ScalarOperator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScalarOperator"/> class.
        /// </summary>
        /// <param name="name">The name of the operator.</param>
        protected ScalarOperator(string name) => Name = name;

        /// <summary>
        /// Gets the name of the scalar operator.
        /// </summary>
        public string Name { get; private set; }
    }

    /// <summary>
    /// Model for Unary operators.
    /// </summary>
    public class ScalarUnaryOperator : ScalarOperator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScalarUnaryOperator"/> class.
        /// </summary>
        /// <param name="name">The name of the operator.</param>
        public ScalarUnaryOperator(string name) : base(name)
        {
        }
    }

    /// <summary>
    /// Model for Binary operators.
    /// </summary>
    public class ScalarBinaryOperator : ScalarOperator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScalarBinaryOperator"/> class.
        /// </summary>
        /// <param name="name">The name of the operator.</param>
        public ScalarBinaryOperator(string name) : base(name)
        {
        }
    }

    /// <summary>
    /// Whitelist Collection of Adt Scalar Operators.
    /// </summary>
    public class ScalarOperators
    {
        /// <summary>
        /// Gets IS_BOOL scalar operator.
        /// </summary>
        public static ScalarUnaryOperator IS_BOOL { get; } = new ScalarUnaryOperator(IsBool);

        /// <summary>
        /// Gets IS_DEFINED scalar operator.
        /// </summary>
        public static ScalarUnaryOperator IS_DEFINED { get; } = new ScalarUnaryOperator(IsDefined);

        /// <summary>
        /// Gets IS_NULL scalar operator.
        /// </summary>
        public static ScalarUnaryOperator IS_NULL { get; } = new ScalarUnaryOperator(IsNull);

        /// <summary>
        /// Gets IS_NUMBER scalar operator.
        /// </summary>
        public static ScalarUnaryOperator IS_NUMBER { get; } = new ScalarUnaryOperator(IsNumber);

        /// <summary>
        /// Gets IS_OBJECT scalar operator.
        /// </summary>
        public static ScalarUnaryOperator IS_OBJECT { get; } = new ScalarUnaryOperator(IsObject);

        /// <summary>
        /// Gets IS_STRING scalar operator.
        /// </summary>
        public static ScalarUnaryOperator IS_STRING { get; } = new ScalarUnaryOperator(IsString);

        /// <summary>
        /// Gets IS_PRIMITIVE scalar operator.
        /// </summary>
        public static ScalarUnaryOperator IS_PRIMITIVE { get; } = new ScalarUnaryOperator(IsPrimitive);

        /// <summary>
        /// Gets ENDSWITH operator.
        /// </summary>
        public static ScalarBinaryOperator ENDSWITH { get; } = new ScalarBinaryOperator(EndsWith);

        /// <summary>
        /// Gets STARTSWITH operator.
        /// </summary>
        public static ScalarBinaryOperator STARTSWITH { get; } = new ScalarBinaryOperator(StartsWith);

        /// <summary>
        /// Gets CONTAINS operator.
        /// </summary>
        public static ScalarBinaryOperator CONTAINS { get; } = new ScalarBinaryOperator(Contains);
    }
}