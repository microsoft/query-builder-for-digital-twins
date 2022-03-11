// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace QueryBuilder.UnitTests
{
    using System.Text.Json.Serialization;
    using Azure.DigitalTwins.Core;

    public class BuildingHasITSiteFunctionRelationship : BasicRelationship
    {
        public BuildingHasITSiteFunctionRelationship()
        {
            Name = "hasITSiteFunction";
        }

        [JsonPropertyName("maxPriority")]
        public int? MaxPriority { get; set; }
    }
}