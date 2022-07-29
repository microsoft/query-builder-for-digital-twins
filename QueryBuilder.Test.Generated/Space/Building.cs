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

    public class Building : Space, IEquatable<Building>
    {
        public Building()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static new string ModelId { get; } = "dtmi:test:Space:Building;1";
        [JsonPropertyName("businessEntityNumber")]
        public string? BusinessEntityNumber { get; set; }
        [JsonPropertyName("number")]
        public int? Number { get; set; }
        [JsonPropertyName("shortName")]
        public string? ShortName { get; set; }
        [JsonPropertyName("squareMeter")]
        public float? SquareMeter { get; set; }
        [JsonPropertyName("rationalSortKey")]
        public string? RationalSortKey { get; set; }
        [JsonPropertyName("regionId")]
        public string? RegionId { get; set; }
        [JsonPropertyName("startOfBusinessTime")]
        public object? StartOfBusinessTime { get; set; }
        [JsonPropertyName("endOfBusinessTime")]
        public object? EndOfBusinessTime { get; set; }
        [JsonPropertyName("businessEntityName")]
        public string? BusinessEntityName { get; set; }
        [JsonPropertyName("amenities")]
        public IDictionary<string, bool>? Amenities { get; set; }
        [JsonIgnore]
        public BuildingHasAddressRelationshipCollection HasAddress { get; set; } = new BuildingHasAddressRelationshipCollection();
        [JsonIgnore]
        public BuildingHasBuildingContactRelationshipCollection HasBuildingContact { get; set; } = new BuildingHasBuildingContactRelationshipCollection();
        public override bool Equals(object? obj)
        {
            return Equals(obj as Building);
        }

        public bool Equals(Building? other)
        {
            return other is not null && base.Equals(other) && BusinessEntityNumber == other.BusinessEntityNumber && Number == other.Number && ShortName == other.ShortName && SquareMeter == other.SquareMeter && RationalSortKey == other.RationalSortKey && RegionId == other.RegionId && StartOfBusinessTime == other.StartOfBusinessTime && EndOfBusinessTime == other.EndOfBusinessTime && BusinessEntityName == other.BusinessEntityName && Amenities == other.Amenities;
        }

        public static bool operator ==(Building? left, Building? right)
        {
            return EqualityComparer<Building?>.Default.Equals(left, right);
        }

        public static bool operator !=(Building? left, Building? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(base.GetHashCode(), BusinessEntityNumber?.GetHashCode(), Number?.GetHashCode(), ShortName?.GetHashCode(), SquareMeter?.GetHashCode(), RationalSortKey?.GetHashCode(), RegionId?.GetHashCode(), StartOfBusinessTime?.GetHashCode(), EndOfBusinessTime?.GetHashCode(), BusinessEntityName?.GetHashCode(), Amenities?.GetHashCode());
        }
    }
}