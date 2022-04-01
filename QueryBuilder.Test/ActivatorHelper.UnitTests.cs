// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace QueryBuilder.UnitTests.QueryBuilder
{
    using System;
    using System.Reflection;
    using Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.Common.Helpers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ActivatorHelperTests
    {
        [TestMethod]
        public void CanCreateInstance()
        {
            var id = Guid.NewGuid();
            var instance = ActivatorHelper.CreateInstance<TestClass>(id);
            Assert.IsNotNull(instance);
            Assert.AreEqual(id, instance.RandomId);
        }

        [TestMethod]
        public void CanCreateInstanceWithFlags()
        {
            var id = Guid.NewGuid();
            var instance = ActivatorHelper.CreateInstance<TestClass>(new object[] { id }, BindingFlags.Public | BindingFlags.CreateInstance | BindingFlags.Instance);
            Assert.IsNotNull(instance);
            Assert.AreEqual(id, instance.RandomId);
        }

        [TestMethod]
        public void CanCreateInstanceWithoutParams()
        {
            var instance = ActivatorHelper.CreateInstance<TestClass>();
            Assert.IsNotNull(instance);
            Assert.IsNotNull(instance.RandomId);
            Assert.IsTrue(instance.RandomId != Guid.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ThrowsIfNoConstructorsMatchParams()
        {
            ActivatorHelper.CreateInstance<TestClass>(Guid.NewGuid(), "randomArg");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ThrowsIfFlagsDoNotReturnAnyConstructors()
        {
            ActivatorHelper.CreateInstance<TestClass>(new object[] { Guid.NewGuid() }, BindingFlags.Instance);
        }
    }

    internal class TestClass
    {
        internal Guid RandomId;

        public TestClass(Guid randomId)
        {
            RandomId = randomId;
        }

        public TestClass() : this(Guid.NewGuid())
        {
        }
    }
}