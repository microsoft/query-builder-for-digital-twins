// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace QueryBuilder.Test.Generated;

using System;
using System.Collections.Generic;
using System.Linq;
using Azure.DigitalTwins.Core;

internal class TwinEqualityComparer : IEqualityComparer<BasicDigitalTwin>
{
    /// <inheritdoc/>
    public bool Equals(BasicDigitalTwin? x, BasicDigitalTwin? y)
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
            var idAndMetadata = string.Equals(x?.Metadata?.ModelId, y?.Metadata?.ModelId) && string.Equals(x?.Id, y?.Id);
            if (x?.Contents != null && y?.Contents != null)
            {
                var contentsEquals = x.Contents.Count == y.Contents.Count && x.Contents.Except(y.Contents).Any();
                return idAndMetadata && contentsEquals;
            }

            return false;
        }

        return false;
    }

    /// <inheritdoc/>
    public int GetHashCode(BasicDigitalTwin obj)
    {
#if NETSTANDARD2_1_OR_GREATER
        return HashCode.Combine(obj.Id);
#else
        return obj.Id.GetHashCode();
#endif
    }
}