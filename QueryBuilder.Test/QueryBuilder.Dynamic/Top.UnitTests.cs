// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace QueryBuilder.UnitTests.QueryBuilder.Dynamic
{
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Dynamic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class QueryTopTest
    {
        [TestMethod]
        public void TopIsAddedWithDefaultSelect()
        {
            var query = DynamicQueryBuilder
                .FromTwins()
                .Top(1);

            Assert.AreEqual($"SELECT TOP(1) twin FROM DIGITALTWINS twin", query.BuildAdtQuery());
        }

        [TestMethod]
        public void TopIsAddedWithDefaultSelectForRelationships()
        {
            var query = DynamicQueryBuilder
                .FromRelationships()
                .Top(1);

            Assert.AreEqual($"SELECT TOP(1) relationship FROM RELATIONSHIPS relationship", query.BuildAdtQuery());
        }

        [TestMethod]
        public void TopIsAddedWithOneSelect()
        {
            var query = DynamicQueryBuilder
                .FromTwins()
                .Select("twin")
                .Top(1);

            Assert.AreEqual($"SELECT TOP(1) twin FROM DIGITALTWINS twin", query.BuildAdtQuery());
        }

        [TestMethod]
        public void LastTopTakesPrecedence()
        {
            var query = DynamicQueryBuilder
                .FromTwins()
                .Select("twin")
                .Top(1)
                .Top(2);

            Assert.AreEqual($"SELECT TOP(2) twin FROM DIGITALTWINS twin", query.BuildAdtQuery());
        }

        [TestMethod]
        public void TopIsAddedWithTwoSelects()
        {
            var query = DynamicQueryBuilder
                .FromTwins()
                .Join(j => j.With("floor").RelatedBy("hasChildren"))
                .Select("twin", "floor")
                .Where(b => b.Property("$dtId").IsEqualTo("ID"))
                .Top(1);

            Assert.AreEqual($"SELECT TOP(1) twin, floor FROM DIGITALTWINS twin JOIN floor RELATED twin.hasChildren haschildrenrelationship WHERE twin.$dtId = 'ID'", query.BuildAdtQuery());
        }

        [TestMethod]
        public void CanSelectAfterTop()
        {
            var query = DynamicQueryBuilder
                .FromTwins()
                .Join(j => j.With("floor").RelatedBy("hasChildren"))
                .Where(b => b.Property("$dtId").IsEqualTo("ID"))
                .Top(1)
                .Select("twin", "floor");

            Assert.AreEqual($"SELECT TOP(1) twin, floor FROM DIGITALTWINS twin JOIN floor RELATED twin.hasChildren haschildrenrelationship WHERE twin.$dtId = 'ID'", query.BuildAdtQuery());
        }

        [TestMethod]
        public void CanSelectAfterTopForRelationships()
        {
            var query = DynamicQueryBuilder
                .FromRelationships()
                .Where(b => b.Property("$relationshipName").IsEqualTo("hasChildren"))
                .Top(1)
                .Select("$targetId", "$relationshipName");

            Assert.AreEqual($"SELECT TOP(1) relationship.$targetId, relationship.$relationshipName FROM RELATIONSHIPS relationship WHERE relationship.$relationshipName = 'hasChildren'", query.BuildAdtQuery());
        }

        [TestMethod]
        public void CanUseTopAfterSelectForRelationships()
        {
            var query = DynamicQueryBuilder
                .FromRelationships()
                .Where(b => b.Property("$targetId").IsEqualTo("ID"))
                .Select("$targetId", "$relationshipName")
                .Top(1);

            Assert.AreEqual($"SELECT TOP(1) relationship.$targetId, relationship.$relationshipName FROM RELATIONSHIPS relationship WHERE relationship.$targetId = 'ID'", query.BuildAdtQuery());
        }
    }
}