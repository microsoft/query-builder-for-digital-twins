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

    public class Device : BasicDigitalTwin, IEquatable<Device>, IEquatable<BasicDigitalTwin>
    {
        public Device()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static string ModelId { get; } = "dtmi:test:Device;1";
        [JsonPropertyName("externalId")]
        public string? ExternalId { get; set; }
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("description")]
        public string? Description { get; set; }
        [JsonIgnore]
        public DeviceHasSensorsRelationshipCollection HasSensors { get; set; } = new DeviceHasSensorsRelationshipCollection();
        public override bool Equals(object? obj)
        {
            return Equals(obj as Device);
        }

        public bool Equals(Device? other)
        {
            return other is not null && Id == other.Id && Metadata.ModelId == other.Metadata.ModelId && ExternalId == other.ExternalId && Name == other.Name && Description == other.Description;
        }

        public static bool operator ==(Device? left, Device? right)
        {
            return EqualityComparer<Device?>.Default.Equals(left, right);
        }

        public static bool operator !=(Device? left, Device? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), Metadata?.ModelId?.GetHashCode(), ExternalId?.GetHashCode(), Name?.GetHashCode(), Description?.GetHashCode());
        }

        public bool Equals(BasicDigitalTwin? other)
        {
            return Equals(other as Device) || new TwinEqualityComparer().Equals(this, other);
        }
    }
}