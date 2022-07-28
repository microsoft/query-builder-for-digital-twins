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

    public class AddressHasCityRelationshipCollection : RelationshipCollection<AddressHasCityRelationship, City>
    {
        public AddressHasCityRelationshipCollection(IEnumerable<AddressHasCityRelationship>? relationships = default) : base(relationships ?? Enumerable.Empty<AddressHasCityRelationship>())
        {
        }
    }
}