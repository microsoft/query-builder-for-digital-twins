// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace QueryBuilder.UnitTests
{
    using System.Text.Json.Serialization;

    public class ConferenceRoom : Space
    {
        public ConferenceRoom()
        {
            Metadata.ModelId = ModelId;
        }

        [JsonIgnore]
        public static new string ModelId { get; } = "dtmi:microsoft:Space:ConferenceRoom;1";
    }
}