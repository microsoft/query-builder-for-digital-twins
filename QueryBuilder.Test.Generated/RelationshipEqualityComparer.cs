// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace QueryBuilder.Test.Generated;

using System.Collections.Generic;
using Azure.DigitalTwins.Core;

/// <summary>
/// An EqualityComparer implementation for ADT BasicRelationships.
/// </summary>
public class RelationshipEqualityComparer : IEqualityComparer<BasicRelationship>
{
    /// <inheritdoc/>
    public bool Equals(BasicRelationship? x, BasicRelationship? y)
    {
        if (x == null && y == null)
        {
            return true;
        }

        if ((x != null && y == null) || (x == null && y != null))
        {
            return false;
        }

        if (x != null && y != null)
        {
            return x.Id == y.Id
                && x.Name == y.Name
                && x.SourceId == y.SourceId
                && x.TargetId == y.TargetId
                && x.Properties.DictionaryEquals(y.Properties);
        }

        return false;
    }

    /// <inheritdoc/>
    public int GetHashCode(BasicRelationship obj)
    {
        return obj.Id.GetHashCode();
    }
}