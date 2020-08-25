using NUnit.Framework;

namespace Braintree.Tests
{
    public enum TestEnum
    {
        [System.ComponentModel.Description("first")] FIRST,
        [System.ComponentModel.Description("second")] SECOND,
        UNRECOGNIZED
    }

    [TestFixture]
    public class EnumHelperTest
    {
        [Test]
        public void FindEnum_ReturnsEnumValue()
        {
            TestEnum value = EnumHelper.FindEnum("second", TestEnum.UNRECOGNIZED);
            Assert.AreEqual(TestEnum.SECOND, value);
        }

        [Test]
        public void FindEnum_WhenDescriptionNotFound_ReturnsDefaultValue()
        {
            TestEnum value = EnumHelper.FindEnum("invalid", TestEnum.UNRECOGNIZED);
            Assert.AreEqual(TestEnum.UNRECOGNIZED, value);
        }

        [Test]
        public void NullableFindEnum_ReturnsEnumValue()
        {
            TestEnum? value = EnumHelper.FindEnum<TestEnum>("second");
            Assert.AreEqual(TestEnum.SECOND, value);
        }

        [Test]
        public void NullableFindEnum_WhenDescriptionNotFound_ReturnsDefaultValue()
        {
            TestEnum? value = EnumHelper.FindEnum("invalid", TestEnum.UNRECOGNIZED);
            Assert.AreEqual(TestEnum.UNRECOGNIZED, value);
        }

        [Test]
        public void NullableFindEnum_WhenDescriptionNotFound_AndDefaultIsOmitted_ReturnsNull()
        {
            TestEnum? value = EnumHelper.FindEnum<TestEnum>("invalid");
            Assert.Null(value);
        }

        [Test]
        public void GetDescription_ReturnsEnumDescription()
        {
            Assert.AreEqual(TestEnum.FIRST.GetDescription(), "first");
        }

        [Test]
        public void GetDescription_WhenDescriptionDoesNotExist_ReturnsNull()
        {
            Assert.Null(TestEnum.UNRECOGNIZED.GetDescription());
        }

        [Test]
        public void GetDescription_WhenEnumValueIsNull_ReturnsNull()
        {
            TestEnum? value = null;
            Assert.Null(value.GetDescription());
        }
    }
}
