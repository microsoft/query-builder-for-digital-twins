// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace QueryBuilder.UnitTests.QueryBuilder.Dynamic
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
            var query = QueryBuilder.FromTwins();
            Assert.IsNull(query.GetType().GetMethod("FromTwins"));
        }

        [TestMethod]
        public void DefaultQueryAllowsTop()
        {
            var query = QueryBuilder.FromTwins();
            Assert.IsNotNull(query.GetType().GetMethod("Top"));
        }

        [TestMethod]
        public void DefaultQueryAllowsSelect()
        {
            var query = QueryBuilder.FromTwins();
            Assert.IsNotNull(query.GetType().GetMethod("Select"));
        }

        [TestMethod]
        public void DefaultQueryAllowsCount()
        {
            var query = QueryBuilder.FromTwins();
            Assert.IsNotNull(query.GetType().GetMethod("Count"));
        }

        [TestMethod]
        public void DefaultQueryAllowsCommonBehavior()
        {
            var query = QueryBuilder.FromTwins();
            TestQueryAllowsCommonBehavior(query);
        }

        [TestMethod]
        public void CountQueryDoesNotAllowTop()
        {
            var query = QueryBuilder
                .FromTwins()
                .Count();

            Assert.IsNull(query.GetType().GetMethod("Top"));
        }

        [TestMethod]
        public void CountQueryDoesNotAllowSelect()
        {
            var query = QueryBuilder
                .FromTwins()
                .Count();

            Assert.IsNull(query.GetType().GetMethod("Select"));
        }

        [TestMethod]
        public void CountQueryAllowsCommonBehavior()
        {
            var query = QueryBuilder
                .FromTwins()
                .Count();

            TestQueryAllowsCommonBehavior(query);
        }

        [TestMethod]
        public void DefaultSelectQueryAllowsSelect()
        {
            var query = QueryBuilder
                .FromTwins()
                .Top(2);

            Assert.IsNotNull(query.GetType().GetMethod("Select"));
        }

        [TestMethod]
        public void DefaultSelectQueryDoesNotAllowCount()
        {
            var query = QueryBuilder
                .FromTwins()
                .Top(2);

            Assert.IsNull(query.GetType().GetMethod("Count"));
        }

        [TestMethod]
        public void DefaultSelectQueryAllowsCommonBehavior()
        {
            var query = QueryBuilder
                .FromTwins()
                .Top(2);

            TestQueryAllowsCommonBehavior(query);
        }

        [TestMethod]
        public void OneSelectQueryDoesNotAllowSelect()
        {
            var query = QueryBuilder
                .FromTwins("building")
                .Select("building");

            Assert.IsNull(query.GetType().GetMethod("Select"));
        }

        [TestMethod]
        public void OneSelectQueryAllowsTop()
        {
            var query = QueryBuilder
                .FromTwins("building")
                .Select("building");

            Assert.IsNotNull(query.GetType().GetMethod("Top"));
        }

        [TestMethod]
        public void OneSelectQueryDoesNotAllowCount()
        {
            var query = QueryBuilder
                .FromTwins("building")
                .Select("building");

            Assert.IsNull(query.GetType().GetMethod("Count"));
        }

        [TestMethod]
        public void OneSelectQueryAllowsCommonBehavior()
        {
            var query = QueryBuilder
                .FromTwins("building")
                .Select("building");

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
        }

        private void TestQueryDoesNotAllowWhere(object query)
        {
            Assert.IsNull(query.GetType().GetMethods().FirstOrDefault(m => m.Name == "Where"));
        }
    }
}