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

    public class WBuilding : Building, IEquatable<WBuilding>
    {
        public WBuilding()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static new string ModelId { get; } = "dtmi:test:Space:Building:WBuilding;1";
        [JsonPropertyName("limit")]
        public int? Limit { get; set; }
        [JsonPropertyName("landlordName")]
        public string? LandlordName { get; set; }
        public override bool Equals(object? obj)
        {
            return Equals(obj as WBuilding);
        }

        public bool Equals(WBuilding? other)
        {
            return other is not null && base.Equals(other) && Limit == other.Limit && LandlordName == other.LandlordName;
        }

        public static bool operator ==(WBuilding? left, WBuilding? right)
        {
            return EqualityComparer<WBuilding?>.Default.Equals(left, right);
        }

        public static bool operator !=(WBuilding? left, WBuilding? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(base.GetHashCode(), Limit?.GetHashCode(), LandlordName?.GetHashCode());
        }
    }
}