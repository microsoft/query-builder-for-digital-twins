// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace QueryBuilder.UnitTests
{
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class QueryTopTest
    {
        [TestMethod]
        public void TopIsAddedWithDefaultSelect()
        {
            var query = QueryBuilder
                .From<Building>()
                .Top(1);

            Assert.AreEqual($"SELECT TOP(1) building FROM DIGITALTWINS building WHERE IS_OF_MODEL(building, '{Building.ModelId.UpdateVersion(1)}')", query.BuildAdtQuery());
        }

        [TestMethod]
        public void TopIsAddedWithOneSelect()
        {
            var query = QueryBuilder
                .From<Building>()
                .Select<Building>()
                .Top(1);

            Assert.AreEqual($"SELECT TOP(1) building FROM DIGITALTWINS building WHERE IS_OF_MODEL(building, '{Building.ModelId.UpdateVersion(1)}')", query.BuildAdtQuery());
        }

        [TestMethod]
        public void LastTopTakesPrecedence()
        {
            var query = QueryBuilder
                .From<Building>()
                .Select<Building>()
                .Top(1)
                .Top(2);

            Assert.AreEqual($"SELECT TOP(2) building FROM DIGITALTWINS building WHERE IS_OF_MODEL(building, '{Building.ModelId.UpdateVersion(1)}')", query.BuildAdtQuery());
        }

        [TestMethod]
        public void TopIsAddedWithTwoSelects()
        {
            var query = QueryBuilder
                .From<Building>()
                .Join<Building, Floor>(b => b.HasChildren)
                .Select<Building>()
                .Select<Floor>()
                .Where<Building>(b => b.Id, ComparisonOperators.IsEqualTo, "ID")
                .Top(1);

            Assert.AreEqual($"SELECT TOP(1) building, floor FROM DIGITALTWINS building JOIN floor RELATED building.hasChildren spacehaschildrenrelationship WHERE IS_OF_MODEL(building, '{Building.ModelId.UpdateVersion(1)}') AND IS_OF_MODEL(floor, '{Floor.ModelId.UpdateVersion(1)}') AND building.$dtId = 'ID'", query.BuildAdtQuery());
        }

        [TestMethod]
        public void CanSelectTwiceAfterTop()
        {
            var query = QueryBuilder
                .From<Building>()
                .Join<Building, Floor>(b => b.HasChildren)
                .Where<Building>(b => b.Id, ComparisonOperators.IsEqualTo, "ID")
                .Top(1)
                .Select<Building>()
                .Select<Floor>();

            Assert.AreEqual($"SELECT TOP(1) building, floor FROM DIGITALTWINS building JOIN floor RELATED building.hasChildren spacehaschildrenrelationship WHERE IS_OF_MODEL(building, '{Building.ModelId.UpdateVersion(1)}') AND IS_OF_MODEL(floor, '{Floor.ModelId.UpdateVersion(1)}') AND building.$dtId = 'ID'", query.BuildAdtQuery());
        }
    }
}