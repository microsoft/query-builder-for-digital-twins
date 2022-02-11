// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace QueryBuilder.UnitTests
{
    using System.Text.Json.Serialization;

    public class Floor : Space
    {
        public Floor()
        {
            Metadata.ModelId = ModelId;
        }

        [JsonIgnore]
        public static new string ModelId { get; } = "dtmi:microsoft:Space:Floor;1";

        [JsonPropertyName("logicalOrder")]
        public int? LogicalOrder { get; set; }

        [JsonPropertyName("regionId")]
        public string RegionId { get; set; }
    }
}