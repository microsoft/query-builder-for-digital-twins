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

    public class CountryHasCityRelationshipCollection : RelationshipCollection<CountryHasCityRelationship, City>
    {
        public CountryHasCityRelationshipCollection(IEnumerable<CountryHasCityRelationship>? relationships = default) : base(relationships ?? Enumerable.Empty<CountryHasCityRelationship>())
        {
        }
    }
}