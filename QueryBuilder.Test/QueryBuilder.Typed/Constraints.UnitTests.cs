// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace QueryBuilder.UnitTests.QueryBuilder.Typed
{
    using System.Linq;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class QueryConstraintsTest
    {
        [TestMethod]
        public void CanNotBuildEmptyQuery()
        {
            Assert.IsNull(typeof(QueryBuilder).GetMethod("BuildAdtQuery"));
        }

        [TestMethod]
        public void CanInvokeFromOnlyOnce()
        {
            var query = QueryBuilder
                .From<Building>();

            Assert.IsNull(query.GetType().GetMethod("From"));
        }

        [TestMethod]
        public void CanNotInvokeSelectMoreThanSixTimes()
        {
            var query = QueryBuilder
                .From<Building>()
                .Select<Building>()
                .Select<Building>()
                .Select<Building>()
                .Select<Building>()
                .Select<Building>()
                .Select<Building>();

            Assert.IsNull(query.GetType().GetMethod("Select"));
        }

        [TestMethod]
        public void DefaultQueryAllowsTop()
        {
            var query = QueryBuilder
                .From<Building>();

            Assert.IsNotNull(query.GetType().GetMethod("Top"));
        }

        [TestMethod]
        public void DefaultQueryAllowsSelect()
        {
            var query = QueryBuilder
                .From<Building>();

            Assert.IsTrue(query.GetType().GetMethods().Any(mi => mi.Name == "Select"));
        }

        [TestMethod]
        public void DefaultQueryAllowsCount()
        {
            var query = QueryBuilder
                .From<Building>();

            Assert.IsNotNull(query.GetType().GetMethod("Count"));
        }

        [TestMethod]
        public void DefaultQueryAllowsCommonBehavior()
        {
            var query = QueryBuilder
                .From<Building>();

            TestQueryAllowsCommonBehavior(query);
        }

        [TestMethod]
        public void CountQueryDoesNotAllowTop()
        {
            var query = QueryBuilder
                .From<Building>()
                .Count();

            Assert.IsNull(query.GetType().GetMethod("Top"));
        }

        [TestMethod]
        public void CountQueryDoesNotAllowSelect()
        {
            var query = QueryBuilder
                .From<Building>()
                .Count();

            Assert.IsNull(query.GetType().GetMethod("Select"));
        }

        [TestMethod]
        public void CountQueryAllowsCommonBehavior()
        {
            var query = QueryBuilder
                .From<Building>()
                .Count();

            TestQueryAllowsCommonBehavior(query);
        }

        [TestMethod]
        public void DefaultSelectQueryAllowsSelect()
        {
            var query = QueryBuilder
                .From<Building>()
                .Top(2);

            Assert.IsTrue(query.GetType().GetMethods().Any(mi => mi.Name == "Select"));
        }

        [TestMethod]
        public void DefaultSelectQueryDoesNotAllowCount()
        {
            var query = QueryBuilder
                .From<Building>()
                .Top(2);

            Assert.IsNull(query.GetType().GetMethod("Count"));
        }

        [TestMethod]
        public void DefaultSelectQueryAllowsCommonBehavior()
        {
            var query = QueryBuilder
                .From<Building>()
                .Top(2);

            TestQueryAllowsCommonBehavior(query);
        }

        [TestMethod]
        public void OneSelectQueryAllowsSelect()
        {
            var query = QueryBuilder
                .From<Building>()
                .Select<Building>();

            Assert.IsTrue(query.GetType().GetMethods().Any(mi => mi.Name == "Select"));
        }

        [TestMethod]
        public void OneSelectQueryAllowsTop()
        {
            var query = QueryBuilder
                .From<Building>()
                .Select<Building>();

            Assert.IsNotNull(query.GetType().GetMethod("Top"));
        }

        [TestMethod]
        public void OneSelectQueryDoesNotAllowCount()
        {
            var query = QueryBuilder
                .From<Building>()
                .Select<Building>();

            Assert.IsNull(query.GetType().GetMethod("Count"));
        }

        [TestMethod]
        public void OneSelectQueryAllowsCommonBehavior()
        {
            var query = QueryBuilder
                .From<Building>()
                .Select<Building>();

            TestQueryAllowsCommonBehavior(query);
        }

        [TestMethod]
        public void TwoSelectQueryAllowsSelect()
        {
            var query = QueryBuilder
                .From<Building>()
                .Select<Building>()
                .Select<Building>();

            Assert.IsTrue(query.GetType().GetMethods().Any(mi => mi.Name == "Select"));
        }

        [TestMethod]
        public void TwoSelectQueryAllowsTop()
        {
            var query = QueryBuilder
                .From<Building>()
                .Select<Building>()
                .Select<Building>();

            Assert.IsNotNull(query.GetType().GetMethod("Top"));
        }

        [TestMethod]
        public void TwoSelectQueryDoesNotAllowCount()
        {
            var query = QueryBuilder
                .From<Building>()
                .Select<Building>()
                .Select<Building>();

            Assert.IsNull(query.GetType().GetMethod("Count"));
        }

        [TestMethod]
        public void TwoSelectQueryAllowsCommonBehavior()
        {
            var query = QueryBuilder
                .From<Building>()
                .Select<Building>()
                .Select<Building>();

            TestQueryAllowsCommonBehavior(query);
        }

        private void TestQueryAllowsCommonBehavior(object query)
        {
            Assert.IsNotNull(query.GetType().GetMethods().FirstOrDefault(m => m.Name == "Join"));
            TestQueryAllowsWhere(query);
        }

        private void TestQueryDoesNotAllowCommonBehavior(object query)
        {
            Assert.IsNull(query.GetType().GetMethods().FirstOrDefault(m => m.Name == "Join"));
            TestQueryDoesNotAllowWhere(query);
        }

        private void TestQueryAllowsWhere(object query)
        {
            Assert.IsNotNull(query.GetType().GetMethods().FirstOrDefault(m => m.Name == "Where"));
            Assert.IsNotNull(query.GetType().GetMethods().FirstOrDefault(m => m.Name == "WhereIn"));
            Assert.IsNotNull(query.GetType().GetMethods().FirstOrDefault(m => m.Name == "WhereNotIn"));
            Assert.IsNotNull(query.GetType().GetMethods().FirstOrDefault(m => m.Name == "WhereStartsWith"));
            Assert.IsNotNull(query.GetType().GetMethods().FirstOrDefault(m => m.Name == "WhereEndsWith"));
        }

        private void TestQueryDoesNotAllowWhere(object query)
        {
            Assert.IsNull(query.GetType().GetMethods().FirstOrDefault(m => m.Name == "Where"));
            Assert.IsNull(query.GetType().GetMethods().FirstOrDefault(m => m.Name == "WhereIn"));
            Assert.IsNull(query.GetType().GetMethods().FirstOrDefault(m => m.Name == "WhereNotIn"));
            Assert.IsNull(query.GetType().GetMethods().FirstOrDefault(m => m.Name == "WhereStartsWith"));
            Assert.IsNull(query.GetType().GetMethods().FirstOrDefault(m => m.Name == "WhereEndsWith"));
        }
    }
}