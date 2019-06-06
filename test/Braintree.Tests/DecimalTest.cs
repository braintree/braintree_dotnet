using NUnit.Framework;

namespace Braintree.Tests
{
    [TestFixture]
    public class DecimalTest
    {
        [Test]
        public void Assignment_WorksForStrings()
        {
            Decimal value = "100";
            Assert.AreEqual(new Decimal("100"), value);
        }

        [Test]
        public void Assignment_WorksForPrimitiveDecimalTypes()
        {
            Decimal value = 100.0M;
            Assert.AreEqual(new Decimal("100.0"), value);
        }

        [Test]
        public void Assignment_DoesWorkForIncorrectlyFormattedStrings()
        {
            Assert.DoesNotThrow(() =>
            {
                Decimal value = "invalid";
            });
        }
    }
}

