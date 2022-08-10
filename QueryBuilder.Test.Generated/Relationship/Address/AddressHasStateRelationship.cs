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

    public class AddressHasStateRelationship : Relationship<StateProvince>, IEquatable<AddressHasStateRelationship>
    {
        public AddressHasStateRelationship()
        {
            Name = "hasState";
        }

        public AddressHasStateRelationship(Address source, StateProvince target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as AddressHasStateRelationship);
        }

        public bool Equals(AddressHasStateRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(AddressHasStateRelationship? left, AddressHasStateRelationship? right)
        {
            return EqualityComparer<AddressHasStateRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(AddressHasStateRelationship? left, AddressHasStateRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as AddressHasStateRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}