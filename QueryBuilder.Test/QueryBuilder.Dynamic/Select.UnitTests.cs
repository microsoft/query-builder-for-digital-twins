// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace QueryBuilder.UnitTests.QueryBuilder.Dynamic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Dynamic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class QuerySelectTest
    {
        [TestMethod]
        public void FromGeneratesDefaultSelect()
        {
            var query = DynamicQueryBuilder
                .FromTwins();

            Assert.AreEqual($"SELECT twin FROM DIGITALTWINS twin", query.BuildAdtQuery());
        }

        [TestMethod]
        public void CanModifyDefaultSelect()
        {
            var query = DynamicQueryBuilder
                .FromTwins()
                .Select("twin");

            Assert.AreEqual($"SELECT twin FROM DIGITALTWINS twin", query.BuildAdtQuery());

            query = DynamicQueryBuilder
                .FromTwins("bldng")
                .Select("bldng");

            Assert.AreEqual($"SELECT bldng FROM DIGITALTWINS bldng", query.BuildAdtQuery());
        }

        [TestMethod]
        public void CanModifyDefaultSelectForRelationships()
        {
            var query = DynamicQueryBuilder
                    .FromRelationships("rel")
                    .Select("rel");
            Assert.AreEqual("SELECT rel FROM RELATIONSHIPS rel", query.BuildAdtQuery());
        }

        [TestMethod]
        public void CanSelectAllFive()
        {
            var query = DynamicQueryBuilder
                .FromTwins()
                .Join(j => j.With("floor").RelatedBy("hasChildren"))
                .Join(j => j.With("confroom").RelatedBy("hasChildren").On("floor").WithAlias("haschildrenrelationship1"))
                .Join(j => j.With("device").RelatedBy("hasDevices").On("confroom"))
                .Join(j => j.With("sensor").RelatedBy("hasSensors").On("device"))
                .Where(w => w.Property("$dtId").IsEqualTo("ID"))
                .Select("twin", "floor", "confroom", "sensor", "device");

            var expectedQuery = $"SELECT twin, floor, confroom, sensor, device FROM DIGITALTWINS twin JOIN floor RELATED twin.hasChildren haschildrenrelationship JOIN confroom RELATED floor.hasChildren haschildrenrelationship1 JOIN device RELATED confroom.hasDevices hasdevicesrelationship JOIN sensor RELATED device.hasSensors hassensorsrelationship WHERE twin.$dtId = 'ID'";

            Assert.AreEqual(expectedQuery, query.BuildAdtQuery());
        }

        [TestMethod]
        public void CanSelectTwoOutOfFive()
        {
            var query = DynamicQueryBuilder
                .FromTwins()
                .Join(j => j.With("floor").RelatedBy("hasChildren"))
                .Join(j => j.With("confroom").RelatedBy("hasChildren").On("floor").WithAlias("haschildrenrelationship1"))
                .Join(j => j.With("device").RelatedBy("hasDevices").On("confroom"))
                .Join(j => j.With("sensor").RelatedBy("hasSensors").On("device"))
                .Where(w => w.Property("$dtId").IsEqualTo("ID"))
                .Select("confroom", "sensor");

            var expectedQuery = $"SELECT confroom, sensor FROM DIGITALTWINS twin JOIN floor RELATED twin.hasChildren haschildrenrelationship JOIN confroom RELATED floor.hasChildren haschildrenrelationship1 JOIN device RELATED confroom.hasDevices hasdevicesrelationship JOIN sensor RELATED device.hasSensors hassensorsrelationship WHERE twin.$dtId = 'ID'";

            Assert.AreEqual(expectedQuery, query.BuildAdtQuery());
        }

        [TestMethod]
        public void CanAddFilterAfterSelect()
        {
            var query = DynamicQueryBuilder
                .FromTwins("bldng")
                .Join(j => j.With("flr").RelatedBy("hasChildren").On("bldng"))
                .Join(j => j.With("crm").RelatedBy("hasChildren").On("flr").WithAlias("haschildrenrelationship1"))
                .Join(j => j.With("device").RelatedBy("hasDevices").On("crm"))
                .Join(j => j.With("sensor").RelatedBy("hasSensors").On("device"))
                .Select("crm", "sensor")
                .Where(b => b.Property("$dtId").IsEqualTo("ID"));

            var expectedQuery = $"SELECT crm, sensor FROM DIGITALTWINS bldng JOIN flr RELATED bldng.hasChildren haschildrenrelationship JOIN crm RELATED flr.hasChildren haschildrenrelationship1 JOIN device RELATED crm.hasDevices hasdevicesrelationship JOIN sensor RELATED device.hasSensors hassensorsrelationship WHERE bldng.$dtId = 'ID'";

            Assert.AreEqual(expectedQuery, query.BuildAdtQuery());
        }

        [TestMethod]
        public void CanAddJoinAfterSelect()
        {
            var query = DynamicQueryBuilder
                .FromTwins("bldng")
                .Select("bldng")
                .Join(j => j.With("flr").RelatedBy("hasChildren").On("bldng"))
                .Join(j => j.With("crm").RelatedBy("hasChildren").On("flr").WithAlias("haschildrenrelationship1"))
                .Join(j => j.With("device").RelatedBy("hasDevices").On("crm"))
                .Join(j => j.With("sensor").RelatedBy("hasSensors").On("device"))
                .Where(b => b.Property("$dtId").IsEqualTo("ID"));

            var expectedQuery = $"SELECT bldng FROM DIGITALTWINS bldng JOIN flr RELATED bldng.hasChildren haschildrenrelationship JOIN crm RELATED flr.hasChildren haschildrenrelationship1 JOIN device RELATED crm.hasDevices hasdevicesrelationship JOIN sensor RELATED device.hasSensors hassensorsrelationship WHERE bldng.$dtId = 'ID'";

            Assert.AreEqual(expectedQuery, query.BuildAdtQuery());
        }

        [TestMethod]
        public void DefaultSelectsOnlyFirst()
        {
            var query = DynamicQueryBuilder
                .FromTwins()
                .Join(j => j.With("floor").RelatedBy("hasChildren"))
                .Join(j => j.With("confroom").RelatedBy("hasChildren").On("floor").WithAlias("haschildrenrelationship1"))
                .Join(j => j.With("device").RelatedBy("hasDevices").On("confroom"))
                .Join(j => j.With("sensor").RelatedBy("hasSensors").On("device"))
                .Where(w => w.Property("$dtId").IsEqualTo("ID"));

            var expectedQuery = $"SELECT twin FROM DIGITALTWINS twin JOIN floor RELATED twin.hasChildren haschildrenrelationship JOIN confroom RELATED floor.hasChildren haschildrenrelationship1 JOIN device RELATED confroom.hasDevices hasdevicesrelationship JOIN sensor RELATED device.hasSensors hassensorsrelationship WHERE twin.$dtId = 'ID'";

            Assert.AreEqual(expectedQuery, query.BuildAdtQuery());
        }

        [TestMethod]
        public void CanRetrieveSelectedAliases()
        {
            var query1 = DynamicQueryBuilder
                .FromTwins()
                .Join(j => j.With("floor").RelatedBy("hasChildren"))
                .Join(j => j.With("confroom").RelatedBy("hasChildren").On("floor").WithAlias("haschildrenrelationship1"))
                .Join(j => j.With("device").RelatedBy("hasDevices").On("confroom"))
                .Join(j => j.With("sensor").RelatedBy("hasSensors").On("device"))
                .Where(w => w.Property("$dtId").IsEqualTo("ID"));
            var expectedAliases = new List<string> { "twin" };
            Assert.IsFalse(expectedAliases.Where(a => !query1.SelectedAliases.Contains(a)).Any());

            var query2 = DynamicQueryBuilder
                .FromTwins()
                .Join(j => j.With("floor").RelatedBy("hasChildren"))
                .Join(j => j.With("confroom").RelatedBy("hasChildren").On("floor").WithAlias("haschildrenrelationship1"))
                .Join(j => j.With("device").RelatedBy("hasDevices").On("confroom"))
                .Join(j => j.With("sensor").RelatedBy("hasSensors").On("device"))
                .Select("twin", "floor", "confroom", "device", "sensor")
                .Where(w => w.Property("$dtId").IsEqualTo("ID"));
            expectedAliases = new List<string> { "twin", "floor", "confroom", "device", "sensor" };
            Assert.IsFalse(expectedAliases.Where(a => !query2.SelectedAliases.Contains(a)).Any());

            var query3 = DynamicQueryBuilder
                .FromTwins("bldng")
                .Join(j => j.With("floor").RelatedBy("hasChildren"))
                .Join(j => j.With("confroom").RelatedBy("hasChildren").On("floor").WithAlias("haschildrenrelationship1"))
                .Join(j => j.With("device").RelatedBy("hasDevices").On("confroom"))
                .Join(j => j.With("sensor").RelatedBy("hasSensors").On("device"))
                .Select("bldng", "floor", "confroom", "device", "sensor")
                .Where(w => w.Property("$dtId").IsEqualTo("ID"));
            expectedAliases = new List<string> { "bldng", "floor", "confroom", "device", "sensor" };
            Assert.IsFalse(expectedAliases.Where(a => !query3.SelectedAliases.Contains(a)).Any());

            var query4 = DynamicQueryBuilder
                .FromTwins();
            expectedAliases = new List<string> { "twin" };
            Assert.IsFalse(expectedAliases.Where(a => !query4.SelectedAliases.Contains(a)).Any());

            var query5 = DynamicQueryBuilder
                .FromTwins()
                .Top(1);
            expectedAliases = new List<string> { "twin" };
            Assert.IsFalse(expectedAliases.Where(a => !query5.SelectedAliases.Contains(a)).Any());

            var query6 = DynamicQueryBuilder
                .FromTwins()
                .Count();
            expectedAliases = new List<string> { };
            Assert.IsFalse(expectedAliases.Where(a => !query6.SelectedAliases.Contains(a)).Any());
        }

        [TestMethod]
        public void CanSelectRelationshipProperties()
        {
            var query = DynamicQueryBuilder
                    .FromRelationships("rel")
                    .Select("$relationshipName");

            Assert.AreEqual("SELECT rel.$relationshipName FROM RELATIONSHIPS rel", query.BuildAdtQuery());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CannotSelectSameAliasTwice()
        {
            var query = DynamicQueryBuilder
                .FromTwins("spc1")
                .Join(j => j.With("spc2").RelatedBy("hasChildren").On("spc1"))
                .Select("spc1", "spc1");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CannotSelectUndefinedAlias()
        {
            var query = DynamicQueryBuilder
                .FromTwins("spc1")
                .Join(j => j.With("spc2").RelatedBy("hasChildren").On("spc1"))
                .Select("bldng");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CannotSelectUndefinedAliasForRelationships()
        {
            var query = DynamicQueryBuilder
                .FromRelationships("rel1")
                .Select("bldng");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CannotSelectSameAliasTwiceForRelationships()
        {
            var query = DynamicQueryBuilder
                .FromRelationships("spc1")
                .Select("spc1", "spc1");
        }
    }
}