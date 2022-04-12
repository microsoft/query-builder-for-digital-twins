// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace QueryBuilder.UnitTests.QueryBuilder.Dynamic
{
    using System;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class QueryJoinTest
    {
        [TestMethod]
        public void CanJoinWithFilter()
        {
            var query = QueryBuilder
                .FromTwins()
                .Join(j => j.With("floor").RelatedBy("hasChildren"))
                .Where(w => w.Property("$dtId").IsEqualTo("ID1"));

            Assert.AreEqual($"SELECT twin FROM DIGITALTWINS twin JOIN floor RELATED twin.hasChildren haschildrenrelationship WHERE twin.$dtId = 'ID1'", query.BuildAdtQuery());
        }

        [TestMethod]
        public void CanNestJoins()
        {
            var query = QueryBuilder
                .FromTwins()
                .Join(j => j
                    .With("floor")
                    .RelatedBy("hasChildren")
                    .Join(i => i
                        .With("device")
                        .RelatedBy("hasDevices")));

            Assert.AreEqual("SELECT twin FROM DIGITALTWINS twin JOIN floor RELATED twin.hasChildren haschildrenrelationship JOIN device RELATED floor.hasDevices hasdevicesrelationship", query.BuildAdtQuery());
        }

        [TestMethod]
        public void CanFilterInJoin()
        {
            var query = QueryBuilder
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
            var query = QueryBuilder
                .FromTwins("bldng")
                .Join(f => f.With("flr").RelatedBy("hasChildren"))
                .Where(b => b.Property("$dtId").IsEqualTo("ID"));

            Assert.AreEqual($"SELECT bldng FROM DIGITALTWINS bldng JOIN flr RELATED bldng.hasChildren haschildrenrelationship WHERE bldng.$dtId = 'ID'", query.BuildAdtQuery());
        }

        [TestMethod]
        public void CanAssignAliasToRelationship()
        {
            var query = QueryBuilder
                .FromTwins("bldng")
                .Join(b => b.With("flr").RelatedBy("hasChildren").As("rel"))
                .Where(b => b.Property("$dtId").IsEqualTo("ID"));

            Assert.AreEqual($"SELECT bldng FROM DIGITALTWINS bldng JOIN flr RELATED bldng.hasChildren rel WHERE bldng.$dtId = 'ID'", query.BuildAdtQuery());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CanNotAssignAliasAlreadyUsed()
        {
            var query = QueryBuilder
                .FromTwins("bldng")
                .Join(f => f.With("floor").RelatedBy("hasChildren"))
                .Join(b => b.With("bldng").RelatedBy("hasChildren"))
                .Where(b => b.Property("$dtId").IsEqualTo("ID"));

            query.BuildAdtQuery();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CanNotAssignRelationshipAliasAlreadyUsed()
        {
            var query = QueryBuilder
                .FromTwins()
                .Join(f => f.With("floor").RelatedBy("hasChildren").As("rel"))
                .Join(b => b.With("confroom").RelatedBy("hasChildren").As("rel"))
                .Where(b => b.Property("$dtId").IsEqualTo("ID"));

            query.BuildAdtQuery();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CanNotAssignNullAlias()
        {
            var query = QueryBuilder
                .FromTwins()
                .Join(j => j.With(null).RelatedBy("hasChildren"))
                .Where(w => w.Property("$dtId").IsEqualTo("ID"));

            query.BuildAdtQuery();
        }
    }
}