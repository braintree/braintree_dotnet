using Braintree.Exceptions;
using Braintree.TestUtil;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;

namespace Braintree.Tests.Integration
{
    [TestFixture]
    public class ThreeDSecureIntegrationTest
    {

        public BraintreeGateway GetGateway()
        {
            return new BraintreeGateway
            {
                Environment = Environment.DEVELOPMENT,
                MerchantId = "integration_merchant_id",
                PublicKey = "integration_public_key",
                PrivateKey = "integration_private_key"
            };
        }

        public string GetClientDataString(String nonce) {
            return "{" + $@"
                ""authorizationFingerprint"": ""fake-auth-fingerprint"",
                ""braintreeLibraryVersion"": ""braintree/web/3.44.0"",
                ""dfReferenceId"": ""ABC-123"",
                ""nonce"": ""{nonce}"",
                ""clientMetadata"": " + @"{
                    ""cardinalDeviceDataCollectionTimeElapsed"": 40,
                    ""issuerDeviceDataCollectionResult"": true,
                    ""issuerDeviceDataCollectionTimeElapsed"": 413,
                    ""requestedThreeDSecureVersion"": ""2"",
                    ""sdkVersion"": ""web/3.44.0""
                }
            }";
        }

        public string GetClientDataString(BraintreeGateway gateway) {
            var customerRequest = new CustomerRequest
            {
                FirstName = "Foo",
                LastName = "Bar"
            };
            var customer = gateway.Customer.Create(customerRequest).Target;
            var creditCardRequest = new CreditCardRequest
            {
                CustomerId = customer.Id,
                Number = "4000000000001091",
                ExpirationMonth = "12",
                ExpirationYear = "2030"
            };
            CreditCard creditCard = gateway.CreditCard.Create(creditCardRequest).Target;
            var nonce = gateway.PaymentMethodNonce.Create(creditCard.Token).Target.Nonce;

            return GetClientDataString(nonce);
        }

        public void SetDeviceDataFields(ThreeDSecureLookupRequest request) {
            request.BrowserAcceptHeader = "text/html;q=0.8";
            request.BrowserColorDepth = "48";
            request.BrowserJavaEnabled = false;
            request.BrowserJavascriptEnabled = true;
            request.BrowserLanguage = "fr-CA";
            request.BrowserScreenHeight = "600";
            request.BrowserScreenWidth = "800";
            request.BrowserTimeZone = "-60";
            request.DeviceChannel = "Browser";
            request.IpAddress = "2001:0db8:0000:0000:0000:ff00:0042:8329";
            request.UserAgent = "Mozilla/5.0";
        }

        [Test]
        public void LookupThreeDSecure_IsSuccessful()
        {
            var gateway = GetGateway();

            ThreeDSecureLookupAddress billingAddress = new ThreeDSecureLookupAddress
            {
                GivenName = "First",
                Surname = "Last",
                PhoneNumber = "1234567890",
                Locality = "Oakland",
                CountryCodeAlpha2 = "US",
                StreetAddress = "123 Address",
                ExtendedAddress = "Unit 2",
                PostalCode = "94112",
                Region = "CA"
            };

            var clientData = GetClientDataString(gateway);

            ThreeDSecureLookupRequest request = new ThreeDSecureLookupRequest{
                Amount = "199.00",
                ClientData = clientData,
                Email = "first.last@example.com",
                BillingAddress = billingAddress
            };

            SetDeviceDataFields(request);

            ThreeDSecureLookupResponse result = gateway.ThreeDSecure.Lookup(request);

            PaymentMethodNonce paymentMethod = result.PaymentMethod;
            ThreeDSecureLookup lookup = result.Lookup;

            Assert.IsNull(result.Error);
            Assert.IsNotNull(result.PayloadString);
            Assert.IsNotNull(paymentMethod.Nonce);
            Assert.IsNotNull(paymentMethod.ThreeDSecureInfo);
            Assert.IsTrue(paymentMethod.ThreeDSecureInfo.LiabilityShiftPossible);
            Assert.IsFalse(paymentMethod.ThreeDSecureInfo.LiabilityShifted);
            Assert.IsNotNull(lookup.AcsUrl);
            Assert.IsNotNull(lookup.ThreeDSecureVersion);
            Assert.IsNotNull(lookup.TransactionId);
        }

        [Test]
        public void LookupThreeDSecure_IsSuccesfulWithPartialCustomerInfo()
        {
            var gateway = GetGateway();
            var clientData = GetClientDataString(gateway);

            ThreeDSecureLookupRequest request = new ThreeDSecureLookupRequest{
                Amount = "199.00",
                ClientData = clientData,
                Email = "first.last@example.com",
            };

            SetDeviceDataFields(request);

            ThreeDSecureLookupResponse result = null;

            result = gateway.ThreeDSecure.Lookup(request);

            PaymentMethodNonce paymentMethod = result.PaymentMethod;
            ThreeDSecureLookup lookup = result.Lookup;

            Assert.IsNull(result.Error);
            Assert.IsNotNull(result.PayloadString);
            Assert.IsNotNull(paymentMethod.Nonce);
            Assert.IsNotNull(paymentMethod.ThreeDSecureInfo);
            Assert.IsTrue(paymentMethod.ThreeDSecureInfo.LiabilityShiftPossible);
            Assert.IsFalse(paymentMethod.ThreeDSecureInfo.LiabilityShifted);
            Assert.IsNotNull(lookup.AcsUrl);
            Assert.IsNotNull(lookup.ThreeDSecureVersion);
            Assert.IsNotNull(lookup.TransactionId);
        }

        [Test]
        public void LookupThreeDSecure_IsSuccesfulWithNoCustomerInfo()
        {
            var gateway = GetGateway();
            var clientData = GetClientDataString(gateway);

            ThreeDSecureLookupRequest request = new ThreeDSecureLookupRequest{
                Amount = "199.00",
                ClientData = clientData,
            };

            SetDeviceDataFields(request);

            ThreeDSecureLookupResponse result = gateway.ThreeDSecure.Lookup(request);

            PaymentMethodNonce paymentMethod = result.PaymentMethod;
            ThreeDSecureLookup lookup = result.Lookup;

            Assert.IsNull(result.Error);
            Assert.IsNotNull(result.PayloadString);
            Assert.IsNotNull(paymentMethod.Nonce);
            Assert.IsNotNull(paymentMethod.ThreeDSecureInfo);
            Assert.IsTrue(paymentMethod.ThreeDSecureInfo.LiabilityShiftPossible);
            Assert.IsFalse(paymentMethod.ThreeDSecureInfo.LiabilityShifted);
            Assert.IsNotNull(lookup.AcsUrl);
            Assert.IsNotNull(lookup.ThreeDSecureVersion);
            Assert.IsNotNull(lookup.TransactionId);
        }

        [Test]
        public void LookupThreeDSecure_ThowsBraintreeExceptionForBadNonce()
        {
            var gateway = GetGateway();

            ThreeDSecureLookupAddress billingAddress = new ThreeDSecureLookupAddress
            {
                GivenName = "First",
                Surname = "Last",
                PhoneNumber = "1234567890",
                Locality = "Oakland",
                CountryCodeAlpha2 = "US",
                StreetAddress = "123 Address",
                ExtendedAddress = "Unit 2",
                PostalCode = "94112",
                Region = "CA"
            };

            var clientData = GetClientDataString("bad-nonce");

            ThreeDSecureLookupRequest request = new ThreeDSecureLookupRequest{
                Amount = "199.00",
                ClientData = clientData,
                Email = "first.last@example.com",
                BillingAddress = billingAddress
            };

            Assert.Throws<Braintree.Exceptions.NotFoundException>(() => gateway.ThreeDSecure.Lookup(request));
        }

        [Test]
        public void LookupThreeDSecure_HasValidationError()
        {
            var gateway = GetGateway();

            ThreeDSecureLookupAddress billingAddress = new ThreeDSecureLookupAddress
            {
                GivenName = "Løst",
                Surname = "Lést",
                PhoneNumber = "1234567890",
                Locality = "Oakland",
                CountryCodeAlpha2 = "US",
                StreetAddress = "123 Address",
                ExtendedAddress = "Unit 2",
                PostalCode = "94112222222",
                Region = "CA"
            };

            var clientData = GetClientDataString(gateway);

            ThreeDSecureLookupRequest request = new ThreeDSecureLookupRequest{
                Amount = "199.00",
                ClientData = clientData,
                Email = "first.last@example.com",
                BillingAddress = billingAddress
            };

            ThreeDSecureLookupResponse result = gateway.ThreeDSecure.Lookup(request);

            PaymentMethodNonce paymentMethod = result.PaymentMethod;
            ThreeDSecureLookup lookup = result.Lookup;

            Assert.IsNotNull(result.PayloadString);
            Assert.IsNull(paymentMethod);
            Assert.IsNull(lookup);

            Assert.IsNotNull(result.Error);
            StringAssert.Contains("format is invalid", (string)result.Error.message);
        }
    }
}
