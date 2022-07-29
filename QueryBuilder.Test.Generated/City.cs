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

    public class City : BasicDigitalTwin, IEquatable<City>, IEquatable<BasicDigitalTwin>
    {
        public City()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static string ModelId { get; } = "dtmi:test:City;1";
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("code")]
        public string? Code { get; set; }
        public override bool Equals(object? obj)
        {
            return Equals(obj as City);
        }

        public bool Equals(City? other)
        {
            return other is not null && Id == other.Id && Metadata.ModelId == other.Metadata.ModelId && Name == other.Name && Code == other.Code;
        }

        public static bool operator ==(City? left, City? right)
        {
            return EqualityComparer<City?>.Default.Equals(left, right);
        }

        public static bool operator !=(City? left, City? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), Metadata?.ModelId?.GetHashCode(), Name?.GetHashCode(), Code?.GetHashCode());
        }

        public bool Equals(BasicDigitalTwin? other)
        {
            return Equals(other as City) || new TwinEqualityComparer().Equals(this, other);
        }
    }
}