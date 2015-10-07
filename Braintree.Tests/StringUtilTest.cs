using System;
using NUnit.Framework;

namespace Braintree.Tests
{
    //NOTE: good
    [TestFixture]
    public class StringUtilTest
    {
        [Test]
        public void Dasherize_ReturnsNullForNullString()
        {
            Assert.IsNull(StringUtil.Dasherize(null));
        }

        [Test]
        public void Dasherize_WorksForDasherizedString()
        {
            Assert.AreEqual("foo-bar", StringUtil.Dasherize("foo-bar"));
        }

        [Test]
        public void Dasherize_WorksForCamelCaseString()
        {
            Assert.AreEqual("first-name", StringUtil.Dasherize("firstName"));
        }

        [Test]
        public void Dasherize_WorksForCamelCaseStringWithLeadingUppercaseLetter()
        {
            Assert.AreEqual("first-name", StringUtil.Dasherize("FirstName"));
        }

        [Test]
        public void Dasherize_WorksForUnderscoreString()
        {
            Assert.AreEqual("first-name", StringUtil.Dasherize("first_name"));
        }

        [Test]
        public void Dasherize_WorksForCamelCaseStringWithContiguousUppercaseLetters()
        {
            Assert.AreEqual("headline-cnn-news", StringUtil.Dasherize("HeadlineCNNNews"));
        }

        [Test]
        public void Underscore_ReturnsNullForNullString()
        {
            Assert.IsNull(StringUtil.Underscore(null));
        }

        [Test]
        public void Underscore_WorksForDasherizedString()
        {
            Assert.AreEqual("foo_bar", StringUtil.Underscore("foo-bar"));
        }

        [Test]
        public void Underscore_WorksForCamelCaseString()
        {
            Assert.AreEqual("first_name", StringUtil.Underscore("firstName"));
        }

        [Test]
        public void Underscore_WorksForCamelCaseStringWithLeadingUppercaseLetter()
        {
            Assert.AreEqual("first_name", StringUtil.Underscore("FirstName"));
        }

        [Test]
        public void Underscore_WorksForCamelCaseStringWithContiguousUppercaseLetters()
        {
            Assert.AreEqual("headline_cnn_news", StringUtil.Underscore("HeadlineCNNNews"));
        }

        [Test]
        public void Underscore_WorksForUnderscoreString()
        {
            Assert.AreEqual("first_name", StringUtil.Underscore("first_name"));
        }
    }
}

