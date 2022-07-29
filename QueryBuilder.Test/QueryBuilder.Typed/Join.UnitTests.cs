// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace QueryBuilder.UnitTests.QueryBuilder.Typed
{
    using System;
    using global::Azure.DigitalTwins.Core;
    using global::QueryBuilder.Test.Generated;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class QueryJoinTest
    {
        [TestMethod]
        public void CanJoinWithFilter()
        {
            var query = QueryBuilder
                .From<Building>()
                .Join<Building, Floor>(b => b.HasChildren)
                .WhereIn<Building>(b => b.Id, new string[] { "ID1", "ID2" });

            Assert.AreEqual($"SELECT building FROM DIGITALTWINS building JOIN floor RELATED building.hasChildren spacehaschildrenrelationship WHERE IS_OF_MODEL(building, '{Building.ModelId.UpdateVersion(1)}') AND IS_OF_MODEL(floor, '{Floor.ModelId.UpdateVersion(1)}') AND building.$dtId IN ['ID1','ID2']", query.BuildAdtQuery());
        }

        [TestMethod]
        public void CanJoinWithoutAlias()
        {
            var query = QueryBuilder
                .From<Building>()
                .Join<Building, Floor>(b => b.HasChildren)
                .Where<Building>(b => b.Id, ComparisonOperators.IsEqualTo, "ID");

            Assert.AreEqual($"SELECT building FROM DIGITALTWINS building JOIN floor RELATED building.hasChildren spacehaschildrenrelationship WHERE IS_OF_MODEL(building, '{Building.ModelId.UpdateVersion(1)}') AND IS_OF_MODEL(floor, '{Floor.ModelId.UpdateVersion(1)}') AND building.$dtId = 'ID'", query.BuildAdtQuery());
        }

        [TestMethod]
        public void CanJoinWithAlias()
        {
            var query = QueryBuilder
                .From<Building>("bldng")
                .Join<Building, Floor>(b => b.HasChildren, "bldng", "flr")
                .Where<Building>(b => b.Id, ComparisonOperators.IsEqualTo, "ID");

            Assert.AreEqual($"SELECT bldng FROM DIGITALTWINS bldng JOIN flr RELATED bldng.hasChildren spacehaschildrenrelationship WHERE IS_OF_MODEL(bldng, '{Building.ModelId.UpdateVersion(1)}') AND IS_OF_MODEL(flr, '{Floor.ModelId.UpdateVersion(1)}') AND bldng.$dtId = 'ID'", query.BuildAdtQuery());
        }

        [TestMethod]
        public void CanChooseWhatToJoin()
        {
            var query = QueryBuilder
                .From<Building>()
                .Join<Building, Floor>(b => b.HasChildren)
                .Where<Building>(b => b.Id, ComparisonOperators.IsEqualTo, "ID");

            Assert.AreEqual($"SELECT building FROM DIGITALTWINS building JOIN floor RELATED building.hasChildren spacehaschildrenrelationship WHERE IS_OF_MODEL(building, '{Building.ModelId.UpdateVersion(1)}') AND IS_OF_MODEL(floor, '{Floor.ModelId.UpdateVersion(1)}') AND building.$dtId = 'ID'", query.BuildAdtQuery());
        }

        [TestMethod]
        public void CanAssignAliasToBothJoinedTypes()
        {
            var query = QueryBuilder
                .From<Building>("bldng")
                .Join<Building, Floor>(b => b.HasChildren, "bldng", "flr")
                .Where<Building>(b => b.Id, ComparisonOperators.IsEqualTo, "ID", "bldng");

            Assert.AreEqual($"SELECT bldng FROM DIGITALTWINS bldng JOIN flr RELATED bldng.hasChildren spacehaschildrenrelationship WHERE IS_OF_MODEL(bldng, '{Building.ModelId.UpdateVersion(1)}') AND IS_OF_MODEL(flr, '{Floor.ModelId.UpdateVersion(1)}') AND bldng.$dtId = 'ID'", query.BuildAdtQuery());
        }

        [TestMethod]
        public void AliasNotRequiredIfThereIsNoAmbiguity()
        {
            var query = QueryBuilder
                .From<Building>("bldng")
                .Join<Building, Floor>(b => b.HasChildren, "bldng", "flr")
                .Where<Building>(b => b.Id, ComparisonOperators.IsEqualTo, "ID");

            Assert.AreEqual($"SELECT bldng FROM DIGITALTWINS bldng JOIN flr RELATED bldng.hasChildren spacehaschildrenrelationship WHERE IS_OF_MODEL(bldng, '{Building.ModelId.UpdateVersion(1)}') AND IS_OF_MODEL(flr, '{Floor.ModelId.UpdateVersion(1)}') AND bldng.$dtId = 'ID'", query.BuildAdtQuery());
        }

        [TestMethod]
        public void DoNotHaveToAssignAliasToAllModels()
        {
            var query = QueryBuilder
                .From<Building>("bldng")
                .Join<Building, Floor>(b => b.HasChildren, "bldng", "flr")
                .Join<Floor, ConferenceRoom>(f => f.HasChildren)
                .Where<Building>(b => b.Id, ComparisonOperators.IsEqualTo, "ID");

            Assert.AreEqual($"SELECT bldng FROM DIGITALTWINS bldng JOIN flr RELATED bldng.hasChildren spacehaschildrenrelationship JOIN conferenceroom RELATED flr.hasChildren spacehaschildrenrelationship1 WHERE IS_OF_MODEL(bldng, '{Building.ModelId.UpdateVersion(1)}') AND IS_OF_MODEL(flr, '{Floor.ModelId.UpdateVersion(1)}') AND IS_OF_MODEL(conferenceroom, '{ConferenceRoom.ModelId.UpdateVersion(1)}') AND bldng.$dtId = 'ID'", query.BuildAdtQuery());
        }

        [TestMethod]
        public void CanAssignAliasToRelationship()
        {
            var query = QueryBuilder
                .From<Building>("bldng")
                .Join<Building, Floor>(b => b.HasChildren, "bldng", "flr", "rel")
                .Join<Floor, ConferenceRoom>(f => f.HasChildren)
                .Where<Building>(b => b.Id, ComparisonOperators.IsEqualTo, "ID");

            Assert.AreEqual($"SELECT bldng FROM DIGITALTWINS bldng JOIN flr RELATED bldng.hasChildren rel JOIN conferenceroom RELATED flr.hasChildren spacehaschildrenrelationship WHERE IS_OF_MODEL(bldng, '{Building.ModelId.UpdateVersion(1)}') AND IS_OF_MODEL(flr, '{Floor.ModelId.UpdateVersion(1)}') AND IS_OF_MODEL(conferenceroom, '{ConferenceRoom.ModelId.UpdateVersion(1)}') AND bldng.$dtId = 'ID'", query.BuildAdtQuery());
        }

        [TestMethod]
        public void TwinTypeDoesNotGenerateTypeFilter()
        {
            var query = QueryBuilder
                .From<Building>("bldng")
                .Join<Building, Floor>(b => b.HasChildren, "bldng", "flr", "rel")
                .Join<Floor, BasicDigitalTwin>(f => f.HasChildren)
                .Where<Building>(b => b.Id, ComparisonOperators.IsEqualTo, "ID");

            Assert.AreEqual($"SELECT bldng FROM DIGITALTWINS bldng JOIN flr RELATED bldng.hasChildren rel JOIN basicdigitaltwin RELATED flr.hasChildren spacehaschildrenrelationship WHERE IS_OF_MODEL(bldng, '{Building.ModelId.UpdateVersion(1)}') AND IS_OF_MODEL(flr, '{Floor.ModelId.UpdateVersion(1)}') AND bldng.$dtId = 'ID'", query.BuildAdtQuery());

            query = QueryBuilder
                .From<Building>("bldng")
                .Join<Building, Floor>(b => b.HasChildren, "bldng", "flr", "rel")
                .Join<Floor, BasicDigitalTwin>(f => f.HasChildren, "flr", "mdl")
                .Where<Building>(b => b.Id, ComparisonOperators.IsEqualTo, "ID");

            Assert.AreEqual($"SELECT bldng FROM DIGITALTWINS bldng JOIN flr RELATED bldng.hasChildren rel JOIN mdl RELATED flr.hasChildren spacehaschildrenrelationship WHERE IS_OF_MODEL(bldng, '{Building.ModelId.UpdateVersion(1)}') AND IS_OF_MODEL(flr, '{Floor.ModelId.UpdateVersion(1)}') AND bldng.$dtId = 'ID'", query.BuildAdtQuery());
        }

        [TestMethod]
        public void JoinWithoutDtIdFilter()
        {
            var query = QueryBuilder
                    .From<Building>()
                    .Join<Building, Floor>(b => b.HasChildren);
            Assert.AreEqual($"SELECT building FROM DIGITALTWINS building JOIN floor RELATED building.hasChildren spacehaschildrenrelationship WHERE IS_OF_MODEL(building, '{Building.ModelId.UpdateVersion(1)}') AND IS_OF_MODEL(floor, '{Floor.ModelId.UpdateVersion(1)}')", query.BuildAdtQuery());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AliasHasToBeAssignedBeforeUsed()
        {
            var query = QueryBuilder
                .From<Building>()
                .Join<Building, Floor>(b => b.HasChildren, "bldng", "flr")
                .Where<Building>(b => b.Id, ComparisonOperators.IsEqualTo, "ID");

            query.BuildAdtQuery();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AmbiguousQueryRequiresAliasesDuringFiltering()
        {
            var query = QueryBuilder
                .From<Space>()
                .Join<Space, Space>(s => s.HasChildren)
                .Where<Space>(s => s.Id, ComparisonOperators.IsEqualTo, "ID");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CanNotUseTheSameAliasWithTwoModels()
        {
            var query = QueryBuilder
                .From<Building>("spc")
                .Join<Building, Floor>(b => b.HasChildren, "spc", "spc1")
                .Join<Building, Floor>(b => b.HasChildren, "spc1", "spc")
                .Where<Building>(b => b.Id, ComparisonOperators.IsEqualTo, "ID");

            query.BuildAdtQuery();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CanNotAssignAliasAlreadyUsed()
        {
            var query = QueryBuilder
                .From<Building>()
                .Join<Building, Floor>(b => b.HasChildren)
                .Join<Floor, ConferenceRoom>(b => b.HasChildren, "floor", "building")
                .Where<Building>(b => b.Id, ComparisonOperators.IsEqualTo, "ID");

            query.BuildAdtQuery();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CanNotAssignRelationshipAliasAlreadyUsed()
        {
            var query = QueryBuilder
                .From<Building>()
                .Join<Building, Floor>(b => b.HasChildren, "building", "floor", "rel")
                .Join<Floor, ConferenceRoom>(b => b.HasChildren, "floor", "conferenceroom", "rel")
                .Where<Building>(b => b.Id, ComparisonOperators.IsEqualTo, "ID");

            query.BuildAdtQuery();
        }
    }
}