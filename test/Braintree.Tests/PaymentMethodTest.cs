using Braintree.Exceptions;
using NUnit.Framework;

namespace Braintree.Tests
{
    [TestFixture]
    public class PaymentMethodTest
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
        public void ToXml_IncludesDeviceData()
        {
            var request = new PaymentMethodRequest()
            {
                DeviceData = "{\"device_session_id\":\"my_dsid\", \"fraud_merchant_id\":\"my_fmid\"}"
            };

            Assert.IsTrue(request.ToXml().Contains("device_session_id"));
            Assert.IsTrue(request.ToXml().Contains("my_dsid"));
            Assert.IsTrue(request.ToXml().Contains("fraud_merchant_id"));
            Assert.IsTrue(request.ToXml().Contains("my_fmid"));
        }


        [Test]
        public void Find_RaisesNotFoundErrorWhenTokenIsBlank()
        {
            Assert.Throws<NotFoundException>(() => gateway.PaymentMethod.Find(" "));
        }

        [Test]
        public void PaymentMethod_Delete_ToQueryString_IncludesRevokeAllGrants()
        {
            var request = new PaymentMethodDeleteRequest { RevokeAllGrants = true};
            Assert.IsTrue(request.ToQueryString().Contains("revoke_all_grants=true"));
        }

        [Test]
        public void PaymentMethod_Create_ToXml_IncludesAllElements()
        {
            PaymentMethodRequest request = new PaymentMethodRequest
            {
                BillingAddressId = "some-billing-address-id",
                CardholderName = "some-name",
                CustomerId = "some-customer-id",
                CVV = "123",
                DeviceData = "some-device-data",
                ExpirationDate = "05/12",
                ExpirationMonth = "05",
                ExpirationYear = "2012",
                Number = "4111111111111111",
                Options = new PaymentMethodOptionsRequest {
                    VerifyCard = true
                },
                PaymentMethodNonce = "some-payment-method-nonce",
                PayPalRefreshToken = "some-paypal-refresh-token",
                ThreeDSecureAuthenticationId = "some-authentication-id",
                Token = "some-token"
            };

            string xml = request.ToXml();

            Assert.IsTrue(xml.Contains("<billing-address-id>some-billing-address-id</billing-address-id>"));
            Assert.IsTrue(xml.Contains("<cardholder-name>some-name</cardholder-name>"));
            Assert.IsTrue(xml.Contains("<customer-id>some-customer-id</customer-id>"));
            Assert.IsTrue(xml.Contains("<cvv>123</cvv>"));
            Assert.IsTrue(xml.Contains("<device-data>some-device-data</device-data>"));
            Assert.IsTrue(xml.Contains("<expiration-date>05/12</expiration-date>"));
            Assert.IsTrue(xml.Contains("<expiration-month>05</expiration-month>"));
            Assert.IsTrue(xml.Contains("<expiration-year>2012</expiration-year>"));
            Assert.IsTrue(xml.Contains("<number>4111111111111111</number>"));
            Assert.IsTrue(xml.Contains("<options><verify-card>true</verify-card></options>"));
            Assert.IsTrue(xml.Contains("<payment-method-nonce>some-payment-method-nonce</payment-method-nonce>"));
            Assert.IsTrue(xml.Contains("<paypal-refresh-token>some-paypal-refresh-token</paypal-refresh-token>"));
            Assert.IsTrue(xml.Contains("<token>some-token</token>"));
        }

        [Test]
        public void PaymentMethod_Create_ToXml_PassThruIncludesAllElements()
        {
            PaymentMethodRequest request = new PaymentMethodRequest 
            {
                CustomerId = "some-customer-id",
                Options = new PaymentMethodOptionsRequest {
                    VerifyCard = true
                },
                PaymentMethodNonce = "some-payment-method-nonce",
                ThreeDSecurePassThru = new ThreeDSecurePassThruRequest {
                    AuthenticationResponse = "some-auth-response",
                    Cavv = "some-cavv",
                    CavvAlgorithm = "algorithm",
                    DirectoryResponse = "some-directory-response",
                    DsTransactionId = "some-ds-transaction-id",
                    EciFlag = "05",
                    ThreeDSecureVersion = "2.2.0",
                    Xid = "some-xid"
                }
            };

            string xml = request.ToXml();

            Assert.IsTrue(xml.Contains("<customer-id>some-customer-id</customer-id>"));
            Assert.IsTrue(xml.Contains("<options><verify-card>true</verify-card></options>"));
            Assert.IsTrue(xml.Contains("<payment-method-nonce>some-payment-method-nonce</payment-method-nonce>"));
            Assert.IsTrue(xml.Contains("<three-d-secure-pass-thru>"));
            Assert.IsTrue(xml.Contains("</three-d-secure-pass-thru>"));
            Assert.IsTrue(xml.Contains("<authentication_response>some-auth-response</authentication_response>"));
            Assert.IsTrue(xml.Contains("<cavv>some-cavv</cavv>"));
            Assert.IsTrue(xml.Contains("<cavv_algorithm>algorithm</cavv_algorithm>"));
            Assert.IsTrue(xml.Contains("<directory_response>some-directory-response</directory_response>"));
            Assert.IsTrue(xml.Contains("<ds_transaction_id>some-ds-transaction-id</ds_transaction_id>"));
            Assert.IsTrue(xml.Contains("<eci-flag>05</eci-flag>"));
            Assert.IsTrue(xml.Contains("<three-d-secure-version>2.2.0</three-d-secure-version>"));
            Assert.IsTrue(xml.Contains("<xid>some-xid</xid>"));
        }
    }
}
