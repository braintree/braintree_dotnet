using Braintree;
using Braintree.GraphQL;
using Braintree.TestUtil;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;

namespace Braintree.Tests.GraphQL
{
    [TestFixture]
    public class PayPalPurchaseUnitInputTest
    {
        [Test]
        public void TestToGraphQLVariables()
        {
            var payee = PayPalPayeeInput.Builder()
            .EmailAddress("test@example.com")
            .ClientId("merchant-public-id")
            .Build();

            var amount = new MonetaryAmountInput("300.0", "USD");

            var purchaseUnit = PayPalPurchaseUnitInput.Builder(amount)
            .Payee(payee)
            .Build();

            var dict = purchaseUnit.ToGraphQLVariables();

            CollectionAssert.AreEquivalent(
                payee.ToGraphQLVariables(),
                (System.Collections.IEnumerable)dict["payee"]
            );

            CollectionAssert.AreEquivalent(
                amount.ToGraphQLVariables(),
                (System.Collections.IEnumerable)dict["amount"]
            );
        }
    }
}
