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

    public class Amenities : BasicDigitalTwin, IEquatable<Amenities>, IEquatable<BasicDigitalTwin>
    {
        public Amenities()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static string ModelId { get; } = "dtmi:test:Amenities;1";
        [JsonPropertyName("description")]
        public string? Description { get; set; }
        public override bool Equals(object? obj)
        {
            return Equals(obj as Amenities);
        }

        public bool Equals(Amenities? other)
        {
            return other is not null && Id == other.Id && Metadata.ModelId == other.Metadata.ModelId && Description == other.Description;
        }

        public static bool operator ==(Amenities? left, Amenities? right)
        {
            return EqualityComparer<Amenities?>.Default.Equals(left, right);
        }

        public static bool operator !=(Amenities? left, Amenities? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), Metadata?.ModelId?.GetHashCode(), Description?.GetHashCode());
        }

        public bool Equals(BasicDigitalTwin? other)
        {
            return Equals(other as Amenities) || new TwinEqualityComparer().Equals(this, other);
        }
    }
}