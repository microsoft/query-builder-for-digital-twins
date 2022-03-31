// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace QueryBuilder.UnitTests.QueryBuilder.Dynamic
{
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Dynamic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class QueryFromTest
    {
        [TestMethod]
        public void FromGeneratesDefaultSelect()
        {
            var query = DynamicQueryBuilder.FromTwins();
            Assert.AreEqual($"SELECT twin FROM DIGITALTWINS twin", query.BuildAdtQuery());
        }

        [TestMethod]
        public void CanAssignAlias()
        {
            var query = DynamicQueryBuilder.FromTwins("bldng");
            Assert.AreEqual($"SELECT bldng FROM DIGITALTWINS bldng", query.BuildAdtQuery());
        }
    }
}