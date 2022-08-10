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

    public class Room : Area, IEquatable<Room>
    {
        public Room()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static new string ModelId { get; } = "dtmi:test:Space:Area:Room;1";
        [JsonPropertyName("seatCount")]
        public int? SeatCount { get; set; }
        [JsonPropertyName("roomCapacity")]
        public int? RoomCapacity { get; set; }
        public override bool Equals(object? obj)
        {
            return Equals(obj as Room);
        }

        public bool Equals(Room? other)
        {
            return other is not null && base.Equals(other) && SeatCount == other.SeatCount && RoomCapacity == other.RoomCapacity;
        }

        public static bool operator ==(Room? left, Room? right)
        {
            return EqualityComparer<Room?>.Default.Equals(left, right);
        }

        public static bool operator !=(Room? left, Room? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(base.GetHashCode(), SeatCount?.GetHashCode(), RoomCapacity?.GetHashCode());
        }
    }
}