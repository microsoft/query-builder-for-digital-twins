// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace QueryBuilder.UnitTests
{
    using System.Text.Json.Serialization;
    using Azure.DigitalTwins.Core;

    public class Sensor : BasicDigitalTwin
    {
        public Sensor()
        {
            Metadata.ModelId = ModelId;
        }

        [JsonIgnore]
        public static string ModelId { get; } = "dtmi:microsoft:Sensor;1";

        [JsonPropertyName("port")]
        public string Port { get; set; }

        [JsonPropertyName("pollRate")]
        public int? PollRate { get; set; }

        [JsonPropertyName("basicDeltaProcessingRefreshTime")]
        public int? BasicDeltaProcessingRefreshTime { get; set; }

        [JsonPropertyName("manufacturerName")]
        public string ManufacturerName { get; set; }

        [JsonPropertyName("brandName")]
        public string BrandName { get; set; }

        [JsonPropertyName("regionId")]
        public string RegionId { get; set; }

        [JsonPropertyName("buildingId")]
        public string BuildingId { get; set; }

        [JsonPropertyName("floorId")]
        public string FloorId { get; set; }
    }
}