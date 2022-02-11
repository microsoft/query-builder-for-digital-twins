// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace QueryBuilder.UnitTests
{
    using System.Text.Json.Serialization;
    using Azure.DigitalTwins.Core;

    public class BuildingHasBuildingContactRelationship : BasicRelationship
    {
        public BuildingHasBuildingContactRelationship()
        {
            Name = "hasBuildingContact";
        }

        [JsonPropertyName("contactType")]
        public string ContactType { get; set; }

        [JsonPropertyName("comments")]
        public string Comments { get; set; }

        [JsonPropertyName("externalId")]
        public int? ExternalId { get; set; }
    }
}