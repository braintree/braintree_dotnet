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
        public void ToXML_EnsuresUSLocaleForDecimals()
        {
            System.Globalization.CultureInfo existingCulture = System.Globalization.CultureInfo.CurrentCulture;

            try
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("it-IT");

                TransactionRequest transactionRequest = new TransactionRequest
                {
                    Amount = 100.0M
                };

                SubscriptionRequest subscriptionRequest = new SubscriptionRequest
                {
                    Price = 200.0M,
                };

                SubscriptionTransactionRequest subscriptionTransactionRequest = new SubscriptionTransactionRequest
                {
                    Amount = 300.0M
                };

                ModificationRequest modificationRequest = new ModificationRequest
                {
                    Amount = 400.0M
                };

                TestHelper.AssertIncludes("<amount>100.0</amount>", transactionRequest.ToXml());
                TestHelper.AssertIncludes("<price>200.0</price>", subscriptionRequest.ToXml());
                TestHelper.AssertIncludes("<amount>300.0</amount>", subscriptionTransactionRequest.ToXml());
                TestHelper.AssertIncludes("<amount>400.0</amount>", modificationRequest.ToXml("root"));
            }
            finally
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = existingCulture;
            }
        }

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
                Ids.IncludedIn("<active");

            TestHelper.AssertIncludes("<ids type=\"array\"><item>&lt;active</item></ids>", request.ToXml());
        }

        [Test]
        public void ToXML_EscapesGeneratedXMLForSearchCriteria()
        {
            SubscriptionSearchRequest request = new SubscriptionSearchRequest().
                PlanId.Is("<my-id");

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
