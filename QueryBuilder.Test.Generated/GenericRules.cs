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

    [Serializable]
    public class GenericRules : IEquatable<GenericRules>
    {
        [JsonPropertyName("isSmokingAllowed")]
        public bool? IsSmokingAllowed { get; set; }
        [JsonPropertyName("isAlcoholAllowed")]
        public bool? IsAlcoholAllowed { get; set; }
        [JsonPropertyName("isFireAllowed")]
        public bool? IsFireAllowed { get; set; }
        [JsonPropertyName("isFeedingAnimalsAllowed")]
        public bool? IsFeedingAnimalsAllowed { get; set; }
        [JsonPropertyName("isBroomAvailable")]
        public bool? IsBroomAvailable { get; set; }
        public override bool Equals(object? obj)
        {
            return Equals(obj as GenericRules);
        }

        public bool Equals(GenericRules? other)
        {
            return other is not null && IsSmokingAllowed == other.IsSmokingAllowed && IsAlcoholAllowed == other.IsAlcoholAllowed && IsFireAllowed == other.IsFireAllowed && IsFeedingAnimalsAllowed == other.IsFeedingAnimalsAllowed && IsBroomAvailable == other.IsBroomAvailable;
        }

        public static bool operator ==(GenericRules? left, GenericRules? right)
        {
            return EqualityComparer<GenericRules?>.Default.Equals(left, right);
        }

        public static bool operator !=(GenericRules? left, GenericRules? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(IsSmokingAllowed?.GetHashCode(), IsAlcoholAllowed?.GetHashCode(), IsFireAllowed?.GetHashCode(), IsFeedingAnimalsAllowed?.GetHashCode(), IsBroomAvailable?.GetHashCode());
        }
    }
}