// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace QueryBuilder.UnitTests.QueryBuilder.Typed
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class QuerySelectTest
    {
        [TestMethod]
        public void FromGeneratesDefaultSelect()
        {
            var query = QueryBuilder
                .From<Building>();

            Assert.AreEqual($"SELECT building FROM DIGITALTWINS building WHERE IS_OF_MODEL(building, '{Building.ModelId.UpdateVersion(1)}')", query.BuildAdtQuery());
        }

        [TestMethod]
        public void CanModifyDefaultSelect()
        {
            var query = QueryBuilder
                .From<Building>()
                .Select<Building>();

            Assert.AreEqual($"SELECT building FROM DIGITALTWINS building WHERE IS_OF_MODEL(building, '{Building.ModelId.UpdateVersion(1)}')", query.BuildAdtQuery());

            query = QueryBuilder
                .From<Building>("bldng")
                .Select<Building>("bldng");

            Assert.AreEqual($"SELECT bldng FROM DIGITALTWINS bldng WHERE IS_OF_MODEL(bldng, '{Building.ModelId.UpdateVersion(1)}')", query.BuildAdtQuery());
        }

        [TestMethod]
        public void CanSelectAllFive()
        {
            var query = QueryBuilder
                .From<Building>()
                .Join<Building, Floor>(s => s.HasChildren)
                .Join<Floor, ConferenceRoom>(s => s.HasChildren)
                .Join<ConferenceRoom, Device>(s => s.HasDevices)
                .Join<Device, Sensor>(d => d.HasSensors)
                .Where<Building>(b => b.Id, ComparisonOperators.IsEqualTo, "ID")
                .Select<Building>()
                .Select<Floor>()
                .Select<ConferenceRoom>()
                .Select<Sensor>()
                .Select<Device>();

            var expectedQuery = $"SELECT building, floor, conferenceroom, sensor, device FROM DIGITALTWINS building JOIN floor RELATED building.hasChildren spacehaschildrenrelationship JOIN conferenceroom RELATED floor.hasChildren spacehaschildrenrelationship1 JOIN device RELATED conferenceroom.hasDevices spacehasdevicesrelationship JOIN sensor RELATED device.hasSensors devicehassensorsrelationship WHERE IS_OF_MODEL(building, '{Building.ModelId.UpdateVersion(1)}') AND IS_OF_MODEL(floor, '{Floor.ModelId.UpdateVersion(1)}') AND IS_OF_MODEL(conferenceroom, '{ConferenceRoom.ModelId.UpdateVersion(1)}') AND IS_OF_MODEL(device, '{Device.ModelId.UpdateVersion(1)}') AND IS_OF_MODEL(sensor, '{Sensor.ModelId.UpdateVersion(1)}') AND building.$dtId = 'ID'";

            Assert.AreEqual(expectedQuery, query.BuildAdtQuery());
        }

        [TestMethod]
        public void CanSelectAllUsingAlias()
        {
            var query = QueryBuilder
                .From<Building>("bldng")
                .Join<Building, Floor>(s => s.HasChildren, "bldng", "flr")
                .Join<Floor, ConferenceRoom>(s => s.HasChildren, "flr", "crm")
                .Join<ConferenceRoom, Device>(s => s.HasDevices, "crm", "dv")
                .Join<Device, Sensor>(d => d.HasSensors, "dv", "snsr")
                .Where<Building>(b => b.Id, ComparisonOperators.IsEqualTo, "ID")
                .Select<Building>("bldng")
                .Select<Floor>("flr")
                .Select<ConferenceRoom>("crm")
                .Select<Sensor>("snsr")
                .Select<Device>("dv");

            var expectedQuery = $"SELECT bldng, flr, crm, snsr, dv FROM DIGITALTWINS bldng JOIN flr RELATED bldng.hasChildren spacehaschildrenrelationship JOIN crm RELATED flr.hasChildren spacehaschildrenrelationship1 JOIN dv RELATED crm.hasDevices spacehasdevicesrelationship JOIN snsr RELATED dv.hasSensors devicehassensorsrelationship WHERE IS_OF_MODEL(bldng, '{Building.ModelId.UpdateVersion(1)}') AND IS_OF_MODEL(flr, '{Floor.ModelId.UpdateVersion(1)}') AND IS_OF_MODEL(crm, '{ConferenceRoom.ModelId.UpdateVersion(1)}') AND IS_OF_MODEL(dv, '{Device.ModelId.UpdateVersion(1)}') AND IS_OF_MODEL(snsr, '{Sensor.ModelId.UpdateVersion(1)}') AND bldng.$dtId = 'ID'";

            Assert.AreEqual(expectedQuery, query.BuildAdtQuery());
        }

        [TestMethod]
        public void CanSelectTwoOutOfFive()
        {
            var query = QueryBuilder
                .From<Building>()
                .Join<Building, Floor>(s => s.HasChildren)
                .Join<Floor, ConferenceRoom>(s => s.HasChildren)
                .Join<ConferenceRoom, Device>(s => s.HasDevices)
                .Join<Device, Sensor>(d => d.HasSensors)
                .Where<Building>(b => b.Id, ComparisonOperators.IsEqualTo, "ID")
                .Select<ConferenceRoom>()
                .Select<Sensor>();

            var expectedQuery = $"SELECT conferenceroom, sensor FROM DIGITALTWINS building JOIN floor RELATED building.hasChildren spacehaschildrenrelationship JOIN conferenceroom RELATED floor.hasChildren spacehaschildrenrelationship1 JOIN device RELATED conferenceroom.hasDevices spacehasdevicesrelationship JOIN sensor RELATED device.hasSensors devicehassensorsrelationship WHERE IS_OF_MODEL(building, '{Building.ModelId.UpdateVersion(1)}') AND IS_OF_MODEL(floor, '{Floor.ModelId.UpdateVersion(1)}') AND IS_OF_MODEL(conferenceroom, '{ConferenceRoom.ModelId.UpdateVersion(1)}') AND IS_OF_MODEL(device, '{Device.ModelId.UpdateVersion(1)}') AND IS_OF_MODEL(sensor, '{Sensor.ModelId.UpdateVersion(1)}') AND building.$dtId = 'ID'";

            Assert.AreEqual(expectedQuery, query.BuildAdtQuery());
        }

        [TestMethod]
        public void CanSelectSomeUsingAlias()
        {
            var query = QueryBuilder
                .From<Building>("bldng")
                .Join<Building, Floor>(s => s.HasChildren, "bldng", "flr")
                .Join<Floor, ConferenceRoom>(s => s.HasChildren, "flr", "crm")
                .Join<ConferenceRoom, Device>(s => s.HasDevices)
                .Join<Device, Sensor>(d => d.HasSensors)
                .Where<Building>(b => b.Id, ComparisonOperators.IsEqualTo, "ID")
                .Select<ConferenceRoom>("crm")
                .Select<Sensor>();

            var expectedQuery = $"SELECT crm, sensor FROM DIGITALTWINS bldng JOIN flr RELATED bldng.hasChildren spacehaschildrenrelationship JOIN crm RELATED flr.hasChildren spacehaschildrenrelationship1 JOIN device RELATED crm.hasDevices spacehasdevicesrelationship JOIN sensor RELATED device.hasSensors devicehassensorsrelationship WHERE IS_OF_MODEL(bldng, '{Building.ModelId.UpdateVersion(1)}') AND IS_OF_MODEL(flr, '{Floor.ModelId.UpdateVersion(1)}') AND IS_OF_MODEL(crm, '{ConferenceRoom.ModelId.UpdateVersion(1)}') AND IS_OF_MODEL(device, '{Device.ModelId.UpdateVersion(1)}') AND IS_OF_MODEL(sensor, '{Sensor.ModelId.UpdateVersion(1)}') AND bldng.$dtId = 'ID'";

            Assert.AreEqual(expectedQuery, query.BuildAdtQuery());
        }

        [TestMethod]
        public void CanAddFilterAfterSelect()
        {
            var query = QueryBuilder
                .From<Building>("bldng")
                .Join<Building, Floor>(s => s.HasChildren, "bldng", "flr")
                .Join<Floor, ConferenceRoom>(s => s.HasChildren, "flr", "crm")
                .Join<ConferenceRoom, Device>(s => s.HasDevices)
                .Join<Device, Sensor>(d => d.HasSensors)
                .Select<ConferenceRoom>("crm")
                .Select<Sensor>()
                .Where<Building>(b => b.Id, ComparisonOperators.IsEqualTo, "ID");

            var expectedQuery = $"SELECT crm, sensor FROM DIGITALTWINS bldng JOIN flr RELATED bldng.hasChildren spacehaschildrenrelationship JOIN crm RELATED flr.hasChildren spacehaschildrenrelationship1 JOIN device RELATED crm.hasDevices spacehasdevicesrelationship JOIN sensor RELATED device.hasSensors devicehassensorsrelationship WHERE IS_OF_MODEL(bldng, '{Building.ModelId.UpdateVersion(1)}') AND IS_OF_MODEL(flr, '{Floor.ModelId.UpdateVersion(1)}') AND IS_OF_MODEL(crm, '{ConferenceRoom.ModelId.UpdateVersion(1)}') AND IS_OF_MODEL(device, '{Device.ModelId.UpdateVersion(1)}') AND IS_OF_MODEL(sensor, '{Sensor.ModelId.UpdateVersion(1)}') AND bldng.$dtId = 'ID'";

            Assert.AreEqual(expectedQuery, query.BuildAdtQuery());
        }

        [TestMethod]
        public void CanAddJoinAfterSelect()
        {
            var query = QueryBuilder
                .From<Building>("bldng")
                .Select<Building>()
                .Join<Building, Floor>(s => s.HasChildren, "bldng", "flr")
                .Join<Floor, ConferenceRoom>(s => s.HasChildren, "flr", "crm")
                .Join<ConferenceRoom, Device>(s => s.HasDevices)
                .Join<Device, Sensor>(d => d.HasSensors)
                .Where<Building>(b => b.Id, ComparisonOperators.IsEqualTo, "ID")
                .Select<ConferenceRoom>("crm")
                .Select<Sensor>();

            var expectedQuery = $"SELECT bldng, crm, sensor FROM DIGITALTWINS bldng JOIN flr RELATED bldng.hasChildren spacehaschildrenrelationship JOIN crm RELATED flr.hasChildren spacehaschildrenrelationship1 JOIN device RELATED crm.hasDevices spacehasdevicesrelationship JOIN sensor RELATED device.hasSensors devicehassensorsrelationship WHERE IS_OF_MODEL(bldng, '{Building.ModelId.UpdateVersion(1)}') AND IS_OF_MODEL(flr, '{Floor.ModelId.UpdateVersion(1)}') AND IS_OF_MODEL(crm, '{ConferenceRoom.ModelId.UpdateVersion(1)}') AND IS_OF_MODEL(device, '{Device.ModelId.UpdateVersion(1)}') AND IS_OF_MODEL(sensor, '{Sensor.ModelId.UpdateVersion(1)}') AND bldng.$dtId = 'ID'";

            Assert.AreEqual(expectedQuery, query.BuildAdtQuery());
        }

        [TestMethod]
        public void DefaultSelectsOnlyFirst()
        {
            var query = QueryBuilder
                .From<Building>()
                .Join<Building, Floor>(s => s.HasChildren)
                .Join<Floor, ConferenceRoom>(s => s.HasChildren)
                .Join<ConferenceRoom, Device>(s => s.HasDevices)
                .Join<Device, Sensor>(d => d.HasSensors)
                .Where<Building>(b => b.Id, ComparisonOperators.IsEqualTo, "ID");

            var expectedQuery = $"SELECT building FROM DIGITALTWINS building JOIN floor RELATED building.hasChildren spacehaschildrenrelationship JOIN conferenceroom RELATED floor.hasChildren spacehaschildrenrelationship1 JOIN device RELATED conferenceroom.hasDevices spacehasdevicesrelationship JOIN sensor RELATED device.hasSensors devicehassensorsrelationship WHERE IS_OF_MODEL(building, '{Building.ModelId.UpdateVersion(1)}') AND IS_OF_MODEL(floor, '{Floor.ModelId.UpdateVersion(1)}') AND IS_OF_MODEL(conferenceroom, '{ConferenceRoom.ModelId.UpdateVersion(1)}') AND IS_OF_MODEL(device, '{Device.ModelId.UpdateVersion(1)}') AND IS_OF_MODEL(sensor, '{Sensor.ModelId.UpdateVersion(1)}') AND building.$dtId = 'ID'";

            Assert.AreEqual(expectedQuery, query.BuildAdtQuery());
        }

        [TestMethod]
        public void CanRetrieveSelectedAliases()
        {
            var query1 = QueryBuilder
                .From<Building>()
                .Join<Building, Floor>(s => s.HasChildren)
                .Join<Floor, ConferenceRoom>(s => s.HasChildren)
                .Join<ConferenceRoom, Device>(s => s.HasDevices)
                .Join<Device, Sensor>(d => d.HasSensors)
                .Where<Building>(b => b.Id, ComparisonOperators.IsEqualTo, "ID");
            var expectedAliases = new List<string> { "building" };
            Assert.IsFalse(expectedAliases.Where(a => !query1.SelectedAliases.Contains(a)).Any());

            var query2 = QueryBuilder
                .From<Building>()
                .Join<Building, Floor>(s => s.HasChildren)
                .Join<Floor, ConferenceRoom>(s => s.HasChildren)
                .Join<ConferenceRoom, Device>(s => s.HasDevices)
                .Join<Device, Sensor>(d => d.HasSensors)
                .Select<Building>()
                .Select<Floor>()
                .Select<ConferenceRoom>()
                .Select<Device>()
                .Select<Sensor>()
                .Where<Building>(b => b.Id, ComparisonOperators.IsEqualTo, "ID");
            expectedAliases = new List<string> { "building", "floor", "conferenceroom", "device", "sensor" };
            Assert.IsFalse(expectedAliases.Where(a => !query2.SelectedAliases.Contains(a)).Any());

            var query3 = QueryBuilder
                .From<Building>("b")
                .Join<Building, Floor>(s => s.HasChildren)
                .Join<Floor, ConferenceRoom>(s => s.HasChildren)
                .Join<ConferenceRoom, Device>(s => s.HasDevices)
                .Join<Device, Sensor>(d => d.HasSensors)
                .Select<Building>("b")
                .Select<Floor>()
                .Select<ConferenceRoom>()
                .Select<Device>()
                .Select<Sensor>()
                .Where<Building>(b => b.Id, ComparisonOperators.IsEqualTo, "ID");
            expectedAliases = new List<string> { "b", "floor", "conferenceroom", "device", "sensor" };
            Assert.IsFalse(expectedAliases.Where(a => !query3.SelectedAliases.Contains(a)).Any());

            var query4 = QueryBuilder
                .From<Building>();
            expectedAliases = new List<string> { "building" };
            Assert.IsFalse(expectedAliases.Where(a => !query4.SelectedAliases.Contains(a)).Any());

            var query5 = QueryBuilder
                .From<Building>()
                .Top(1);
            expectedAliases = new List<string> { "building" };
            Assert.IsFalse(expectedAliases.Where(a => !query5.SelectedAliases.Contains(a)).Any());

            var query6 = QueryBuilder
                .From<Building>()
                .Count();
            expectedAliases = new List<string> { };
            Assert.IsFalse(expectedAliases.Where(a => !query6.SelectedAliases.Contains(a)).Any());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AmbiguousQueryRequiresAliasesWhenSelecting()
        {
            var query = QueryBuilder
                .From<Space>()
                .Join<Space, Space>(s => s.HasChildren)
                .Select<Space>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CannotSelectSameAliasTwice()
        {
            var query = QueryBuilder
                .From<Space>("spc1")
                .Join<Space, Space>(s => s.HasChildren, "spc1", "spc2")
                .Select<Space>("spc1")
                .Select<Space>("spc1");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CannotSelectUndefinedAlias()
        {
            var query = QueryBuilder
                .From<Space>("spc1")
                .Join<Space, Space>(s => s.HasChildren, "spc1", "spc2")
                .Select<Space>("space");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CannotSelectMissingType()
        {
            var query = QueryBuilder
                .From<Space>("spc1")
                .Join<Space, Space>(s => s.HasChildren, "spc1", "spc2")
                .Select<Building>();
        }

        [TestMethod]
        public void CanSelectRelationship()
        {
            var query = QueryBuilder
                .From<Building>()
                .Join<Building, Floor>(s => s.HasChildren)
                .Where<Building>(b => b.Id, ComparisonOperators.IsEqualTo, "ID")
                .Select<Building>()
                .Select<SpaceHasChildrenRelationship>()
                .Select<Floor>();

            var expectedQuery = $"SELECT building, spacehaschildrenrelationship, floor FROM DIGITALTWINS building JOIN floor RELATED building.hasChildren spacehaschildrenrelationship WHERE IS_OF_MODEL(building, '{Building.ModelId.UpdateVersion(1)}') AND IS_OF_MODEL(floor, '{Floor.ModelId.UpdateVersion(1)}') AND building.$dtId = 'ID'";

            Assert.AreEqual(expectedQuery, query.BuildAdtQuery());
        }

        [TestMethod]
        public void CanSelectRelationshipWithAlias()
        {
            var query = QueryBuilder
                .From<Building>("b")
                .Join<Building, Floor>(s => s.HasChildren, "b", "f", "rel")
                .Where<Building>(b => b.Id, ComparisonOperators.IsEqualTo, "ID")
                .Select<Building>()
                .Select<SpaceHasChildrenRelationship>()
                .Select<Floor>();

            var expectedQuery = $"SELECT b, rel, f FROM DIGITALTWINS b JOIN f RELATED b.hasChildren rel WHERE IS_OF_MODEL(b, '{Building.ModelId.UpdateVersion(1)}') AND IS_OF_MODEL(f, '{Floor.ModelId.UpdateVersion(1)}') AND b.$dtId = 'ID'";

            Assert.AreEqual(expectedQuery, query.BuildAdtQuery());
        }

        [TestMethod]
        public void CanSelectSingleProperty()
        {
            var query = QueryBuilder
                .From<Building>()
                .Top(1)
                .Select<Building>(b => b.Number);

            var expectedQuery = $"SELECT TOP(1) building.number FROM DIGITALTWINS building WHERE IS_OF_MODEL(building, '{Building.ModelId.UpdateVersion(1)}')";

            Assert.AreEqual(expectedQuery, query.BuildAdtQuery());
        }

        [TestMethod]
        public void CanSelectMultipleProperties()
        {
            var query = QueryBuilder
                .From<Building>()
                .Select<Building>(b => b.BusinessEntityNumber)
                .Select<Building>(b => b.BusinessEntityName)
                .Select<Building>(b => b.Number)
                .Select<Building>(b => b.ShortName)
                .Select<Building>(b => b.Description)
                .Select<Building>(b => b.SquareFootArea);

            var expectedQuery = $"SELECT building.businessEntityNumber, building.businessEntityName, building.number, building.shortName, building.description, building.squareFootArea FROM DIGITALTWINS building WHERE IS_OF_MODEL(building, '{Building.ModelId.UpdateVersion(1)}')";

            Assert.AreEqual(expectedQuery, query.BuildAdtQuery());
        }

        [TestMethod]
        public void CanSelectMultiplePropertiesWithAliases()
        {
            var query = QueryBuilder
                .From<Building>("b")
                .Select<Building>("b", b => b.Number)
                .Select<Building>(b => b.Description);

            var expectedQuery = $"SELECT b.number, b.description FROM DIGITALTWINS b WHERE IS_OF_MODEL(b, '{Building.ModelId.UpdateVersion(1)}')";

            Assert.AreEqual(expectedQuery, query.BuildAdtQuery());
        }

        [TestMethod]
        public void CanSelectPropertiesFromMultipleTypes()
        {
            var query = QueryBuilder
                .From<Building>()
                .Join<Building, Floor>(s => s.HasChildren)
                .Where<Building>(b => b.Id, ComparisonOperators.IsEqualTo, "ID")
                .Select<Building>(b => b.FriendlyName)
                .Select<Floor>(f => f.Description);

            var expectedQuery = $"SELECT building.friendlyName, floor.description FROM DIGITALTWINS building JOIN floor RELATED building.hasChildren spacehaschildrenrelationship WHERE IS_OF_MODEL(building, '{Building.ModelId.UpdateVersion(1)}') AND IS_OF_MODEL(floor, '{Floor.ModelId.UpdateVersion(1)}') AND building.$dtId = 'ID'";

            Assert.AreEqual(expectedQuery, query.BuildAdtQuery());
        }
    }
}