// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace QueryBuilder.UnitTests
{
    using System;
    using System.Text.Json.Serialization;
    using Azure.DigitalTwins.Core;

    public class Device : BasicDigitalTwin
    {
        public Device()
        {
            Metadata.ModelId = ModelId;
        }

        [JsonIgnore]
        public static string ModelId { get; } = "dtmi:microsoft:Device;1";

        [JsonPropertyName("externalId")]
        public string ExternalId { get; set; }

        [JsonPropertyName("facilityLinkId")]
        public string FacilityLinkId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("regionId")]
        public string RegionId { get; set; }

        [JsonPropertyName("buildingId")]
        public string BuildingId { get; set; }

        [JsonPropertyName("floorId")]
        public string FloorId { get; set; }

        [JsonPropertyName("manufacturerName")]
        public string ManufacturerName { get; set; }

        [JsonPropertyName("brandName")]
        public string BrandName { get; set; }

        [JsonPropertyName("hardwareId")]
        public string HardwareId { get; set; }

        [JsonPropertyName("firmwareVersion")]
        public string FirmwareVersion { get; set; }

        [JsonPropertyName("createdDate")]
        public DateTime? CreatedDate { get; set; }

        [JsonPropertyName("lastModifiedDate")]
        public DateTime? LastModifiedDate { get; set; }

        [JsonIgnore]
        public DeviceHasSensorsRelationship HasSensors { get; private set; } = new DeviceHasSensorsRelationship();
    }
}