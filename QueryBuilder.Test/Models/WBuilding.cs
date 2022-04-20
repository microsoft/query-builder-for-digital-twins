// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace QueryBuilder.UnitTests
{
    using System.Text.Json.Serialization;

    public class WBuilding : Building
    {
        public WBuilding()
        {
            Metadata.ModelId = ModelId;
        }

        [JsonIgnore]
        public static new string ModelId { get; } = "dtmi:microsoft:Space:Building:WBuilding;1";

        [JsonPropertyName("limit")]
        public int? Limit { get; set; }

        [JsonPropertyName("landlordName")]
        public string LandlordName { get; set; }
    }
}