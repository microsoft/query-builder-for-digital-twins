// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace QueryBuilder.UnitTests.QueryBuilder.Dynamic
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
                .FromTwins()
                .Count();

            Assert.AreEqual($"SELECT COUNT() FROM DIGITALTWINS twin", query.BuildAdtQuery());
        }

        [TestMethod]
        public void CountIsAddedWithDefaultSelectForRelationships()
        {
            var query = QueryBuilder
                .FromRelationships()
                .Count();

            Assert.AreEqual($"SELECT COUNT() FROM RELATIONSHIPS relationship", query.BuildAdtQuery());
        }

        [TestMethod]
        public void CountCanAllowWhereConditions()
        {
            var query = QueryBuilder
                .FromTwins()
                .Count()
                .Where(b => b.TwinProperty("$dtId").IsEqualTo("ID"));

            Assert.AreEqual($"SELECT COUNT() FROM DIGITALTWINS twin WHERE twin.$dtId = 'ID'", query.BuildAdtQuery());
        }

        [TestMethod]
        public void CountCanAllowWhereConditionsForRelationships()
        {
            var query = QueryBuilder
                .FromRelationships()
                .Count()
                .Where(b => b.Property("$targetId").IsEqualTo("someguid"));

            Assert.AreEqual($"SELECT COUNT() FROM RELATIONSHIPS relationship WHERE relationship.$targetId = 'someguid'", query.BuildAdtQuery());
        }

        [TestMethod]
        public void CanCountAll()
        {
            Assert.AreEqual($"SELECT COUNT() FROM DIGITALTWINS", QueryBuilder.CountAllDigitalTwins().BuildAdtQuery());
            Assert.AreEqual($"SELECT COUNT() FROM DIGITALTWINS", new CountAllQuery().BuildAdtQuery());
        }
    }
}