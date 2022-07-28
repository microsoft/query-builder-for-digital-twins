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

    public class AddressHasCountryRelationship : Relationship<Country>, IEquatable<AddressHasCountryRelationship>
    {
        public AddressHasCountryRelationship()
        {
            Name = "hasCountry";
        }

        public AddressHasCountryRelationship(Address source, Country target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as AddressHasCountryRelationship);
        }

        public bool Equals(AddressHasCountryRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(AddressHasCountryRelationship? left, AddressHasCountryRelationship? right)
        {
            return EqualityComparer<AddressHasCountryRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(AddressHasCountryRelationship? left, AddressHasCountryRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as AddressHasCountryRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}