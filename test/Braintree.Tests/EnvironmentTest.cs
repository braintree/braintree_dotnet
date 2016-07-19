using NUnit.Framework;
using System;
using Braintree;
using Braintree.Exceptions;

namespace Braintree.Tests
{
    [TestFixture]
    public class EnvironmentTest
    {
        [Test]
        [Category("Unit")]
        public void ParseEnvironment_ReturnsSandbox()
        {
            Environment environment = Environment.ParseEnvironment("sandbox");
            Assert.AreEqual(environment, Environment.SANDBOX);
        }

        [Test]
        [Category("Unit")]
        public void ParseEnvironment_ReturnsDevelopment()
        {
            Environment environment = Environment.ParseEnvironment("development");
            Assert.AreEqual(environment, Environment.DEVELOPMENT);
        }

        [Test]
        [Category("Unit")]
        public void ParseEnvironment_ReturnsQA()
        {
            Environment environment = Environment.ParseEnvironment("qa");
            Assert.AreEqual(environment, Environment.QA);
        }

        [Test]
        [Category("Unit")]
        public void ParseEnvironment_ReturnsProduction()
        {
            Environment environment = Environment.ParseEnvironment("production");
            Assert.AreEqual(environment, Environment.PRODUCTION);
        }

        [Test]
        [Category("Unit")]
        public void ParseEnvironment_ThrowsException()
        {
            Exception exception = null;

            try {
                Environment.ParseEnvironment("QA_2");
            } catch (Exception localException) {
                exception = localException;
            }

            Assert.IsNotNull(exception);
            Assert.IsInstanceOf<ConfigurationException>(exception);
            Assert.AreEqual("Unsupported environment: QA_2", exception.Message);
        }
    }
}
