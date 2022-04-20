// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace QueryBuilder.UnitTests
{
    using System.Text.Json.Serialization;

    public class Hallway : Space
    {
        public Hallway()
        {
            Metadata.ModelId = ModelId;
        }

        [JsonIgnore]
        public static new string ModelId { get; } = "dtmi:microsoft:Space:Hallway;1";

        private const string designation = nameof(designation);

        [JsonPropertyName(designation)]
        public HallwayDesignation? Designation { get; set; }
    }
}