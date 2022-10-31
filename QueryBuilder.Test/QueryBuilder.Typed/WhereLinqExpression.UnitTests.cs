// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace QueryBuilder.UnitTests.QueryBuilder.Typed
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;
    using global::QueryBuilder.Test.Generated;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class QueryWhereLinqExpressionTest
    {
        [TestMethod]
        public void CanApplyMultiModelCompoundFilter()
        {
            var floor = new Floor { Name = "floorName" };
            var query = QueryBuilder
                .From<Building>()
                .Join<Building, Device>(building => building.HasDevices)
                .Join<Device, Sensor>(device => device.HasSensors)
                .Where<Building>(building => (building.Id != null && building.Id == floor.Name) || building.Name is string)
                .Where<Device>(device => (device.Name != null && device.Name == "name") || device.Description is string)
                .Select<Sensor>();
            var expected =
                "SELECT sensor FROM DIGITALTWINS building " +
                "JOIN device RELATED building.hasDevices spacehasdevicesrelationship " +
                "JOIN sensor RELATED device.hasSensors devicehassensorsrelationship " +
                $"WHERE IS_OF_MODEL(building, '{Building.ModelId.UpdateVersion(1)}') " +
                $"AND IS_OF_MODEL(device, '{Device.ModelId.UpdateVersion(1)}') " +
                $"AND IS_OF_MODEL(sensor, '{Sensor.ModelId.UpdateVersion(1)}') " +
                $"AND (NOT IS_NULL(building.$dtId) AND building.$dtId = '{floor.Name}') OR IS_STRING(building.name) " +
                "AND (NOT IS_NULL(device.name) AND device.name = 'name') OR IS_STRING(device.description)";
            Assert.AreEqual(expected, query.BuildAdtQuery());
        }

        [TestMethod]
        public void CanApplySingleLevelPropWithEnum()
        {
            var query = QueryBuilder
                .From<Space>()
                .Where<Space>(space => space.Status == SpaceStatus.Active);
        }

        [TestMethod]
        public void CanApplySimpleAndExpression()
        {
            TestLinqExpressionWhereClause(
                building => building.Name == null && building.Name == null,
                "IS_NULL(building.name) AND IS_NULL(building.name)");
        }

        [TestMethod]
        public void CanApplySimpleOrExpression()
        {
            TestLinqExpressionWhereClause(
                building => building.Name == null || building.Name == null,
                "IS_NULL(building.name) OR IS_NULL(building.name)");
        }

        [TestMethod]
        public void CanApplyNestedAndExpression()
        {
            TestLinqExpressionWhereClause(
                building => building.Name == null && (building.Name == null && building.Name == "a"),
                "IS_NULL(building.name) AND IS_NULL(building.name) AND building.name = 'a'");
            TestLinqExpressionWhereClause(
                building => (building.Name == null && building.Name == "a") && building.Name == null,
                "IS_NULL(building.name) AND building.name = 'a' AND IS_NULL(building.name)");
            TestLinqExpressionWhereClause(
                building => (building.Name == "a" && building.Name == "b") && (building.Name == "c" && building.Name == "d"),
                "building.name = 'a' AND building.name = 'b' AND building.name = 'c' AND building.name = 'd'");
        }

        [TestMethod]
        public void CanApplyNestedOrExpression()
        {
            TestLinqExpressionWhereClause(
                building => building.Name == null || (building.Name == null || building.Name == "a"),
                "IS_NULL(building.name) OR IS_NULL(building.name) OR building.name = 'a'");
            TestLinqExpressionWhereClause(
                building => (building.Name == null || building.Name == "a") || building.Name == null,
                "IS_NULL(building.name) OR building.name = 'a' OR IS_NULL(building.name)");
            TestLinqExpressionWhereClause(
                building => (building.Name == "a" || building.Name == "b") || (building.Name == "c" || building.Name == "d"),
                "building.name = 'a' OR building.name = 'b' OR building.name = 'c' OR building.name = 'd'");
        }

        [TestMethod]
        public void CanApplyNestedAndOrExpressions()
        {
            TestLinqExpressionWhereClause(
                building => building.Name.StartsWith("a") || ((building.Name == "a" && building.RoomKey == "1") || (building.Name == "b" && building.RoomKey == "2") || building.Name == null),
                "STARTSWITH(building.name, 'a') OR (building.name = 'a' AND building.roomKey = '1') OR (building.name = 'b' AND building.roomKey = '2') OR IS_NULL(building.name)");
        }

        [TestMethod]
        [SuppressMessage("Usage", "SA1408: Conditional expressions should declare precedence", Justification = "Testing ambiguous case.")]
        public void CanAplyAmbiguousPrecedence()
        {
            TestLinqExpressionWhereClause(
                building => building.Name == null || building.Name == null && building.Name == null,
                "IS_NULL(building.name) OR (IS_NULL(building.name) AND IS_NULL(building.name))");
        }

        [TestMethod]
        public void CanApplyScalarUnaryFilters()
        {
            TestLinqExpressionWhereClause(building => building.Name != null, "NOT IS_NULL(building.name)");
            TestLinqExpressionWhereClause(building => building.Name == null, "IS_NULL(building.name)");
            TestLinqExpressionWhereClause(building => building.Name is string, "IS_STRING(building.name)");
            TestLinqExpressionWhereClause(building => building.EndOfBusinessTime is bool, "IS_BOOL(building.endOfBusinessTime)");
            TestLinqExpressionWhereClause(building => building.EndOfBusinessTime is int, "IS_NUMBER(building.endOfBusinessTime)");
            TestLinqExpressionWhereClause(building => building.EndOfBusinessTime is short, "IS_NUMBER(building.endOfBusinessTime)");
            TestLinqExpressionWhereClause(building => building.EndOfBusinessTime is long, "IS_NUMBER(building.endOfBusinessTime)");
            TestLinqExpressionWhereClause(building => building.EndOfBusinessTime is double, "IS_NUMBER(building.endOfBusinessTime)");
            TestLinqExpressionWhereClause(building => building.EndOfBusinessTime is float, "IS_NUMBER(building.endOfBusinessTime)");
            TestLinqExpressionWhereClause(building => building.EndOfBusinessTime is object, "IS_OBJECT(building.endOfBusinessTime)");
            TestLinqExpressionWhereClause(building => building.Amenities.Values is object, "IS_OBJECT(building.amenities.values)");
        }

        [TestMethod]
        public void CanApplyScalarUnaryFiltersFunctionCall()
        {
            var randomString = "testName";
            TestLinqExpressionWhereClause(building => building.Name.StartsWith(randomString), $"STARTSWITH(building.name, '{randomString}')");
            TestLinqExpressionWhereClause(building => building.Name.EndsWith(randomString), $"ENDSWITH(building.name, '{randomString}')");
            TestLinqExpressionWhereClause(building => building.Name.Contains(randomString), $"CONTAINS(building.name, '{randomString}')");
        }

        [TestMethod]
        public void CanApplyComparisonFilters()
        {
            TestLinqExpressionWhereClause(building => building.Name == "test", "building.name = 'test'");
            TestLinqExpressionWhereClause(building => building.Name != "test", "building.name != 'test'");
            TestLinqExpressionWhereClause(building => building.SquareFootArea >= 50, "building.squareFootArea >= 50");
            TestLinqExpressionWhereClause(building => building.SquareFootArea > 50, "building.squareFootArea > 50");
            TestLinqExpressionWhereClause(building => building.SquareFootArea <= 50, "building.squareFootArea <= 50");
            TestLinqExpressionWhereClause(building => building.SquareFootArea < 50, "building.squareFootArea < 50");
        }

        [TestMethod]
        [ExpectedException(typeof(LinqExpressionNotSupportedException), "The given expression is not a Binary comparison.")]
        public void NoPropertyNameThrowsException()
        {
            QueryBuilder
                .From<Building>()
                .Where<Building>(building => 1 == 2);
        }

        [TestMethod]
        [ExpectedException(typeof(LinqOperatorNotSupportedException), "The given expression includes function call that's not supported.")]
        public void MethodNotSupportedThrowsException()
        {
            QueryBuilder
                .From<Building>()
                .Where<Building>(building => building.Name.IsNormalized());
        }

        [TestMethod]
        [ExpectedException(typeof(LinqExpressionNotSupportedException), "The given expression is always true.")]
        public void NoPropertyThrowsException()
        {
            QueryBuilder
                .From<Building>()
                .Where<Building>(building => true);
        }

        [TestMethod]
        [ExpectedException(typeof(LinqOperatorNotSupportedException), "The Array type is not supported.")]
        public void TypeNotSupportedThrowsException()
        {
            QueryBuilder
                .From<Building>()
                .Where<Building>(building => building.EndOfBusinessTime is Array);
        }

        [TestMethod]
        [ExpectedException(typeof(LinqOperatorNotSupportedException), "XOR expressions are not supported.")]
        public void ExpressionTypeNotSupportedThrowsException()
        {
            QueryBuilder
                .From<Building>()
                .Where<Building>(building => building.Number == 1 ^ building.Number == 0);
        }

        [TestMethod]
        [ExpectedException(typeof(LinqExpressionNotSupportedException), "Null value provided.")]
        public void NullValueThrowsException()
        {
            var myBuilding = new Building { Name = null };
            QueryBuilder
                .From<Building>()
                .Where<Building>(building => building.Name.Contains(myBuilding.Name));
        }

        private void TestLinqExpressionWhereClause(Expression<Func<Building, bool>> exp, string expectedOpString)
        {
            var query = QueryBuilder
                .From<Building>()
                .Where<Building>(exp);
            var expected = $"SELECT building FROM DIGITALTWINS building WHERE IS_OF_MODEL(building, '{Building.ModelId.UpdateVersion(1)}') AND {expectedOpString}";
            Assert.AreEqual(expected, query.BuildAdtQuery());
        }
    }
}