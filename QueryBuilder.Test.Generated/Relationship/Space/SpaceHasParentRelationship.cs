// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace QueryBuilder.Test.Generated
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class SpaceHasParentRelationship : Relationship<Space>, IEquatable<SpaceHasParentRelationship>
    {
        public SpaceHasParentRelationship()
        {
            Name = "hasParent";
        }

        public SpaceHasParentRelationship(Space source, Space target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as SpaceHasParentRelationship);
        }

        public bool Equals(SpaceHasParentRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(SpaceHasParentRelationship? left, SpaceHasParentRelationship? right)
        {
            return EqualityComparer<SpaceHasParentRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(SpaceHasParentRelationship? left, SpaceHasParentRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as SpaceHasParentRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}