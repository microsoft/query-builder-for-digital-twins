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

    public class AddressHasCityRelationship : Relationship<City>, IEquatable<AddressHasCityRelationship>
    {
        public AddressHasCityRelationship()
        {
            Name = "hasCity";
        }

        public AddressHasCityRelationship(Address source, City target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as AddressHasCityRelationship);
        }

        public bool Equals(AddressHasCityRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(AddressHasCityRelationship? left, AddressHasCityRelationship? right)
        {
            return EqualityComparer<AddressHasCityRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(AddressHasCityRelationship? left, AddressHasCityRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as AddressHasCityRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}