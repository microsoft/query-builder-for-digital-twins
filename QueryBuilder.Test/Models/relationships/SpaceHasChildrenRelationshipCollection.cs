// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace QueryBuilder.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;

    public class SpaceHasChildrenRelationshipCollection : RelationshipCollection<SpaceHasChildrenRelationship, Space>
    {
        public SpaceHasChildrenRelationshipCollection(IEnumerable<SpaceHasChildrenRelationship>? relationships = default) : base(relationships ?? Enumerable.Empty<SpaceHasChildrenRelationship>())
        {
        }
    }
}