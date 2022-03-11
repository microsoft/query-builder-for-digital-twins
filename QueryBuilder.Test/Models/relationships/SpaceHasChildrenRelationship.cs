// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace QueryBuilder.UnitTests
{
    using Azure.DigitalTwins.Core;

    public class SpaceHasChildrenRelationship : BasicRelationship
    {
        public SpaceHasChildrenRelationship()
        {
            Name = "hasChildren";
        }
    }
}