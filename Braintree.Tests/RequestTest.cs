using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Braintree;

namespace Braintree.Tests
{
    [TestFixture]
    public class RequestTest
    {
        [Test]
        public void ToXML_EscapesGeneratedXMLForString()
        {
            TransactionRequest request = new TransactionRequest
            {
                OrderId = "<>&\"'"
            };

            TestHelper.AssertIncludes("<order-id>&lt;&gt;&amp;&quot;&apos;</order-id>", request.ToXml());
        }

        [Test]
        public void ToXML_EscapesGeneratedXMLForNestedSearchCriteria()
        {
            TransactionRequest request = new TransactionRequest
            {
                Customer = new CustomerRequest
                {
                    FirstName = "<John>"
                }
            };

            TestHelper.AssertIncludes("<first-name>&lt;John&gt;</first-name>", request.ToXml());
        }

        [Test]
        public void ToXML_EscapesGeneratedXMLForArrayElements()
        {
            SubscriptionSearchRequest request = new SubscriptionSearchRequest().
                Status().IncludedIn("<active");

            TestHelper.AssertIncludes("<status type=\"array\"><item>&lt;active</item></status>", request.ToXml());
        }

        [Test]
        public void ToXML_EscapesGeneratedXMLForSearchCriteria()
        {
            SubscriptionSearchRequest request = new SubscriptionSearchRequest().
                PlanId().Is("<my-id");

            TestHelper.AssertIncludes("<plan-id><is>&lt;my-id</is></plan-id>", request.ToXml());
        }

        [Test]
        public void ToXML_EscapesGeneratedXMLForCustomFields()
        {
            TransactionRequest request = new TransactionRequest
            {
                CustomFields = new Dictionary<String, String>
                {
                    { "<key>", "val&ue" }
                }
            };

            TestHelper.AssertIncludes("<custom-fields><&lt;key&gt;>val&amp;ue</&lt;key&gt;></custom-fields>", request.ToXml());
        }

    }
}
