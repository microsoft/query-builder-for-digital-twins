// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace QueryBuilder.UnitTests.QueryBuilder.Dynamic
{
    using System;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Dynamic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class QueryJoinTest
    {
        [TestMethod]
        public void CanJoinWithFilter()
        {
            var query = DynamicQueryBuilder
                .FromTwins()
                .Join(j => j.With("floor").RelatedBy("hasChildren"))
                .Where(w => w.Property("$dtId").IsEqualTo("ID1"));

            Assert.AreEqual($"SELECT twin FROM DIGITALTWINS twin JOIN floor RELATED twin.hasChildren haschildrenrelationship WHERE twin.$dtId = 'ID1'", query.BuildAdtQuery());
        }

        [TestMethod]
        public void CanFilterInJoin()
        {
            var query = DynamicQueryBuilder
                .FromTwins()
                .Join(j => j
                        .With("floor")
                        .RelatedBy("hasChildren")
                        .Where(w => w
                            .Property("name")
                            .IsEqualTo("floor 1")));

            Assert.AreEqual($"SELECT twin FROM DIGITALTWINS twin JOIN floor RELATED twin.hasChildren haschildrenrelationship WHERE floor.name = 'floor 1'", query.BuildAdtQuery());
        }

        [TestMethod]
        public void AliasNotRequiredIfThereIsNoAmbiguity()
        {
            var query = DynamicQueryBuilder
                .FromTwins("bldng")
                .Join(f => f.With("flr").RelatedBy("hasChildren").On("bldng"))
                .Where(b => b.Property("$dtId").IsEqualTo("ID"));

            Assert.AreEqual($"SELECT bldng FROM DIGITALTWINS bldng JOIN flr RELATED bldng.hasChildren haschildrenrelationship WHERE bldng.$dtId = 'ID'", query.BuildAdtQuery());
        }

        [TestMethod]
        public void CanAssignAliasToRelationship()
        {
            var query = DynamicQueryBuilder
                .FromTwins("bldng")
                .Join(b => b.With("flr").RelatedBy("hasChildren").On("bldng").WithAlias("rel"))
                .Where(b => b.Property("$dtId").IsEqualTo("ID"));

            Assert.AreEqual($"SELECT bldng FROM DIGITALTWINS bldng JOIN flr RELATED bldng.hasChildren rel WHERE bldng.$dtId = 'ID'", query.BuildAdtQuery());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AliasHasToBeAssignedBeforeUsed()
        {
            var query = DynamicQueryBuilder
                .FromTwins()
                .Join(f => f.With("flr").RelatedBy("hasChildren").On("bldng"))
                .Where(b => b.Property("$dtId").IsEqualTo("ID"));

            query.BuildAdtQuery();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CanNotAssignAliasAlreadyUsed()
        {
            var query = DynamicQueryBuilder
                .FromTwins("bldng")
                .Join(f => f.With("floor").RelatedBy("hasChildren").On("bldng"))
                .Join(b => b.With("bldng").RelatedBy("hasChildren").On("floor"))
                .Where(b => b.Property("$dtId").IsEqualTo("ID"));

            query.BuildAdtQuery();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CanNotAssignRelationshipAliasAlreadyUsed()
        {
            var query = DynamicQueryBuilder
                .FromTwins()
                .Join(f => f.With("floor").RelatedBy("hasChildren").WithAlias("rel"))
                .Join(b => b.With("confroom").RelatedBy("hasChildren").On("floor").WithAlias("rel"))
                .Where(b => b.Property("$dtId").IsEqualTo("ID"));

            query.BuildAdtQuery();
        }
    }
}