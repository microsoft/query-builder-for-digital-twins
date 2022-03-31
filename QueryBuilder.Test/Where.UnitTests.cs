// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace QueryBuilder.UnitTests
{
    using System;
    using global::Azure.DigitalTwins.Core;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class QueryWhereTest
    {
        [TestMethod]
        public void CannotCompareToComplexTypes()
        {
            var query = QueryBuilder
                .From<Building>()
                .Where<Building>(b => b.Name, ComparisonOperators.IsEqualTo, new Building());

            Assert.ThrowsException<InvalidOperationException>(() => query.BuildAdtQuery());
        }

        [TestMethod]
        public void FromGeneratesFilterOnModel()
        {
            var query = QueryBuilder
                .From<Building>();

            Assert.AreEqual($"SELECT building FROM DIGITALTWINS building WHERE IS_OF_MODEL(building, '{Building.ModelId.UpdateVersion(1)}')", query.BuildAdtQuery());
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

        [TestMethod]
        public void CanApplyScalarUnaryFilters()
        {
            TestScalarUnaryOperator(ScalarOperators.IS_BOOL);
            TestScalarUnaryOperator(ScalarOperators.IS_DEFINED);
            TestScalarUnaryOperator(ScalarOperators.IS_NULL);
            TestScalarUnaryOperator(ScalarOperators.IS_NUMBER);
            TestScalarUnaryOperator(ScalarOperators.IS_OBJECT);
            TestScalarUnaryOperator(ScalarOperators.IS_PRIMITIVE);
            TestScalarUnaryOperator(ScalarOperators.IS_STRING);
        }

        [TestMethod]
        public void CanApplyScalarBinaryFilters()
        {
            var query = QueryBuilder
                   .From<Building>()
                   .WhereStartsWith<Building>(b => b.Name, "word");

            Assert.AreEqual($"SELECT building FROM DIGITALTWINS building WHERE IS_OF_MODEL(building, '{Building.ModelId.UpdateVersion(1)}') AND STARTSWITH(building.name, 'word')", query.BuildAdtQuery());

            query = QueryBuilder
                   .From<Building>()
                   .WhereStartsWith<Building>("dict.temp", "word");

            Assert.AreEqual($"SELECT building FROM DIGITALTWINS building WHERE IS_OF_MODEL(building, '{Building.ModelId.UpdateVersion(1)}') AND STARTSWITH(building.dict.temp, 'word')", query.BuildAdtQuery());

            query = QueryBuilder
                .From<Building>("bldng")
                .Join<Building, ITSiteFunction>(b => b.HasITSiteFunction, "bldng", "itfunc", "rel")
                .WhereStartsWith<BuildingHasITSiteFunctionRelationship>("maxPriority", "word")
                .Where<Building>(b => b.Id, ComparisonOperators.IsEqualTo, "ID");

            Assert.AreEqual($"SELECT bldng FROM DIGITALTWINS bldng JOIN itfunc RELATED bldng.hasITSiteFunction rel WHERE IS_OF_MODEL(bldng, '{Building.ModelId.UpdateVersion(1)}') AND IS_OF_MODEL(itfunc, '{ITSiteFunction.ModelId.UpdateVersion(1)}') AND STARTSWITH(rel.maxPriority, 'word') AND bldng.$dtId = 'ID'", query.BuildAdtQuery());

            query = QueryBuilder
                   .From<Building>()
                   .WhereEndsWith<Building>(b => b.Name, "word");

            Assert.AreEqual($"SELECT building FROM DIGITALTWINS building WHERE IS_OF_MODEL(building, '{Building.ModelId.UpdateVersion(1)}') AND ENDSWITH(building.name, 'word')", query.BuildAdtQuery());

            query = QueryBuilder
                   .From<Building>()
                   .WhereEndsWith<Building>("dict.temp", "word");

            Assert.AreEqual($"SELECT building FROM DIGITALTWINS building WHERE IS_OF_MODEL(building, '{Building.ModelId.UpdateVersion(1)}') AND ENDSWITH(building.dict.temp, 'word')", query.BuildAdtQuery());

            query = QueryBuilder
                .From<Building>("bldng")
                .Join<Building, ITSiteFunction>(b => b.HasITSiteFunction, "bldng", "itfunc", "rel")
                .WhereEndsWith<BuildingHasITSiteFunctionRelationship>("maxPriority", "word")
                .Where<Building>(b => b.Id, ComparisonOperators.IsEqualTo, "ID");

            Assert.AreEqual($"SELECT bldng FROM DIGITALTWINS bldng JOIN itfunc RELATED bldng.hasITSiteFunction rel WHERE IS_OF_MODEL(bldng, '{Building.ModelId.UpdateVersion(1)}') AND IS_OF_MODEL(itfunc, '{ITSiteFunction.ModelId.UpdateVersion(1)}') AND ENDSWITH(rel.maxPriority, 'word') AND bldng.$dtId = 'ID'", query.BuildAdtQuery());

            query = QueryBuilder
                   .From<Building>()
                   .WhereContains<Building>(b => b.Name, "word");

            Assert.AreEqual($"SELECT building FROM DIGITALTWINS building WHERE IS_OF_MODEL(building, '{Building.ModelId.UpdateVersion(1)}') AND CONTAINS(building.name, 'word')", query.BuildAdtQuery());

            query = QueryBuilder
                   .From<Building>()
                   .WhereContains<Building>("dict.temp", "word");

            Assert.AreEqual($"SELECT building FROM DIGITALTWINS building WHERE IS_OF_MODEL(building, '{Building.ModelId.UpdateVersion(1)}') AND CONTAINS(building.dict.temp, 'word')", query.BuildAdtQuery());

            query = QueryBuilder
                .From<Building>("bldng")
                .Join<Building, ITSiteFunction>(b => b.HasITSiteFunction, "bldng", "itfunc", "rel")
                .WhereContains<BuildingHasITSiteFunctionRelationship>("maxPriority", "word")
                .Where<Building>(b => b.Id, ComparisonOperators.IsEqualTo, "ID");

            Assert.AreEqual($"SELECT bldng FROM DIGITALTWINS bldng JOIN itfunc RELATED bldng.hasITSiteFunction rel WHERE IS_OF_MODEL(bldng, '{Building.ModelId.UpdateVersion(1)}') AND IS_OF_MODEL(itfunc, '{ITSiteFunction.ModelId.UpdateVersion(1)}') AND CONTAINS(rel.maxPriority, 'word') AND bldng.$dtId = 'ID'", query.BuildAdtQuery());
        }

        [TestMethod]
        public void CanApplyInFilters()
        {
            var query = QueryBuilder
                   .From<Building>()
                   .WhereIn<Building>(b => b.Name, new string[] { "name1", "name2" });

            Assert.AreEqual($"SELECT building FROM DIGITALTWINS building WHERE IS_OF_MODEL(building, '{Building.ModelId.UpdateVersion(1)}') AND building.name IN ['name1','name2']", query.BuildAdtQuery());

            query = QueryBuilder
                   .From<Building>()
                   .WhereIn<Building>("dict.temp", new string[] { "name1", "name2" });

            Assert.AreEqual($"SELECT building FROM DIGITALTWINS building WHERE IS_OF_MODEL(building, '{Building.ModelId.UpdateVersion(1)}') AND building.dict.temp IN ['name1','name2']", query.BuildAdtQuery());

            query = QueryBuilder
                .From<Building>("bldng")
                .Join<Building, ITSiteFunction>(b => b.HasITSiteFunction, "bldng", "itfunc", "rel")
                .WhereIn<BuildingHasITSiteFunctionRelationship>("maxPriority", new string[] { "name1", "name2" })
                .Where<Building>(b => b.Id, ComparisonOperators.IsEqualTo, "ID");

            Assert.AreEqual($"SELECT bldng FROM DIGITALTWINS bldng JOIN itfunc RELATED bldng.hasITSiteFunction rel WHERE IS_OF_MODEL(bldng, '{Building.ModelId.UpdateVersion(1)}') AND IS_OF_MODEL(itfunc, '{ITSiteFunction.ModelId.UpdateVersion(1)}') AND rel.maxPriority IN ['name1','name2'] AND bldng.$dtId = 'ID'", query.BuildAdtQuery());
        }

        [TestMethod]
        public void CanApplyNotInFilters()
        {
            var query = QueryBuilder
                   .From<Building>()
                   .WhereNotIn<Building>(b => b.Name, new string[] { "name1", "name2" });

            Assert.AreEqual($"SELECT building FROM DIGITALTWINS building WHERE IS_OF_MODEL(building, '{Building.ModelId.UpdateVersion(1)}') AND building.name NIN ['name1','name2']", query.BuildAdtQuery());

            query = QueryBuilder
                   .From<Building>()
                   .WhereNotIn<Building>("dict.temp", new string[] { "name1", "name2" });

            Assert.AreEqual($"SELECT building FROM DIGITALTWINS building WHERE IS_OF_MODEL(building, '{Building.ModelId.UpdateVersion(1)}') AND building.dict.temp NIN ['name1','name2']", query.BuildAdtQuery());

            query = QueryBuilder
                .From<Building>("bldng")
                .Join<Building, ITSiteFunction>(b => b.HasITSiteFunction, "bldng", "itfunc", "rel")
                .WhereNotIn<BuildingHasITSiteFunctionRelationship>("maxPriority", new string[] { "name1", "name2" })
                .Where<Building>(b => b.Id, ComparisonOperators.IsEqualTo, "ID");

            Assert.AreEqual($"SELECT bldng FROM DIGITALTWINS bldng JOIN itfunc RELATED bldng.hasITSiteFunction rel WHERE IS_OF_MODEL(bldng, '{Building.ModelId.UpdateVersion(1)}') AND IS_OF_MODEL(itfunc, '{ITSiteFunction.ModelId.UpdateVersion(1)}') AND rel.maxPriority NIN ['name1','name2'] AND bldng.$dtId = 'ID'", query.BuildAdtQuery());
        }

        [TestMethod]
        public void CanApplyWhereIsOfModelFilter()
        {
            var query = QueryBuilder
                    .From<Space>()
                    .Or(query => query
                        .WhereIsOfModel<Space, Building>()
                        .WhereIsOfModel<Space, Floor>());

            Assert.AreEqual($"SELECT space FROM DIGITALTWINS space WHERE IS_OF_MODEL(space, '{Space.ModelId.UpdateVersion(1)}') AND (IS_OF_MODEL(space, '{Building.ModelId.UpdateVersion(1)}') OR IS_OF_MODEL(space, '{Floor.ModelId.UpdateVersion(1)}'))", query.BuildAdtQuery());

            query = QueryBuilder
                .From<Space>()
                .Join<Space, BasicDigitalTwin>(s => s.HasChildren)
                .Where<Space>(b => b.Id, ComparisonOperators.IsEqualTo, "ID")
                .WhereIsOfModel<BasicDigitalTwin, Floor>();

            Assert.AreEqual($"SELECT space FROM DIGITALTWINS space JOIN basicdigitaltwin RELATED space.hasChildren spacehaschildrenrelationship WHERE IS_OF_MODEL(space, '{Space.ModelId.UpdateVersion(1)}') AND space.$dtId = 'ID' AND IS_OF_MODEL(basicdigitaltwin, '{Floor.ModelId.UpdateVersion(1)}')", query.BuildAdtQuery());

            query = QueryBuilder
                .From<Space>("bldng")
                .Join<Space, BasicDigitalTwin>(s => s.HasChildren, "bldng", "twin")
                .Or(
                    s => s.WhereIsOfModel<BasicDigitalTwin, Hallway>("twin")
                        .WhereIsOfModel<BasicDigitalTwin, Floor>("twin"))
                .Where<BasicDigitalTwin>(b => b.Id, ComparisonOperators.IsEqualTo, "ID", "twin");

            Assert.AreEqual($"SELECT bldng FROM DIGITALTWINS bldng JOIN twin RELATED bldng.hasChildren spacehaschildrenrelationship WHERE IS_OF_MODEL(bldng, '{Space.ModelId.UpdateVersion(1)}') AND (IS_OF_MODEL(twin, '{Hallway.ModelId.UpdateVersion(1)}') OR IS_OF_MODEL(twin, '{Floor.ModelId.UpdateVersion(1)}')) AND twin.$dtId = 'ID'", query.BuildAdtQuery());

            query = QueryBuilder
                .From<Space>("bldng")
                .Join<Space, BasicDigitalTwin>(s => s.HasChildren, "bldng", "twin")
                .Or(
                    s => s.WhereIsOfModel<BasicDigitalTwin, Hallway>("twin")
                        .WhereIsOfModel<BasicDigitalTwin, Floor>("twin"))
                .Where<BasicDigitalTwin>(b => b.Id, ComparisonOperators.IsEqualTo, "ID", "twin")
                .Not(query => query.WhereIsOfModel<Space, ConferenceRoom>("bldng"));

            Assert.AreEqual($"SELECT bldng FROM DIGITALTWINS bldng JOIN twin RELATED bldng.hasChildren spacehaschildrenrelationship WHERE IS_OF_MODEL(bldng, '{Space.ModelId.UpdateVersion(1)}') AND (IS_OF_MODEL(twin, '{Hallway.ModelId.UpdateVersion(1)}') OR IS_OF_MODEL(twin, '{Floor.ModelId.UpdateVersion(1)}')) AND twin.$dtId = 'ID' AND NOT IS_OF_MODEL(bldng, '{ConferenceRoom.ModelId.UpdateVersion(1)}')", query.BuildAdtQuery());
        }

        [TestMethod]
        public void CanApplyWhereIsOfModelFilterWithAlias()
        {
            var query = QueryBuilder
                    .From<Space>("bldg")
                    .WhereStartsWith<Space>(s => s.Name, "word")
                    .Or(query => query
                        .WhereIsOfModel<Space, Building>("bldg")
                        .WhereIsOfModel<Space, Floor>("bldg"));

            Assert.AreEqual($"SELECT bldg FROM DIGITALTWINS bldg WHERE IS_OF_MODEL(bldg, '{Space.ModelId.UpdateVersion(1)}') AND STARTSWITH(bldg.name, 'word') AND (IS_OF_MODEL(bldg, '{Building.ModelId.UpdateVersion(1)}') OR IS_OF_MODEL(bldg, '{Floor.ModelId.UpdateVersion(1)}'))", query.BuildAdtQuery());
        }

        [TestMethod]
        public void CanUseLogicalOperations()
        {
            var query = QueryBuilder
                   .From<Building>()
                   .Not(query => query.WhereEndsWith<Building>(b => b.Name, "word"));

            Assert.AreEqual($"SELECT building FROM DIGITALTWINS building WHERE IS_OF_MODEL(building, '{Building.ModelId.UpdateVersion(1)}') AND NOT ENDSWITH(building.name, 'word')", query.BuildAdtQuery());

            query = QueryBuilder
                   .From<Building>()
                   .Not(query => query.Not(q => q.WhereEndsWith<Building>(b => b.Name, "word")));

            Assert.AreEqual($"SELECT building FROM DIGITALTWINS building WHERE IS_OF_MODEL(building, '{Building.ModelId.UpdateVersion(1)}') AND NOT NOT ENDSWITH(building.name, 'word')", query.BuildAdtQuery());

            query = QueryBuilder
                   .From<Building>("bldng")
                   .Or(query => query
                       .Where<Building>("count", ComparisonOperators.IsGreaterThan, 20, alias: "bldng")
                       .Where<Building>("count", ComparisonOperators.IsLessThan, 10, alias: "bldng"));

            Assert.AreEqual($"SELECT bldng FROM DIGITALTWINS bldng WHERE IS_OF_MODEL(bldng, '{Building.ModelId.UpdateVersion(1)}') AND (bldng.count > 20 OR bldng.count < 10)", query.BuildAdtQuery());

            query = QueryBuilder
                .From<Building>("bldng")
                .Join<Building, ITSiteFunction>(b => b.HasITSiteFunction, "bldng", "itfunc", "rel")
                .Where<Building>(b => b.Id, ComparisonOperators.IsEqualTo, "ID")
                .Or(query => query
                    .Where<Building>("count", ComparisonOperators.IsGreaterThan, 20, alias: "bldng")
                    .And(q => q
                        .Where<Building>("count", ComparisonOperators.IsLessThan, 10, alias: "bldng")
                        .WhereEndsWith<BuildingHasITSiteFunctionRelationship>("maxPriority", "word")));

            Assert.AreEqual($"SELECT bldng FROM DIGITALTWINS bldng JOIN itfunc RELATED bldng.hasITSiteFunction rel WHERE IS_OF_MODEL(bldng, '{Building.ModelId.UpdateVersion(1)}') AND IS_OF_MODEL(itfunc, '{ITSiteFunction.ModelId.UpdateVersion(1)}') AND bldng.$dtId = 'ID' AND (bldng.count > 20 OR (bldng.count < 10 AND ENDSWITH(rel.maxPriority, 'word')))", query.BuildAdtQuery());

            query = QueryBuilder
                .From<Building>("bldng")
                .Join<Building, ITSiteFunction>(b => b.HasITSiteFunction, "bldng", "itfunc", "rel")
                .Where<Building>(b => b.Id, ComparisonOperators.IsEqualTo, "ID")
                .Not(query => query.WhereIsOfModel<Building, Space>("itfunc"))
                .Or(query => query
                    .Where<Building>("count", ComparisonOperators.IsGreaterThan, 20, alias: "bldng")
                    .And(q => q
                        .Where<Building>("count", ComparisonOperators.IsLessThan, 10, alias: "bldng")
                        .WhereEndsWith<BuildingHasITSiteFunctionRelationship>("maxPriority", "word")));

            Assert.AreEqual($"SELECT bldng FROM DIGITALTWINS bldng JOIN itfunc RELATED bldng.hasITSiteFunction rel WHERE IS_OF_MODEL(bldng, '{Building.ModelId.UpdateVersion(1)}') AND IS_OF_MODEL(itfunc, '{ITSiteFunction.ModelId.UpdateVersion(1)}') AND bldng.$dtId = 'ID' AND NOT IS_OF_MODEL(itfunc, '{Space.ModelId.UpdateVersion(1)}') AND (bldng.count > 20 OR (bldng.count < 10 AND ENDSWITH(rel.maxPriority, 'word')))", query.BuildAdtQuery());

            query = QueryBuilder
                .From<Building>("bldng")
                .Join<Building, ITSiteFunction>(b => b.HasITSiteFunction, "bldng", "itfunc", "rel")
                .Where<Building>(b => b.Id, ComparisonOperators.IsEqualTo, "ID")
                .Or(query => query
                    .Where<Building>("count", ComparisonOperators.IsGreaterThan, 20, alias: "bldng")
                    .Not(q => q
                        .Where<Building>("count", ComparisonOperators.IsLessThan, 10, alias: "bldng")
                        .WhereEndsWith<BuildingHasITSiteFunctionRelationship>("maxPriority", "word")));

            Assert.AreEqual($"SELECT bldng FROM DIGITALTWINS bldng JOIN itfunc RELATED bldng.hasITSiteFunction rel WHERE IS_OF_MODEL(bldng, '{Building.ModelId.UpdateVersion(1)}') AND IS_OF_MODEL(itfunc, '{ITSiteFunction.ModelId.UpdateVersion(1)}') AND bldng.$dtId = 'ID' AND (bldng.count > 20 OR NOT (bldng.count < 10 AND ENDSWITH(rel.maxPriority, 'word')))", query.BuildAdtQuery());
        }

        [TestMethod]
        public void CanApplyComparisonConditions()
        {
            TestComparisonOperation(ComparisonOperators.IsEqualTo);
            TestComparisonOperation(ComparisonOperators.IsGreaterThan);
            TestComparisonOperation(ComparisonOperators.IsGreaterThanOrEqualTo);
            TestComparisonOperation(ComparisonOperators.IsLessThan);
            TestComparisonOperation(ComparisonOperators.IsLessThanOrEqualTo);
            TestComparisonOperation(ComparisonOperators.NotEqualTo);
        }

        [TestMethod]
        public void OperatorsHaveTheRightNames()
        {
            Assert.AreEqual("=", ComparisonOperators.IsEqualTo.Operator);
            Assert.AreEqual(">", ComparisonOperators.IsGreaterThan.Operator);
            Assert.AreEqual(">=", ComparisonOperators.IsGreaterThanOrEqualTo.Operator);
            Assert.AreEqual("<", ComparisonOperators.IsLessThan.Operator);
            Assert.AreEqual("<=", ComparisonOperators.IsLessThanOrEqualTo.Operator);
            Assert.AreEqual("!=", ComparisonOperators.NotEqualTo.Operator);
            Assert.AreEqual("IS_BOOL", ScalarOperators.IS_BOOL.Name);
            Assert.AreEqual("IS_DEFINED", ScalarOperators.IS_DEFINED.Name);
            Assert.AreEqual("IS_NULL", ScalarOperators.IS_NULL.Name);
            Assert.AreEqual("IS_NUMBER", ScalarOperators.IS_NUMBER.Name);
            Assert.AreEqual("IS_OBJECT", ScalarOperators.IS_OBJECT.Name);
            Assert.AreEqual("IS_PRIMITIVE", ScalarOperators.IS_PRIMITIVE.Name);
            Assert.AreEqual("IS_STRING", ScalarOperators.IS_STRING.Name);
            Assert.AreEqual("STARTSWITH", ScalarOperators.STARTSWITH.Name);
            Assert.AreEqual("ENDSWITH", ScalarOperators.ENDSWITH.Name);
            Assert.AreEqual("CONTAINS", ScalarOperators.CONTAINS.Name);
        }

        [TestMethod]
        public void UnassignedAliasThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() =>
            {
                QueryBuilder
                       .From<Building>()
                       .WhereNotIn<Building>(b => b.Name, new string[] { "name1", "name2" }, "bldng")
                       .BuildAdtQuery();
            });

            Assert.ThrowsException<ArgumentException>(() =>
            {
                QueryBuilder
                       .From<Building>()
                       .WhereNotIn<Building>("dict.temp", new string[] { "name1", "name2" }, "bldng")
                       .BuildAdtQuery();
            });

            Assert.ThrowsException<ArgumentException>(() =>
            {
                QueryBuilder
                       .From<Building>()
                       .WhereIn<Building>(b => b.Name, new string[] { "name1", "name2" }, "bldng")
                       .BuildAdtQuery();
            });

            Assert.ThrowsException<ArgumentException>(() =>
            {
                QueryBuilder
                       .From<Building>()
                       .WhereIn<Building>("dict.temp", new string[] { "name1", "name2" }, "bldng")
                       .BuildAdtQuery();
            });

            Assert.ThrowsException<ArgumentException>(() =>
            {
                QueryBuilder
                       .From<Building>()
                       .WhereStartsWith<Building>("dict.temp", "word", "bldng")
                       .BuildAdtQuery();
            });

            Assert.ThrowsException<ArgumentException>(() =>
            {
                QueryBuilder
                       .From<Building>()
                       .WhereStartsWith<Building>(b => b.Name, "word", "bldng")
                       .BuildAdtQuery();
            });

            Assert.ThrowsException<ArgumentException>(() =>
            {
                QueryBuilder
                       .From<Building>()
                       .WhereEndsWith<Building>("dict.temp", "word", "bldng")
                       .BuildAdtQuery();
            });

            Assert.ThrowsException<ArgumentException>(() =>
            {
                QueryBuilder
                       .From<Building>()
                       .WhereEndsWith<Building>(b => b.Name, "word", "bldng")
                       .BuildAdtQuery();
            });

            Assert.ThrowsException<ArgumentException>(() =>
            {
                QueryBuilder
                       .From<Building>()
                       .WhereContains<Building>(b => b.Name, "word", "bldng")
                       .BuildAdtQuery();
            });

            Assert.ThrowsException<ArgumentException>(() =>
            {
                QueryBuilder
                    .From<Building>()
                    .WhereIsOfModel<Building, Space>("bldng")
                    .BuildAdtQuery();
            });
        }

        [TestMethod]
        public void CanPutConditionsOnRelationshipProperties()
        {
            var query = QueryBuilder
                      .From<Building>()
                      .Join<Building, ITSiteFunction>(b => b.HasITSiteFunction)
                      .Where<Building>(b => b.HasITSiteFunction.MaxPriority, ComparisonOperators.IsEqualTo, 50)
                      .Where<Building>(b => b.Id, ComparisonOperators.IsEqualTo, "ID");

            Assert.AreEqual($"SELECT building FROM DIGITALTWINS building JOIN itsitefunction RELATED building.hasITSiteFunction buildinghasitsitefunctionrelationship WHERE IS_OF_MODEL(building, '{Building.ModelId.UpdateVersion(1)}') AND IS_OF_MODEL(itsitefunction, '{ITSiteFunction.ModelId.UpdateVersion(1)}') AND buildinghasitsitefunctionrelationship.maxPriority = 50 AND building.$dtId = 'ID'", query.BuildAdtQuery());

            query = QueryBuilder
                      .From<Building>()
                      .Join<Building, Employee>(b => b.HasBuildingContact)
                      .Where<Building>(b => b.HasBuildingContact.Comments, ComparisonOperators.IsEqualTo, "some comment")
                      .Where<Building>(b => b.Id, ComparisonOperators.IsEqualTo, "ID");

            Assert.AreEqual($"SELECT building FROM DIGITALTWINS building JOIN employee RELATED building.hasBuildingContact buildinghasbuildingcontactrelationship WHERE IS_OF_MODEL(building, '{Building.ModelId.UpdateVersion(1)}') AND IS_OF_MODEL(employee, '{Employee.ModelId.UpdateVersion(1)}') AND buildinghasbuildingcontactrelationship.comments = 'some comment' AND building.$dtId = 'ID'", query.BuildAdtQuery());

            query = QueryBuilder
                      .From<Building>("bldng")
                      .Join<Building, ITSiteFunction>(b => b.HasITSiteFunction, "bldng", "itfunc", "rel")
                      .Where<BuildingHasITSiteFunctionRelationship>("maxPriority", ComparisonOperators.IsEqualTo, 50)
                      .Where<Building>(b => b.Id, ComparisonOperators.IsEqualTo, "ID");

            Assert.AreEqual($"SELECT bldng FROM DIGITALTWINS bldng JOIN itfunc RELATED bldng.hasITSiteFunction rel WHERE IS_OF_MODEL(bldng, '{Building.ModelId.UpdateVersion(1)}') AND IS_OF_MODEL(itfunc, '{ITSiteFunction.ModelId.UpdateVersion(1)}') AND rel.maxPriority = 50 AND bldng.$dtId = 'ID'", query.BuildAdtQuery());

            query = QueryBuilder
                      .From<Building>("bldng")
                      .Join<Building, Employee>(b => b.HasBuildingContact, "bldng", "empl", "rel")
                      .Where<BuildingHasBuildingContactRelationship>("comments", ComparisonOperators.IsEqualTo, "some comment")
                      .Where<Building>(b => b.Id, ComparisonOperators.IsEqualTo, "ID");

            Assert.AreEqual($"SELECT bldng FROM DIGITALTWINS bldng JOIN empl RELATED bldng.hasBuildingContact rel WHERE IS_OF_MODEL(bldng, '{Building.ModelId.UpdateVersion(1)}') AND IS_OF_MODEL(empl, '{Employee.ModelId.UpdateVersion(1)}') AND rel.comments = 'some comment' AND bldng.$dtId = 'ID'", query.BuildAdtQuery());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SelectingMethodThrowsException()
        {
            var query = QueryBuilder
                         .From<Building>()
                         .Join<Building, ITSiteFunction>(b => b.HasITSiteFunction)
                         .Where<Building>(b => b.HasSalesGeo.ToString(), ComparisonOperators.IsEqualTo, 50)
                         .Where<Building>(b => b.Id, ComparisonOperators.IsEqualTo, "ID");
        }

        [TestMethod]
        [ExpectedException(typeof(NoJsonPropertyException))]
        public void SelectingNonJsonThrowsException()
        {
            var query = QueryBuilder
                         .From<Building>()
                         .Join<Building, ITSiteFunction>(b => b.HasITSiteFunction)
                         .Where<Building>(b => b.HasSalesGeo.Name.Length, ComparisonOperators.IsEqualTo, 50)
                         .Where<Building>(b => b.Id, ComparisonOperators.IsEqualTo, "ID");
        }

        [TestMethod]
        public void CanUseEscapableCharacters()
        {
            var someBuilding1 = "some'building";
            var someBuildingEscaped1 = @"some\'building";
            var someBuilding2 = "some'buil'ding";
            var someBuildingEscaped2 = @"some\'buil\'ding";

            var query = QueryBuilder
                      .From<Building>()
                      .Where<Building>(b => b.Name, ComparisonOperators.IsEqualTo, someBuilding1);

            Assert.AreEqual($"SELECT building FROM DIGITALTWINS building WHERE IS_OF_MODEL(building, '{Building.ModelId.UpdateVersion(1)}') AND building.name = '{someBuildingEscaped1}'", query.BuildAdtQuery());

            query = QueryBuilder
                  .From<Building>()
                  .WhereIn<Building>(b => b.Name, new string[] { someBuilding1, someBuilding2 });

            Assert.AreEqual($"SELECT building FROM DIGITALTWINS building WHERE IS_OF_MODEL(building, '{Building.ModelId.UpdateVersion(1)}') AND building.name IN ['{someBuildingEscaped1}','{someBuildingEscaped2}']", query.BuildAdtQuery());

            query = QueryBuilder
                  .From<Building>()
                  .WhereStartsWith<Building>(b => b.Name, someBuilding1);

            Assert.AreEqual($"SELECT building FROM DIGITALTWINS building WHERE IS_OF_MODEL(building, '{Building.ModelId.UpdateVersion(1)}') AND STARTSWITH(building.name, '{someBuildingEscaped1}')", query.BuildAdtQuery());
        }

        [TestMethod]
        public void FilteringOverEnumWithNoMemberAttributeUsesInt()
        {
            var query = QueryBuilder
                    .From<Building>()
                    .Where<Building>(x => x.Status, ComparisonOperators.IsEqualTo, CustomEnum.Value2);

            Assert.AreEqual($"SELECT building FROM DIGITALTWINS building WHERE IS_OF_MODEL(building, '{Building.ModelId.UpdateVersion(1)}') AND building.status = '1'", query.BuildAdtQuery());
        }

        private void TestScalarUnaryOperator(AdtScalarUnaryOperator op)
        {
            var query = QueryBuilder
                   .From<Building>()
                   .Where<Building>(b => b.Name, op);

            Assert.AreEqual($"SELECT building FROM DIGITALTWINS building WHERE IS_OF_MODEL(building, '{Building.ModelId.UpdateVersion(1)}') AND {op.Name}(building.name)", query.BuildAdtQuery());

            query = QueryBuilder
                .From<Building>()
                .Where<Building>("dict.temp", op);

            Assert.AreEqual($"SELECT building FROM DIGITALTWINS building WHERE IS_OF_MODEL(building, '{Building.ModelId.UpdateVersion(1)}') AND {op.Name}(building.dict.temp)", query.BuildAdtQuery());

            query = QueryBuilder
                .From<Building>("bldng")
                .Join<Building, ITSiteFunction>(b => b.HasITSiteFunction, "bldng", "itfunc", "rel")
                .Where<BuildingHasITSiteFunctionRelationship>("maxPriority", op)
                .Where<Building>(b => b.Id, ComparisonOperators.IsEqualTo, "ID");

            Assert.AreEqual($"SELECT bldng FROM DIGITALTWINS bldng JOIN itfunc RELATED bldng.hasITSiteFunction rel WHERE IS_OF_MODEL(bldng, '{Building.ModelId.UpdateVersion(1)}') AND IS_OF_MODEL(itfunc, '{ITSiteFunction.ModelId.UpdateVersion(1)}') AND {op.Name}(rel.maxPriority) AND bldng.$dtId = 'ID'", query.BuildAdtQuery());
        }

        private void TestComparisonOperation(ComparisonOperators op)
        {
            var query = QueryBuilder
                   .From<Building>()
                   .Where<Building>(b => b.Name, op, 50);

            Assert.AreEqual($"SELECT building FROM DIGITALTWINS building WHERE IS_OF_MODEL(building, '{Building.ModelId.UpdateVersion(1)}') AND building.name {op.Operator} 50", query.BuildAdtQuery());

            query = QueryBuilder
                .From<Building>()
                .Where<Building>(x => x.Status, op, SpaceStatus.Active);

            Assert.AreEqual($"SELECT building FROM DIGITALTWINS building WHERE IS_OF_MODEL(building, '{Building.ModelId.UpdateVersion(1)}') AND building.status {op.Operator} 'Active'", query.BuildAdtQuery());

            query = QueryBuilder
                .From<Building>()
                .Where<Building>("dict.temp", op, 50);

            Assert.AreEqual($"SELECT building FROM DIGITALTWINS building WHERE IS_OF_MODEL(building, '{Building.ModelId.UpdateVersion(1)}') AND building.dict.temp {op.Operator} 50", query.BuildAdtQuery());
        }

        private enum CustomEnum
        {
            Value1,
            Value2
        }
    }
}