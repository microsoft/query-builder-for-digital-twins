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

    public class BuildingHasBuildingContactRelationship : Relationship<Employee>, IEquatable<BuildingHasBuildingContactRelationship>
    {
        public BuildingHasBuildingContactRelationship()
        {
            Name = "hasBuildingContact";
        }

        public BuildingHasBuildingContactRelationship(Building source, Employee target) : this()
        {
            InitializeFromTwins(source, target);
        }

        [JsonPropertyName("contactType")]
        public string? ContactType { get; set; }
        [JsonPropertyName("comments")]
        public string? Comments { get; set; }
        [JsonPropertyName("externalId")]
        public int? ExternalId { get; set; }
        public override bool Equals(object? obj)
        {
            return Equals(obj as BuildingHasBuildingContactRelationship);
        }

        public bool Equals(BuildingHasBuildingContactRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name && ContactType == other.ContactType && Comments == other.Comments && ExternalId == other.ExternalId;
        }

        public static bool operator ==(BuildingHasBuildingContactRelationship? left, BuildingHasBuildingContactRelationship? right)
        {
            return EqualityComparer<BuildingHasBuildingContactRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(BuildingHasBuildingContactRelationship? left, BuildingHasBuildingContactRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode(), ContactType?.GetHashCode(), Comments?.GetHashCode(), ExternalId?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as BuildingHasBuildingContactRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}