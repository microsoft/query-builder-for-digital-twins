// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace QueryBuilder.UnitTests.QueryBuilder.Typed
{
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class QueryCountTest
    {
        [TestMethod]
        public void CountIsAddedWithDefaultSelect()
        {
            var query = QueryBuilder
                .From<Building>()
                .Count();

            Assert.AreEqual($"SELECT COUNT() FROM DIGITALTWINS building WHERE IS_OF_MODEL(building, '{Building.ModelId.UpdateVersion(1)}')", query.BuildAdtQuery());
        }

        [TestMethod]
        public void CountCanAllowWhereConditions()
        {
            var query = QueryBuilder
                .From<Building>()
                .Count()
                .Where<Building>(b => b.Id, ComparisonOperators.IsEqualTo, "ID");

            Assert.AreEqual($"SELECT COUNT() FROM DIGITALTWINS building WHERE IS_OF_MODEL(building, '{Building.ModelId.UpdateVersion(1)}') AND building.$dtId = 'ID'", query.BuildAdtQuery());
        }

        [TestMethod]
        public void CanCountAll()
        {
            Assert.AreEqual($"SELECT COUNT() FROM DIGITALTWINS", QueryBuilder.CountAllDigitalTwins().BuildAdtQuery());
            Assert.AreEqual($"SELECT COUNT() FROM DIGITALTWINS", new CountAllQuery().BuildAdtQuery());
        }
    }
}