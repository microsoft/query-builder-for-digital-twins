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

    public class AddressHasStateRelationshipCollection : RelationshipCollection<AddressHasStateRelationship, StateProvince>
    {
        public AddressHasStateRelationshipCollection(IEnumerable<AddressHasStateRelationship>? relationships = default) : base(relationships ?? Enumerable.Empty<AddressHasStateRelationship>())
        {
        }
    }
}