using System;
using NUnit.Framework;
using Braintree;

namespace Braintree.Tests
{
    [TestFixture]
    public class MultipleValueNodeTest
    {
        [Test]
        public void IncludedIn_RaisesErrorIfProvidedInvalidValue()
        {
            String[] allowedValues = {"good", "better"};
            MultipleValueNode<TransactionSearchRequest> node = new MultipleValueNode<TransactionSearchRequest>("test", new TransactionSearchRequest(), allowedValues);
            try
            {
                node.IncludedIn("bad");
                Assert.Fail();
            }
            catch(ArgumentOutOfRangeException)
            {
                // expect to raise
            }
        }
    }
}
