// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace QueryBuilder.UnitTests.QueryBuilder.Dynamic
{
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Dynamic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class QueryCountTest
    {
        [TestMethod]
        public void CountIsAddedWithDefaultSelect()
        {
            var query = DynamicQueryBuilder
                .FromTwins()
                .Count();

            Assert.AreEqual($"SELECT COUNT() FROM DIGITALTWINS twin", query.BuildAdtQuery());
        }

        [TestMethod]
        public void CountCanAllowWhereConditions()
        {
            var query = DynamicQueryBuilder
                .FromTwins()
                .Count()
                .Where(b => b.Property("$dtId").IsEqualTo("ID"));

            Assert.AreEqual($"SELECT COUNT() FROM DIGITALTWINS twin WHERE twin.$dtId = 'ID'", query.BuildAdtQuery());
        }

        [TestMethod]
        public void CanCountAll()
        {
            Assert.AreEqual($"SELECT COUNT() FROM DIGITALTWINS", DynamicQueryBuilder.CountAllDigitalTwins().BuildAdtQuery());
            Assert.AreEqual($"SELECT COUNT() FROM DIGITALTWINS", new CountAllQuery().BuildAdtQuery());
        }
    }
}