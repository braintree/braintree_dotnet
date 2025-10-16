using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace Braintree.Tests
{
    [TestFixture]
    public class BankAccountInstantVerificationJwtRequestTest
    {
        [Test]
        public void ToGraphQLVariables_IncludesAllFields()
        {
            var request = new BankAccountInstantVerificationJwtRequest
            {
                BusinessName = "Test Business",
                ReturnUrl = "https://example.com/success",
                CancelUrl = "https://example.com/cancel",
            };

            var variables = request.ToGraphQLVariables();

            Assert.IsNotNull(variables);
            Assert.IsTrue(variables.ContainsKey("input"));
            
            var input = (Dictionary<string, object>)variables["input"];
            
            Assert.AreEqual("Test Business", input["businessName"]);
            Assert.AreEqual("https://example.com/success", input["returnUrl"]);
            Assert.AreEqual("https://example.com/cancel", input["cancelUrl"]);
        }

        [Test]
        public void ToGraphQLVariables_OnlyIncludesNonNullFields()
        {
            var request = new BankAccountInstantVerificationJwtRequest
            {
                BusinessName = "Test Business",
                ReturnUrl = "https://example.com/success"
            };

            var variables = request.ToGraphQLVariables();

            var input = (Dictionary<string, object>)variables["input"];
            
            Assert.AreEqual("Test Business", input["businessName"]);
            Assert.AreEqual("https://example.com/success", input["returnUrl"]);
            Assert.IsFalse(input.ContainsKey("cancelUrl"));
        }

        [Test]
        public void ToXml_IncludesAllFields()
        {
            var request = new BankAccountInstantVerificationJwtRequest
            {
                BusinessName = "Test Business",
                ReturnUrl = "https://example.com/success",
                CancelUrl = "https://example.com/cancel",
            };

            var xml = request.ToXml();

            Assert.IsTrue(xml.Contains("<businessName>Test Business</businessName>"));
            Assert.IsTrue(xml.Contains("<returnUrl>https://example.com/success</returnUrl>"));
            Assert.IsTrue(xml.Contains("<cancelUrl>https://example.com/cancel</cancelUrl>"));
        }

        [Test]
        public void ToXml_EscapesSpecialCharacters()
        {
            var request = new BankAccountInstantVerificationJwtRequest
            {
                BusinessName = "Test Business & Co., Inc. <Special>"
            };

            var xml = request.ToXml();

            Assert.IsTrue(xml.Contains("<businessName>Test Business &amp; Co., Inc. &lt;Special&gt;</businessName>"));
        }

        [Test]
        public void ToQueryString_IncludesAllFields()
        {
            var request = new BankAccountInstantVerificationJwtRequest
            {
                BusinessName = "Test Business",
                ReturnUrl = "https://example.com/success",
                CancelUrl = "https://example.com/cancel",
            };

            var queryString = request.ToQueryString();

            Assert.IsTrue(queryString.Contains("business_name=Test+Business"));
            Assert.IsTrue(queryString.Contains("return_url=https%3A%2F%2Fexample.com%2Fsuccess"));
            Assert.IsTrue(queryString.Contains("cancel_url=https%3A%2F%2Fexample.com%2Fcancel"));
        }
    }
}