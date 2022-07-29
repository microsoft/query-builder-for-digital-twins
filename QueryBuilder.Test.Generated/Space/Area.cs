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

    public class Area : Space, IEquatable<Area>
    {
        public Area()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static new string ModelId { get; } = "dtmi:test:Space:Area;1";
        [JsonPropertyName("floorId")]
        public string? FloorId { get; set; }
        [JsonPropertyName("buildingId")]
        public string? BuildingId { get; set; }
        [JsonPropertyName("regionId")]
        public string? RegionId { get; set; }
        public override bool Equals(object? obj)
        {
            return Equals(obj as Area);
        }

        public bool Equals(Area? other)
        {
            return other is not null && base.Equals(other) && FloorId == other.FloorId && BuildingId == other.BuildingId && RegionId == other.RegionId;
        }

        public static bool operator ==(Area? left, Area? right)
        {
            return EqualityComparer<Area?>.Default.Equals(left, right);
        }

        public static bool operator !=(Area? left, Area? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(base.GetHashCode(), FloorId?.GetHashCode(), BuildingId?.GetHashCode(), RegionId?.GetHashCode());
        }
    }
}