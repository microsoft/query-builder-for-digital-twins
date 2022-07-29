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

    public class Sensor : BasicDigitalTwin, IEquatable<Sensor>, IEquatable<BasicDigitalTwin>
    {
        public Sensor()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static string ModelId { get; } = "dtmi:test:Sensor;1";
        public override bool Equals(object? obj)
        {
            return Equals(obj as Sensor);
        }

        public bool Equals(Sensor? other)
        {
            return other is not null && Id == other.Id && Metadata.ModelId == other.Metadata.ModelId;
        }

        public static bool operator ==(Sensor? left, Sensor? right)
        {
            return EqualityComparer<Sensor?>.Default.Equals(left, right);
        }

        public static bool operator !=(Sensor? left, Sensor? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), Metadata?.ModelId?.GetHashCode());
        }

        public bool Equals(BasicDigitalTwin? other)
        {
            return Equals(other as Sensor) || new TwinEqualityComparer().Equals(this, other);
        }
    }
}