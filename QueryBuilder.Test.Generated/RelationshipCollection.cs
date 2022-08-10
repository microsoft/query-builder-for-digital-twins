// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace QueryBuilder.Test.Generated;

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Azure.DigitalTwins.Core;

/// <summary>
/// An abstract base class for model-specific relationships to implement.
/// </summary>
/// <typeparam name="TRelationship">The concrete relationship implementation for the DTDL model.</typeparam>
/// <typeparam name="TTarget">The relationship target class.</typeparam>
public abstract class RelationshipCollection<TRelationship, TTarget> : BasicRelationship, IEnumerable<TRelationship>
    where TRelationship : Relationship<TTarget>, new()
    where TTarget : BasicDigitalTwin
{
    private readonly ICollection<TRelationship> relationships = new List<TRelationship>();

    /// <summary>
    /// Initializes a new instance of the <see cref="RelationshipCollection{T, T}"/> class.
    /// </summary>
    public RelationshipCollection()
    {
        Name = new TRelationship().Name;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RelationshipCollection{T, T}"/> class.
    /// </summary>
    /// <param name="relationships">A collection of relationships to initalize the <see cref="RelationshipCollection{T, T}"/> with.</param>
    public RelationshipCollection(IEnumerable<TRelationship> relationships) : this()
    {
        this.relationships = new List<TRelationship>(relationships);
    }

    /// <summary>
    /// Gets the target twins of this relationship collection.
    /// </summary>
    public IEnumerable<TTarget> Targets => (IEnumerable<TTarget>)relationships.Where(r => r.Target != null).Select(r => r.Target);

    /// <inheritdoc/>
    public IEnumerator<TRelationship> GetEnumerator() => relationships.GetEnumerator();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => relationships.GetEnumerator();
}