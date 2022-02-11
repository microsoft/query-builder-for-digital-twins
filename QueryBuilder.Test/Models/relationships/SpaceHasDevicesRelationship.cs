// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace QueryBuilder.UnitTests
{
    using Azure.DigitalTwins.Core;

    public class SpaceHasDevicesRelationship : BasicRelationship
    {
        public SpaceHasDevicesRelationship()
        {
            Name = "hasDevices";
        }
    }
}