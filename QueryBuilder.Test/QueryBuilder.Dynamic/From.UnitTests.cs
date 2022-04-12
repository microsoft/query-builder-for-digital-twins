// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace QueryBuilder.UnitTests.QueryBuilder.Dynamic
{
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class QueryFromTest
    {
        [TestMethod]
        public void FromGeneratesDefaultSelect()
        {
            var query = QueryBuilder.FromTwins();
            Assert.AreEqual($"SELECT twin FROM DIGITALTWINS twin", query.BuildAdtQuery());
        }

        [TestMethod]
        public void CanAssignAlias()
        {
            var query = QueryBuilder.FromTwins("bldng");
            Assert.AreEqual($"SELECT bldng FROM DIGITALTWINS bldng", query.BuildAdtQuery());
        }

        [TestMethod]
        public void CanAssignAliasWithRelationshipsQuery()
        {
            var query = QueryBuilder.FromRelationships("rel");
            Assert.AreEqual("SELECT rel FROM RELATIONSHIPS rel", query.BuildAdtQuery());
        }
    }
}