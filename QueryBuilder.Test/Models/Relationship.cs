// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace QueryBuilder.UnitTests
{
    using System;
    using System.Text.Json.Serialization;
    using Azure.DigitalTwins.Core;

    /// <summary>
    /// An abstract base class to be implemented for DTDL model-specific relationships.
    /// </summary>
    /// <typeparam name="TTarget">Target Type.</typeparam>
    public abstract class Relationship<TTarget> : BasicRelationship, IEquatable<BasicRelationship>
        where TTarget : BasicDigitalTwin
    {
        /// <summary>
        /// Gets or sets the Target twin of the Relationship.
        /// </summary>
        [JsonIgnore]
        public TTarget? Target { get; set; }

        /// <inheritdoc/>
        public abstract bool Equals(BasicRelationship? other);

        /// <summary>
        /// Initializes the relationship properties given a source and target twin.
        /// </summary>
        /// <param name="source">The source twin to use for the relationship.</param>
        /// <param name="target">The target twin to use for the relationship.</param>
        protected void InitializeFromTwins(BasicDigitalTwin source, TTarget target)
        {
            Id = $"{source.Id}-{Name}->{target.Id}";
            SourceId = source.Id;
            TargetId = target.Id;
            Target = target;
        }
    }
}