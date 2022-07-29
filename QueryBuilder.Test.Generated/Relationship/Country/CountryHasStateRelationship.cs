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

    public class CountryHasStateRelationship : Relationship<StateProvince>, IEquatable<CountryHasStateRelationship>
    {
        public CountryHasStateRelationship()
        {
            Name = "hasState";
        }

        public CountryHasStateRelationship(Country source, StateProvince target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as CountryHasStateRelationship);
        }

        public bool Equals(CountryHasStateRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(CountryHasStateRelationship? left, CountryHasStateRelationship? right)
        {
            return EqualityComparer<CountryHasStateRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(CountryHasStateRelationship? left, CountryHasStateRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as CountryHasStateRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}