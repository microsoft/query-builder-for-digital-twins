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

    public class StateProvince : BasicDigitalTwin, IEquatable<StateProvince>, IEquatable<BasicDigitalTwin>
    {
        public StateProvince()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static string ModelId { get; } = "dtmi:test:StateProvince;30";
        [JsonPropertyName("code")]
        public string? Code { get; set; }
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        public override bool Equals(object? obj)
        {
            return Equals(obj as StateProvince);
        }

        public bool Equals(StateProvince? other)
        {
            return other is not null && Id == other.Id && Metadata.ModelId == other.Metadata.ModelId && Code == other.Code && Name == other.Name;
        }

        public static bool operator ==(StateProvince? left, StateProvince? right)
        {
            return EqualityComparer<StateProvince?>.Default.Equals(left, right);
        }

        public static bool operator !=(StateProvince? left, StateProvince? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), Metadata?.ModelId?.GetHashCode(), Code?.GetHashCode(), Name?.GetHashCode());
        }

        public bool Equals(BasicDigitalTwin? other)
        {
            return Equals(other as StateProvince) || new TwinEqualityComparer().Equals(this, other);
        }
    }
}