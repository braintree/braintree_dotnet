using NUnit.Framework;

namespace Braintree.Tests
{
    [TestFixture]
    public class ApplePayCardOptionsRequestTest
    {
        [Test]
        public void ToXml_IncludesAllProperties()
        {
            var request = new ApplePayCardOptionsRequest()
            {
                MakeDefault = false,
                VerifyCard = true,
                VerificationAccountType = "credit",
                VerificationAmount = "1.23",
                VerificationMerchantAccountId = "merchant_account_id"
            };

            Assert.IsTrue(request.ToXml("options").Contains("<make-default>false</make-default>"));
            Assert.IsTrue(request.ToXml("options").Contains("<verify-card>true</verify-card>"));
            Assert.IsTrue(request.ToXml("options").Contains("<verification-account-type>credit</verification-account-type>"));
            Assert.IsTrue(request.ToXml("options").Contains("<verification-amount>1.23</verification-amount>"));
            Assert.IsTrue(request.ToXml("options").Contains("<verification-merchant-account-id>merchant_account_id</verification-merchant-account-id>"));
        }
    }
}