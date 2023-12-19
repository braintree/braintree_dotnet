using NUnit.Framework;

namespace Braintree.Tests
{
    [TestFixture]
    public class CreditCardVerificationRequestTest
    {
        private BraintreeGateway gateway;

        [SetUp]
        public void Setup()
        {
            gateway = new BraintreeGateway
            {
                Environment = Environment.DEVELOPMENT,
                MerchantId = "integration_merchant_id",
                PublicKey = "integration_public_key",
                PrivateKey = "integration_private_key"
            };
        }

        [Test]
        public void ConstructRequest()
        {
            string expected = "<verification>" +
            "<credit-card>" +
                "<expiration-date>05/2029</expiration-date>" +
                "<number>4111111111111111</number>" +
            "</credit-card>"  +
            "<external-vault>" +
                "<status>vaulted</status>" +
                "<previous-network-transaction-id>1234</previous-network-transaction-id>" +
            "</external-vault>" +
            "<intendedTransactionSource>installment</intendedTransactionSource>" +
            "<options>" +
                "<amount>5.00</amount>" +
                "<merchant-account-id>123456</merchant-account-id>" +
            "</options>" +
            "<paymentMethodNonce>payment-method-nonce</paymentMethodNonce>" +
            "<risk-data>" +
                "<customer-browser>IE6</customer-browser>" +
                "<customer-ip>192.168.0.1</customer-ip>" +
            "</risk-data>" +
            "<threeDSecureAuthenticationID>3ds-auth-id</threeDSecureAuthenticationID>" +
            "<threeDSecurePassThru>" +
                "<eci-flag>05</eci-flag>" +
                "<cavv>some_cavv</cavv>" +
                "<xid>some_xid</xid>" +
                "<three-d-secure-version>1.0.2</three-d-secure-version>" +
                "<authentication_response>Y</authentication_response>" +
                "<directory_response>Y</directory_response>" +
                "<cavv_algorithm>2</cavv_algorithm>" +
                "<ds_transaction_id>some_ds_transaction_id</ds_transaction_id>" +
            "</threeDSecurePassThru></verification>";

            CreditCardVerificationRequest verification = new CreditCardVerificationRequest
            {
                CreditCard = new CreditCardVerificationCreditCardRequest
                {
                     Number = SandboxValues.CreditCardNumber.VISA,
                     ExpirationDate = "05/2029",
                     BillingAddress = new CreditCardAddressRequest
                     {
                         CountryName = "Greece",
                         CountryCodeAlpha2 = "GR",
                         CountryCodeAlpha3 = "GRC",
                         CountryCodeNumeric = "300"
                     }
                },
                ExternalVault = new ExternalVaultRequest()
                {
                    PreviousNetworkTransactionId = "1234",
                    Status = "vaulted"
                },
                IntendedTransactionSource = "installment",
                PaymentMethodNonce = "payment-method-nonce",
                Options = new CreditCardVerificationOptionsRequest
                {
                    MerchantAccountId = "123456", 
                    Amount = "5.00"
                },
                RiskData = new RiskDataRequest()
                {
                    CustomerBrowser = "IE6",
                    CustomerIP = "192.168.0.1",
                },
                ThreeDSecureAuthenticationID = "3ds-auth-id",
                ThreeDSecurePassThru = new ThreeDSecurePassThruRequest()
                {
                    EciFlag = "05",
                    Cavv = "some_cavv",
                    Xid = "some_xid",
                    AuthenticationResponse = "Y",
                    DirectoryResponse = "Y",
                    CavvAlgorithm = "2",
                    DsTransactionId = "some_ds_transaction_id",
                    ThreeDSecureVersion = "1.0.2"
                }
            };
            Assert.AreEqual(expected, verification.ToXml());
        }
    }
}
