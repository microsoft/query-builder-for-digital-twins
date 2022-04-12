// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace QueryBuilder.UnitTests.QueryBuilder.Dynamic
{
    using System;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Dynamic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class QueryWhereTest
    {
        [TestMethod]
        public void CannotCompareToComplexTypes()
        {
            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                var query = DynamicQueryBuilder
                .FromTwins()
                .Where(b => b.Property("name").IsEqualTo(new Building()))
                .BuildAdtQuery();
            });
        }

        [TestMethod]
        public void CanApplyScalarUnaryFilters()
        {
            var query = DynamicQueryBuilder
                    .FromTwins()
                    .Where(t => t.Property("name").IsBool());

            Assert.AreEqual("SELECT twin FROM DIGITALTWINS twin WHERE IS_BOOL(twin.name)", query.BuildAdtQuery());

            query = DynamicQueryBuilder
                    .FromTwins()
                    .Where(t => t.Property("name").IsDefined());

            Assert.AreEqual("SELECT twin FROM DIGITALTWINS twin WHERE IS_DEFINED(twin.name)", query.BuildAdtQuery());

            query = DynamicQueryBuilder
                    .FromTwins()
                    .Where(t => t.Property("name").IsNull());

            Assert.AreEqual("SELECT twin FROM DIGITALTWINS twin WHERE IS_NULL(twin.name)", query.BuildAdtQuery());

            query = DynamicQueryBuilder
                    .FromTwins()
                    .Where(t => t.Property("name").IsNumber());

            Assert.AreEqual("SELECT twin FROM DIGITALTWINS twin WHERE IS_NUMBER(twin.name)", query.BuildAdtQuery());

            query = DynamicQueryBuilder
                    .FromTwins()
                    .Where(t => t.Property("name").IsObject());

            Assert.AreEqual("SELECT twin FROM DIGITALTWINS twin WHERE IS_OBJECT(twin.name)", query.BuildAdtQuery());

            query = DynamicQueryBuilder
                    .FromTwins()
                    .Where(t => t.Property("name").IsPrimitive());

            Assert.AreEqual("SELECT twin FROM DIGITALTWINS twin WHERE IS_PRIMITIVE(twin.name)", query.BuildAdtQuery());

            query = DynamicQueryBuilder
                    .FromTwins()
                    .Where(t => t.Property("name").IsString());

            Assert.AreEqual("SELECT twin FROM DIGITALTWINS twin WHERE IS_STRING(twin.name)", query.BuildAdtQuery());
        }

        [TestMethod]
        public void CanApplyScalarBinaryFilters()
        {
            var query = DynamicQueryBuilder
                   .FromTwins()
                   .Where(t => t.Property("name").StartsWith("word"));

            Assert.AreEqual($"SELECT twin FROM DIGITALTWINS twin WHERE STARTSWITH(twin.name, 'word')", query.BuildAdtQuery());

            query = DynamicQueryBuilder
                   .FromTwins()
                   .Where(t => t.Property("dict.temp").StartsWith("word"));

            Assert.AreEqual($"SELECT twin FROM DIGITALTWINS twin WHERE STARTSWITH(twin.dict.temp, 'word')", query.BuildAdtQuery());

            query = DynamicQueryBuilder
                .FromTwins("bldng")
                .Join(f => f
                    .With("itfunc")
                    .RelatedBy("hasITSiteFunction")
                    .WithAlias("rel")
                    .Where(r => r.RelationshipProperty("maxPriority").StartsWith("word")))
                .Where(b => b.Property("$dtId").IsEqualTo("ID"));

            Assert.AreEqual($"SELECT bldng FROM DIGITALTWINS bldng JOIN itfunc RELATED bldng.hasITSiteFunction rel WHERE STARTSWITH(rel.maxPriority, 'word') AND bldng.$dtId = 'ID'", query.BuildAdtQuery());

            query = DynamicQueryBuilder
                   .FromTwins()
                   .Where(t => t.Property("name").EndsWith("word"));

            Assert.AreEqual($"SELECT twin FROM DIGITALTWINS twin WHERE ENDSWITH(twin.name, 'word')", query.BuildAdtQuery());

            query = DynamicQueryBuilder
                   .FromTwins()
                   .Where(t => t.Property("dict.temp").EndsWith("word"));

            Assert.AreEqual($"SELECT twin FROM DIGITALTWINS twin WHERE ENDSWITH(twin.dict.temp, 'word')", query.BuildAdtQuery());

            query = DynamicQueryBuilder
                .FromTwins("bldng")
                .Join(f => f
                    .With("itfunc")
                    .RelatedBy("hasITSiteFunction")
                    .WithAlias("rel")
                    .Where(r => r.RelationshipProperty("maxPriority").EndsWith("word")))
                .Where(b => b.Property("$dtId").IsEqualTo("ID"));

            Assert.AreEqual($"SELECT bldng FROM DIGITALTWINS bldng JOIN itfunc RELATED bldng.hasITSiteFunction rel WHERE ENDSWITH(rel.maxPriority, 'word') AND bldng.$dtId = 'ID'", query.BuildAdtQuery());

            query = DynamicQueryBuilder
                   .FromTwins()
                   .Where(t => t.Property("name").Contains("word"));

            Assert.AreEqual($"SELECT twin FROM DIGITALTWINS twin WHERE CONTAINS(twin.name, 'word')", query.BuildAdtQuery());

            query = DynamicQueryBuilder
                   .FromTwins()
                   .Where(t => t.Property("dict.temp").Contains("word"));

            Assert.AreEqual($"SELECT twin FROM DIGITALTWINS twin WHERE CONTAINS(twin.dict.temp, 'word')", query.BuildAdtQuery());

            query = DynamicQueryBuilder
                .FromTwins("bldng")
                .Join(f => f
                    .With("itfunc")
                    .RelatedBy("hasITSiteFunction")
                    .WithAlias("rel")
                    .Where(r => r.RelationshipProperty("maxPriority").Contains("word")))
                .Where(b => b.Property("$dtId").IsEqualTo("ID"));

            Assert.AreEqual($"SELECT bldng FROM DIGITALTWINS bldng JOIN itfunc RELATED bldng.hasITSiteFunction rel WHERE CONTAINS(rel.maxPriority, 'word') AND bldng.$dtId = 'ID'", query.BuildAdtQuery());
        }

        [TestMethod]
        public void CanApplyInFilters()
        {
            var query = DynamicQueryBuilder
                   .FromTwins()
                   .Where(b => b.Property("name").IsIn(new string[] { "name1", "name2" }));

            Assert.AreEqual($"SELECT twin FROM DIGITALTWINS twin WHERE twin.name IN ['name1','name2']", query.BuildAdtQuery());

            query = DynamicQueryBuilder
                   .FromTwins()
                   .Where(b => b.Property("dict.temp").IsIn(new string[] { "name1", "name2" }));

            Assert.AreEqual($"SELECT twin FROM DIGITALTWINS twin WHERE twin.dict.temp IN ['name1','name2']", query.BuildAdtQuery());

            query = DynamicQueryBuilder
                .FromTwins("bldng")
                .Join(b => b
                    .With("itfunc")
                    .RelatedBy("hasITSiteFunction")
                    .WithAlias("rel")
                    .Where(r => r.RelationshipProperty("maxPriority").IsIn(new string[] { "name1", "name2" })))
                .Where(b => b.Property("$dtId").IsEqualTo("ID"));

            Assert.AreEqual($"SELECT bldng FROM DIGITALTWINS bldng JOIN itfunc RELATED bldng.hasITSiteFunction rel WHERE rel.maxPriority IN ['name1','name2'] AND bldng.$dtId = 'ID'", query.BuildAdtQuery());
        }

        [TestMethod]
        public void CanApplyNotInFilters()
        {
            var query = DynamicQueryBuilder
                   .FromTwins()
                   .Where(b => b.Property("name").IsNotIn(new string[] { "name1", "name2" }));

            Assert.AreEqual($"SELECT twin FROM DIGITALTWINS twin WHERE twin.name NIN ['name1','name2']", query.BuildAdtQuery());

            query = DynamicQueryBuilder
                   .FromTwins()
                   .Where(b => b.Property("dict.temp").IsNotIn(new string[] { "name1", "name2" }));

            Assert.AreEqual($"SELECT twin FROM DIGITALTWINS twin WHERE twin.dict.temp NIN ['name1','name2']", query.BuildAdtQuery());

            query = DynamicQueryBuilder
                .FromTwins("bldng")
                .Join(b => b
                    .With("itfunc")
                    .RelatedBy("hasITSiteFunction")
                    .WithAlias("rel")
                    .Where(r => r.RelationshipProperty("maxPriority").IsNotIn(new string[] { "name1", "name2" })))
                .Where(b => b.Property("$dtId").IsEqualTo("ID"));

            Assert.AreEqual($"SELECT bldng FROM DIGITALTWINS bldng JOIN itfunc RELATED bldng.hasITSiteFunction rel WHERE rel.maxPriority NIN ['name1','name2'] AND bldng.$dtId = 'ID'", query.BuildAdtQuery());
        }

        [TestMethod]
        public void CanAssignPrecedence()
        {
            var query = DynamicQueryBuilder
                    .FromTwins("bldng")
                    .Where(b => b
                        .Property("name")
                        .IsEqualTo("building 1")
                        .And()
                        .Precedence(p => p
                            .Property("count")
                            .IsGreaterThan(20)
                            .Or(b => b
                                .Property("count")
                                .IsLessThan(10))));

            Assert.AreEqual($"SELECT bldng FROM DIGITALTWINS bldng WHERE bldng.name = 'building 1' AND (bldng.count > 20 OR bldng.count < 10)", query.BuildAdtQuery());
        }

        [TestMethod]
        public void CanUseLogicalOperations()
        {
            var query = DynamicQueryBuilder
                   .FromTwins()
                   .Where(t => t.Not(b => b.Property("name").EndsWith("word")));

            Assert.AreEqual($"SELECT twin FROM DIGITALTWINS twin WHERE NOT ENDSWITH(twin.name, 'word')", query.BuildAdtQuery());

            query = DynamicQueryBuilder
                   .FromTwins()
                   .Where(t => t.Not(q => q.Not(b => b.Property("name").EndsWith("word"))));

            Assert.AreEqual($"SELECT twin FROM DIGITALTWINS twin WHERE NOT NOT ENDSWITH(twin.name, 'word')", query.BuildAdtQuery());

            query = DynamicQueryBuilder
                    .FromTwins("bldng")
                    .Where(b => b
                        .Property("count")
                        .IsGreaterThan(20)
                        .Or(b => b.Property("count").IsLessThan(10)));

            Assert.AreEqual($"SELECT bldng FROM DIGITALTWINS bldng WHERE bldng.count > 20 OR bldng.count < 10", query.BuildAdtQuery());

            query = DynamicQueryBuilder
                    .FromTwins("bldng")
                    .Where(b => b
                        .Property("count")
                        .IsGreaterThan(20)
                        .Or()
                        .Property("count").IsLessThan(10));

            Assert.AreEqual($"SELECT bldng FROM DIGITALTWINS bldng WHERE bldng.count > 20 OR bldng.count < 10", query.BuildAdtQuery());

            query = DynamicQueryBuilder
                .FromTwins("bldng")
                .Join(b => b
                    .With("itfunc")
                    .RelatedBy("hasITSiteFunction")
                    .WithAlias("rel"))
                .Where(b => b
                    .Property("$dtId")
                    .IsEqualTo("ID")
                    .And(a => a
                        .Precedence(p => p
                            .Property("count")
                            .IsGreaterThan(20)
                            .Or(o => o
                                .Precedence(p => p
                                    .Property("count")
                                    .IsLessThan(10)
                                    .And(a => a
                                        .Property("maxPriority", "rel")
                                        .EndsWith("word")))))));

            Assert.AreEqual($"SELECT bldng FROM DIGITALTWINS bldng JOIN itfunc RELATED bldng.hasITSiteFunction rel WHERE bldng.$dtId = 'ID' AND (bldng.count > 20 OR (bldng.count < 10 AND ENDSWITH(rel.maxPriority, 'word')))", query.BuildAdtQuery());

            query = DynamicQueryBuilder
                .FromTwins("bldng")
                .Join(b => b
                    .With("itfunc")
                    .RelatedBy("hasITSiteFunction")
                    .WithAlias("rel"))
                .Where(b => b
                    .Property("$dtId")
                    .IsEqualTo("ID")
                    .And(a => a
                        .Precedence(p => p
                            .Property("count")
                            .IsGreaterThan(20)
                            .Or(o => o
                                .Not(n => n
                                    .Precedence(p => p
                                        .Property("count")
                                        .IsLessThan(10)
                                        .And(a => a
                                            .Property("maxPriority", "rel")
                                            .EndsWith("word"))))))));

            Assert.AreEqual($"SELECT bldng FROM DIGITALTWINS bldng JOIN itfunc RELATED bldng.hasITSiteFunction rel WHERE bldng.$dtId = 'ID' AND (bldng.count > 20 OR NOT (bldng.count < 10 AND ENDSWITH(rel.maxPriority, 'word')))", query.BuildAdtQuery());
        }

        [TestMethod]
        public void CanApplyComparisonConditions()
        {
            var query = DynamicQueryBuilder
                .FromTwins()
                .Where(t => t.Property("name").IsEqualTo("building 1"));

            Assert.AreEqual($"SELECT twin FROM DIGITALTWINS twin WHERE twin.name = 'building 1'", query.BuildAdtQuery());

            query = DynamicQueryBuilder
                .FromTwins()
                .Where(t => t.Property("externalId").IsGreaterThan(1));

            Assert.AreEqual($"SELECT twin FROM DIGITALTWINS twin WHERE twin.externalId > 1", query.BuildAdtQuery());

            query = DynamicQueryBuilder
                .FromTwins()
                .Where(t => t.Property("externalId").IsGreaterThanOrEqualTo(1));

            Assert.AreEqual($"SELECT twin FROM DIGITALTWINS twin WHERE twin.externalId >= 1", query.BuildAdtQuery());

            query = DynamicQueryBuilder
                .FromTwins()
                .Where(t => t.Property("externalId").IsLessThan(1));

            Assert.AreEqual($"SELECT twin FROM DIGITALTWINS twin WHERE twin.externalId < 1", query.BuildAdtQuery());

            query = DynamicQueryBuilder
                .FromTwins()
                .Where(t => t.Property("externalId").IsLessThanOrEqualTo(1));

            Assert.AreEqual($"SELECT twin FROM DIGITALTWINS twin WHERE twin.externalId <= 1", query.BuildAdtQuery());

            query = DynamicQueryBuilder
                .FromTwins()
                .Where(t => t.Property("name").NotEqualTo("building 1"));

            Assert.AreEqual($"SELECT twin FROM DIGITALTWINS twin WHERE twin.name != 'building 1'", query.BuildAdtQuery());
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
        public void CanUseEscapableCharacters()
        {
            var someBuilding1 = "some'building";
            var someBuildingEscaped1 = @"some\'building";
            var someBuilding2 = "some'buil'ding";
            var someBuildingEscaped2 = @"some\'buil\'ding";

            var query = DynamicQueryBuilder
                      .FromTwins()
                      .Where(b => b.Property("name").IsEqualTo(someBuilding1));

            Assert.AreEqual($"SELECT twin FROM DIGITALTWINS twin WHERE twin.name = '{someBuildingEscaped1}'", query.BuildAdtQuery());

            query = DynamicQueryBuilder
                  .FromTwins()
                  .Where(b => b.Property("name").IsIn(new string[] { someBuilding1, someBuilding2 }));

            Assert.AreEqual($"SELECT twin FROM DIGITALTWINS twin WHERE twin.name IN ['{someBuildingEscaped1}','{someBuildingEscaped2}']", query.BuildAdtQuery());

            query = DynamicQueryBuilder
                  .FromTwins()
                  .Where(b => b.Property("name").StartsWith(someBuilding1));

            Assert.AreEqual($"SELECT twin FROM DIGITALTWINS twin WHERE STARTSWITH(twin.name, '{someBuildingEscaped1}')", query.BuildAdtQuery());
        }

        [TestMethod]
        public void FilteringOverEnumWithNoMemberAttributeUsesInt()
        {
            var query = DynamicQueryBuilder
                    .FromTwins()
                    .Where(x => x.Property("status").IsEqualTo(CustomEnum.Value2));

            Assert.AreEqual($"SELECT twin FROM DIGITALTWINS twin WHERE twin.status = '1'", query.BuildAdtQuery());
        }

        [TestMethod]
        public void CanFilterOnModelDtmi()
        {
            var dtmi = "dtmi:microsoft:Space:Building;1";
            var query = DynamicQueryBuilder
                    .FromTwins()
                    .Where(x => x.IsOfModel(dtmi));

            Assert.AreEqual($"SELECT twin FROM DIGITALTWINS twin WHERE IS_OF_MODEL(twin, '{dtmi}')", query.BuildAdtQuery());
        }

        [TestMethod]
        public void CanFilterWithRelationshipsQuery()
        {
            var query = DynamicQueryBuilder
                    .FromRelationships()
                    .Where(w => w.Property("$relationshipName").IsEqualTo("hasCalendar"));

            Assert.AreEqual("SELECT relationship FROM RELATIONSHIPS relationship WHERE relationship.$relationshipName = 'hasCalendar'", query.BuildAdtQuery());
        }

        [TestMethod]
        public void CanUseConjunctionsOnRelationshipQueries()
        {
            var query = DynamicQueryBuilder
                    .FromRelationships()
                    .Where(w => w
                        .Property("$relationshipName")
                        .IsEqualTo("hasCalendar")
                        .And(a => a
                            .Not(n => n
                                .Precedence(p => p
                                    .Property("$targetId")
                                    .IsEqualTo("someguid")
                                    .Or()
                                    .Property("$targetId")
                                    .IsEqualTo("someotherguid")))));

            Assert.AreEqual("SELECT relationship FROM RELATIONSHIPS relationship WHERE relationship.$relationshipName = 'hasCalendar' AND NOT (relationship.$targetId = 'someguid' OR relationship.$targetId = 'someotherguid')", query.BuildAdtQuery());
        }

        [TestMethod]
        public void CanFilterOnNestedJoins()
        {
            var query = DynamicQueryBuilder
                .FromTwins()
                .Join(j => j
                    .With("floor")
                    .RelatedBy("hasChildren")
                    .Join(i => i
                        .With("device")
                        .RelatedBy("hasDevices")
                        .Where(i => i.IsOfModel("dtmi:microsoft:somemodel;1"))));

            Assert.AreEqual("SELECT twin FROM DIGITALTWINS twin JOIN floor RELATED twin.hasChildren haschildrenrelationship JOIN device RELATED floor.hasDevices hasdevicesrelationship WHERE IS_OF_MODEL(device, 'dtmi:microsoft:somemodel;1')", query.BuildAdtQuery());
        }

        private enum CustomEnum
        {
            Value1,
            Value2
        }
    }
}