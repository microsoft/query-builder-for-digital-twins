// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace QueryBuilder.Test.Generated
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;
    using System.Linq;

    public class DeviceHasSensorsRelationshipCollection : RelationshipCollection<DeviceHasSensorsRelationship, Sensor>
    {
        public DeviceHasSensorsRelationshipCollection(IEnumerable<DeviceHasSensorsRelationship>? relationships = default) : base(relationships ?? Enumerable.Empty<DeviceHasSensorsRelationship>())
        {
        }
    }
}