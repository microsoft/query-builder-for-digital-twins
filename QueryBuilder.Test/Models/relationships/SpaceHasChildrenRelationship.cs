// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace QueryBuilder.UnitTests
{
    using System;
    using System.Collections.Generic;
    using Azure.DigitalTwins.Core;

    public class SpaceHasChildrenRelationship : Relationship<Space>, IEquatable<SpaceHasChildrenRelationship>
    {
        public SpaceHasChildrenRelationship()
        {
            Name = "hasChildren";
        }

        public SpaceHasChildrenRelationship(Space source, Space target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as SpaceHasChildrenRelationship);
        }

        public bool Equals(SpaceHasChildrenRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(SpaceHasChildrenRelationship? left, SpaceHasChildrenRelationship? right)
        {
            return EqualityComparer<SpaceHasChildrenRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(SpaceHasChildrenRelationship? left, SpaceHasChildrenRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as SpaceHasChildrenRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}