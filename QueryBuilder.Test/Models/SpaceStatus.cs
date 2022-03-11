// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace QueryBuilder.UnitTests
{
    using System.Runtime.Serialization;

    public enum SpaceStatus
    {
        [EnumMember(Value = "Active")]
        Active,
        [EnumMember(Value = "Inactive")]
        Inactive,
        [EnumMember(Value = "Pending")]
        Pending,
        [EnumMember(Value = "UnderConstruction")]
        UnderConstruction
    }
}