// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.Azure.DigitalTwins.QueryBuilder
{
    using static Microsoft.Azure.DigitalTwins.QueryBuilder.Helpers.Terms;

    /// <summary>
    /// Model for Scalar operators.
    /// </summary>
    public abstract class AdtScalarOperator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdtScalarOperator"/> class.
        /// </summary>
        /// <param name="name">The name of the operator.</param>
        protected AdtScalarOperator(string name) => Name = name;

        /// <summary>
        /// Gets the name of the scalar operator.
        /// </summary>
        public string Name { get; private set; }
    }

    /// <summary>
    /// Model for Unary operators.
    /// </summary>
    public class AdtScalarUnaryOperator : AdtScalarOperator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdtScalarUnaryOperator"/> class.
        /// </summary>
        /// <param name="name">The name of the operator.</param>
        public AdtScalarUnaryOperator(string name) : base(name)
        {
        }
    }

    /// <summary>
    /// Model for Binary operators.
    /// </summary>
    public class AdtScalarBinaryOperator : AdtScalarOperator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdtScalarBinaryOperator"/> class.
        /// </summary>
        /// <param name="name">The name of the operator.</param>
        public AdtScalarBinaryOperator(string name) : base(name)
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
        public static AdtScalarUnaryOperator IS_BOOL { get; } = new AdtScalarUnaryOperator(IsBool);

        /// <summary>
        /// Gets IS_DEFINED scalar operator.
        /// </summary>
        public static AdtScalarUnaryOperator IS_DEFINED { get; } = new AdtScalarUnaryOperator(IsDefined);

        /// <summary>
        /// Gets IS_NULL scalar operator.
        /// </summary>
        public static AdtScalarUnaryOperator IS_NULL { get; } = new AdtScalarUnaryOperator(IsNull);

        /// <summary>
        /// Gets IS_NUMBER scalar operator.
        /// </summary>
        public static AdtScalarUnaryOperator IS_NUMBER { get; } = new AdtScalarUnaryOperator(IsNumber);

        /// <summary>
        /// Gets IS_OBJECT scalar operator.
        /// </summary>
        public static AdtScalarUnaryOperator IS_OBJECT { get; } = new AdtScalarUnaryOperator(IsObject);

        /// <summary>
        /// Gets IS_STRING scalar operator.
        /// </summary>
        public static AdtScalarUnaryOperator IS_STRING { get; } = new AdtScalarUnaryOperator(IsString);

        /// <summary>
        /// Gets IS_PRIMITIVE scalar operator.
        /// </summary>
        public static AdtScalarUnaryOperator IS_PRIMITIVE { get; } = new AdtScalarUnaryOperator(IsPrimitive);

        /// <summary>
        /// Gets ENDSWITH operator.
        /// </summary>
        public static AdtScalarBinaryOperator ENDSWITH { get; } = new AdtScalarBinaryOperator(EndsWith);

        /// <summary>
        /// Gets STARTSWITH operator.
        /// </summary>
        public static AdtScalarBinaryOperator STARTSWITH { get; } = new AdtScalarBinaryOperator(StartsWith);

        /// <summary>
        /// Gets CONTAINS operator.
        /// </summary>
        public static AdtScalarBinaryOperator CONTAINS { get; } = new AdtScalarBinaryOperator(Contains);
    }
}