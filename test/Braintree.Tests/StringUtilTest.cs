using System;
using NUnit.Framework;

namespace Braintree.Tests
{
    [TestFixture]
    public class StringUtilTest
    {
        [Test]
        [Category("Unit")]
        public void Dasherize_ReturnsNullForNullString()
        {
            Assert.IsNull(StringUtil.Dasherize(null));
        }

        [Test]
        [Category("Unit")]
        public void Dasherize_WorksForDasherizedString()
        {
            Assert.AreEqual("foo-bar", StringUtil.Dasherize("foo-bar"));
        }

        [Test]
        [Category("Unit")]
        public void Dasherize_WorksForCamelCaseString()
        {
            Assert.AreEqual("first-name", StringUtil.Dasherize("firstName"));
        }

        [Test]
        [Category("Unit")]
        public void Dasherize_WorksForCamelCaseStringWithLeadingUppercaseLetter()
        {
            Assert.AreEqual("first-name", StringUtil.Dasherize("FirstName"));
        }

        [Test]
        [Category("Unit")]
        public void Dasherize_WorksForUnderscoreString()
        {
            Assert.AreEqual("first-name", StringUtil.Dasherize("first_name"));
        }

        [Test]
        [Category("Unit")]
        public void Dasherize_WorksForCamelCaseStringWithContiguousUppercaseLetters()
        {
            Assert.AreEqual("headline-cnn-news", StringUtil.Dasherize("HeadlineCNNNews"));
        }

        [Test]
        [Category("Unit")]
        public void Underscore_ReturnsNullForNullString()
        {
            Assert.IsNull(StringUtil.Underscore(null));
        }

        [Test]
        [Category("Unit")]
        public void Underscore_WorksForDasherizedString()
        {
            Assert.AreEqual("foo_bar", StringUtil.Underscore("foo-bar"));
        }

        [Test]
        [Category("Unit")]
        public void Underscore_WorksForCamelCaseString()
        {
            Assert.AreEqual("first_name", StringUtil.Underscore("firstName"));
        }

        [Test]
        [Category("Unit")]
        public void Underscore_WorksForCamelCaseStringWithLeadingUppercaseLetter()
        {
            Assert.AreEqual("first_name", StringUtil.Underscore("FirstName"));
        }

        [Test]
        [Category("Unit")]
        public void Underscore_WorksForCamelCaseStringWithContiguousUppercaseLetters()
        {
            Assert.AreEqual("headline_cnn_news", StringUtil.Underscore("HeadlineCNNNews"));
        }

        [Test]
        [Category("Unit")]
        public void Underscore_WorksForUnderscoreString()
        {
            Assert.AreEqual("first_name", StringUtil.Underscore("first_name"));
        }
    }
}

