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

    public class POI : Area, IEquatable<POI>
    {
        public POI()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static new string ModelId { get; } = "dtmi:test:Space:Area:POI;1";
        [JsonPropertyName("category")]
        public Category? Category { get; set; }
        [JsonPropertyName("genericRules")]
        public GenericRules? GenericRules { get; set; }
        [JsonPropertyName("scheduleRules")]
        public ScheduleRules? ScheduleRules { get; set; }
        [JsonPropertyName("amenities")]
        public IDictionary<string, bool>? Amenities { get; set; }
        [JsonPropertyName("weeklyOperationHours")]
        public IDictionary<string, OperationHour>? WeeklyOperationHours { get; set; }
        [JsonPropertyName("subStatus")]
        public POISubStatus? SubStatus { get; set; }
        [JsonPropertyName("mediaList")]
        public IDictionary<string, Media>? MediaList { get; set; }
        public override bool Equals(object? obj)
        {
            return Equals(obj as POI);
        }

        public bool Equals(POI? other)
        {
            return other is not null && base.Equals(other) && Category == other.Category && GenericRules == other.GenericRules && ScheduleRules == other.ScheduleRules && Amenities == other.Amenities && WeeklyOperationHours == other.WeeklyOperationHours && SubStatus == other.SubStatus && MediaList == other.MediaList;
        }

        public static bool operator ==(POI? left, POI? right)
        {
            return EqualityComparer<POI?>.Default.Equals(left, right);
        }

        public static bool operator !=(POI? left, POI? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(base.GetHashCode(), Category?.GetHashCode(), GenericRules?.GetHashCode(), ScheduleRules?.GetHashCode(), Amenities?.GetHashCode(), WeeklyOperationHours?.GetHashCode(), SubStatus?.GetHashCode(), MediaList?.GetHashCode());
        }
    }
}