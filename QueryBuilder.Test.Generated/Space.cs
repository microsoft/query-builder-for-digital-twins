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

    public class Space : BasicDigitalTwin, IEquatable<Space>, IEquatable<BasicDigitalTwin>
    {
        public Space()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static string ModelId { get; } = "dtmi:test:Space;1";
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("roomKey")]
        public string? RoomKey { get; set; }
        [JsonPropertyName("friendlyName")]
        public string? FriendlyName { get; set; }
        [JsonPropertyName("description")]
        public string? Description { get; set; }
        [JsonPropertyName("squareFootArea")]
        public float? SquareFootArea { get; set; }
        [JsonPropertyName("capabilities")]
        public IDictionary<string, bool>? Capabilities { get; set; }
        [JsonPropertyName("status")]
        public SpaceStatus? Status { get; set; }
        [JsonIgnore]
        public SpaceHasChildrenRelationshipCollection HasChildren { get; set; } = new SpaceHasChildrenRelationshipCollection();
        [JsonIgnore]
        public SpaceHasParentRelationshipCollection HasParent { get; set; } = new SpaceHasParentRelationshipCollection();
        [JsonIgnore]
        public SpaceHasDevicesRelationshipCollection HasDevices { get; set; } = new SpaceHasDevicesRelationshipCollection();
        public override bool Equals(object? obj)
        {
            return Equals(obj as Space);
        }

        public bool Equals(Space? other)
        {
            return other is not null && Id == other.Id && Metadata.ModelId == other.Metadata.ModelId && Name == other.Name && RoomKey == other.RoomKey && FriendlyName == other.FriendlyName && Description == other.Description && SquareFootArea == other.SquareFootArea && Capabilities == other.Capabilities && Status == other.Status;
        }

        public static bool operator ==(Space? left, Space? right)
        {
            return EqualityComparer<Space?>.Default.Equals(left, right);
        }

        public static bool operator !=(Space? left, Space? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), Metadata?.ModelId?.GetHashCode(), Name?.GetHashCode(), RoomKey?.GetHashCode(), FriendlyName?.GetHashCode(), Description?.GetHashCode(), SquareFootArea?.GetHashCode(), Capabilities?.GetHashCode(), Status?.GetHashCode());
        }

        public bool Equals(BasicDigitalTwin? other)
        {
            return Equals(other as Space) || new TwinEqualityComparer().Equals(this, other);
        }
    }
}