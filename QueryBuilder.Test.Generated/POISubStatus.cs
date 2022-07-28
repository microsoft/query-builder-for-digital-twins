// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace QueryBuilder.Test.Generated
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum POISubStatus
    {
        [EnumMember(Value = "Available"), Display(Name = "Available", Description = "available"), SourceValue(Value = "1")]
        Available,
        [EnumMember(Value = "Unavailable"), Display(Name = "Unavailable", Description = "unavailable"), SourceValue(Value = "2")]
        Unavailable
    }
}