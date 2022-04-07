// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace QueryBuilder.UnitTests
{
    using Azure.DigitalTwins.Core;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class QueryFromTest
    {
        [TestMethod]
        public void FromGeneratesDefaultSelect()
        {
            var query = QueryBuilder
                .From<Building>();

            Assert.AreEqual($"SELECT building FROM DIGITALTWINS building WHERE IS_OF_MODEL(building, '{Building.ModelId.UpdateVersion(1)}')", query.BuildAdtQuery());
        }

        [TestMethod]
        public void CanAssignAlias()
        {
            var query = QueryBuilder
                .From<Building>("bldng");

            Assert.AreEqual($"SELECT bldng FROM DIGITALTWINS bldng WHERE IS_OF_MODEL(bldng, '{Building.ModelId.UpdateVersion(1)}')", query.BuildAdtQuery());
        }

        [TestMethod]
        public void FromGeneratesSelectAllBasicDigitalTwins()
        {
            var query = QueryBuilder
                .From<BasicDigitalTwin>();

            Assert.AreEqual("SELECT basicdigitaltwin FROM DIGITALTWINS basicdigitaltwin", query.BuildAdtQuery());

            query = QueryBuilder
                .From<BasicDigitalTwin>("twin");

            Assert.AreEqual("SELECT twin FROM DIGITALTWINS twin", query.BuildAdtQuery());
        }
    }
}