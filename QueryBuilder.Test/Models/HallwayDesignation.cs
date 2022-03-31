// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace QueryBuilder.UnitTests
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum HallwayDesignation
    {
        [EnumMember(Value = "Primary"), Display(Name = "Primary")]
        Primary,
        [EnumMember(Value = "Secondary"), Display(Name = "Secondary")]
        Secondary
    }
}