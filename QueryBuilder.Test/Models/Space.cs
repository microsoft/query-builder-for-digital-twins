// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace QueryBuilder.UnitTests
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;
    using global::Azure.DigitalTwins.Core;

    public class Space : BasicDigitalTwin
    {
        public Space()
        {
            Metadata.ModelId = ModelId;
        }

        [JsonIgnore]
        public static string ModelId { get; } = "dtmi:microsoft:Space;1";

        [JsonPropertyName("externalId")]
        public int? ExternalId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("roomKey")]
        public string RoomKey { get; set; }

        [JsonPropertyName("friendlyName")]
        public string FriendlyName { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("squareFootArea")]
        public float? SquareFootArea { get; set; }

        [JsonPropertyName("capabilities")]
        public IDictionary<string, bool> Capabilities { get; set; }

        [JsonPropertyName("status")]
        public SpaceStatus Status { get; set; }

        [JsonIgnore]
        public SpaceHasChildrenRelationship HasChildren { get; private set; } = new SpaceHasChildrenRelationship();

        [JsonIgnore]
        public SpaceHasDevicesRelationship HasDevices { get; private set; } = new SpaceHasDevicesRelationship();
    }
}