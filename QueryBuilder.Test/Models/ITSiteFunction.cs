// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace QueryBuilder.UnitTests
{
    using System.Text.Json.Serialization;
    using Azure.DigitalTwins.Core;

    public class ITSiteFunction : BasicDigitalTwin
    {
        public ITSiteFunction()
        {
            Metadata.ModelId = ModelId;
        }

        [JsonIgnore]
        public static string ModelId { get; } = "dtmi:microsoft:ITSiteFunction;1";

        [JsonPropertyName("externalId")]
        public int? ExternalId { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}