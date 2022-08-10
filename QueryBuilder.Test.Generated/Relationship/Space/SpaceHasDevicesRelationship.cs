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

    public class SpaceHasDevicesRelationship : Relationship<Device>, IEquatable<SpaceHasDevicesRelationship>
    {
        public SpaceHasDevicesRelationship()
        {
            Name = "hasDevices";
        }

        public SpaceHasDevicesRelationship(Space source, Device target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as SpaceHasDevicesRelationship);
        }

        public bool Equals(SpaceHasDevicesRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(SpaceHasDevicesRelationship? left, SpaceHasDevicesRelationship? right)
        {
            return EqualityComparer<SpaceHasDevicesRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(SpaceHasDevicesRelationship? left, SpaceHasDevicesRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as SpaceHasDevicesRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}