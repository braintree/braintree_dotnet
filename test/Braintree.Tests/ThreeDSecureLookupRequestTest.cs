using Braintree.Exceptions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Braintree.Tests
{
    [TestFixture]
    public class ThreeDSecureLookupRequestTest
    {

        [Test]
        public void SerializesWithAmountAndClientData()
        {
            var clientData = @"{
                ""authorizationFingerprint"": ""auth-fingerprint"",
                ""braintreeLibraryVersion"": ""braintree/web/3.44.0"",
                ""dfReferenceId"": ""ABC-123"",
                ""nonce"": ""FAKE-NONCE"",
                ""clientMetadata"": {
                     ""cardinalDeviceDataCollectionTimeElapsed"": 40,
                     ""issuerDeviceDataCollectionResult"": true,
                     ""issuerDeviceDataCollectionTimeElapsed"": 413,
                     ""requestedThreeDSecureVersion"": ""2"",
                     ""sdkVersion"": ""web/3.42.0""
                }
            }";

            ThreeDSecureLookupRequest request = new ThreeDSecureLookupRequest
            {
                Amount = "10.00",
                ClientData = clientData
            };

            Assert.AreEqual("10.00", request.Amount);

            var outputJSON = request.ToJSON();

            StringAssert.Contains(@"""amount"":""" + request.Amount, outputJSON);
            StringAssert.Contains(@"""df_reference_id"":""ABC-123""", outputJSON);
            StringAssert.Contains(@"""authorizationFingerprint"":""auth-fingerprint""", outputJSON);
            StringAssert.Contains(@"""braintreeLibraryVersion"":""braintree/web/3.44.0""", outputJSON);
            StringAssert.DoesNotMatch(@"cardAddChallengeRequested", outputJSON);
            StringAssert.DoesNotMatch(@"challengeRequested", outputJSON);
            StringAssert.DoesNotMatch(@"exemptionRequested", outputJSON);
            StringAssert.DoesNotMatch(@"requestedExemptionType", outputJSON);
            StringAssert.DoesNotMatch(@"merchantInitiatedRequestType", outputJSON);
            StringAssert.DoesNotMatch(@"priorAuthenticationId", outputJSON);
            StringAssert.DoesNotMatch(@"browserJavaEnabled", outputJSON);
            StringAssert.DoesNotMatch(@"browserJavascriptEnabled", outputJSON);
        }

        [Test]
        public void SerializesAdditionalInformation()
        {
            var clientData = @"{
                ""authorizationFingerprint"": ""auth-fingerprint"",
                ""braintreeLibraryVersion"": ""braintree/web/3.44.0"",
                ""dfReferenceId"": ""ABC-123"",
                ""nonce"": ""FAKE-NONCE"",
                ""clientMetadata"": {
                     ""cardinalDeviceDataCollectionTimeElapsed"": 40,
                     ""issuerDeviceDataCollectionResult"": true,
                     ""issuerDeviceDataCollectionTimeElapsed"": 413,
                     ""requestedThreeDSecureVersion"": ""2"",
                     ""sdkVersion"": ""web/3.42.0""
                }
            }";

            ThreeDSecureLookupAddress billingAddress = new ThreeDSecureLookupAddress {
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

            ThreeDSecureLookupAddress shippingAddress = new ThreeDSecureLookupAddress {
                GivenName = "First",
                Surname = "Last",
                PhoneNumber = "0987654321",
                Locality = "Beverly Hills",
                CountryCodeAlpha2 = "US",
                StreetAddress = "123 Fake",
                ExtendedAddress = "Unit 3",
                PostalCode = "90210",
                Region = "CA"
            };

            ThreeDSecureLookupAdditionalInformation additionalInfo = new ThreeDSecureLookupAdditionalInformation {
                ShippingAddress = shippingAddress,
                ProductCode = "1",
                DeliveryTimeframe = "1",
                DeliveryEmail = "last.first@example.com",
                ReorderIndicator = "Y",
                PreorderIndicator = "Y",
                PreorderDate = "11/5/1955",
                GiftCardAmount = "10.00",
                GiftCardCurrencyCode = "USD",
                GiftCardCount = "1",
                AccountAgeIndicator = "1",
                AccountCreateDate = "11/5/1955",
                AccountChangeIndicator = "Y",
                AccountChangeDate = "11/5/1955",
                AccountPwdChangeIndicator = "Y",
                AccountPwdChangeDate = "11/5/1955",
                ShippingAddressUsageIndicator = "Y",
                ShippingAddressUsageDate = "11/5/1955",
                TransactionCountDay = "1",
                TransactionCountYear = "1",
                AddCardAttempts = "1",
                AccountPurchases = "1",
                FraudActivity = "Y",
                ShippingNameIndicator = "Y",
                PaymentAccountIndicator = "Y",
                PaymentAccountAge = "1",
                AddressMatch = "Y",
                AccountId = "1",
                IpAddress = "127.0.0.1",
                OrderDescription = "Fake Description",
                TaxAmount = "1",
                UserAgent = "Interwebz",
                AuthenticationIndicator = "Y",
                Installment = "1",
                PurchaseDate = "11/5/1955",
                RecurringEnd = "11/12/1955",
                RecurringFrequency = "1"
            };

            ThreeDSecureLookupRequest request = new ThreeDSecureLookupRequest {
                Amount = "10.00",
                ClientData = clientData,
                AdditionalInformation = additionalInfo,
                Email = "first.last@example.com",
                BillingAddress = billingAddress
            };

            var outputJSON = request.ToJSON();

            StringAssert.Contains(@"""amount"":""10.00""", outputJSON);
            StringAssert.Contains(@"""email"":""first.last@example.com""", outputJSON);

            StringAssert.Contains(@"""billingGivenName"":""First""", outputJSON);
            StringAssert.Contains(@"""billingSurname"":""Last""", outputJSON);
            StringAssert.Contains(@"""billingLine1"":""123 Address""", outputJSON);
            StringAssert.Contains(@"""billingLine2"":""Unit 2""", outputJSON);
            StringAssert.Contains(@"""billingCity"":""Oakland""", outputJSON);
            StringAssert.Contains(@"""billingState"":""CA""", outputJSON);
            StringAssert.Contains(@"""billingPostalCode"":""94112""", outputJSON);
            StringAssert.Contains(@"""billingCountryCode"":""US""", outputJSON);
            StringAssert.Contains(@"""billingPhoneNumber"":""1234567890""", outputJSON);

            StringAssert.Contains(@"""shipping_given_name"":""First""", outputJSON);
            StringAssert.Contains(@"""shipping_surname"":""Last""", outputJSON);
            StringAssert.Contains(@"""shipping_phone"":""0987654321""", outputJSON);
            StringAssert.Contains(@"""shipping_line1"":""123 Fake""", outputJSON);
            StringAssert.Contains(@"""shipping_line2"":""Unit 3""", outputJSON);
            StringAssert.Contains(@"""shipping_city"":""Beverly Hills""", outputJSON);
            StringAssert.Contains(@"""shipping_state"":""CA""", outputJSON);
            StringAssert.Contains(@"""shipping_postal_code"":""90210""", outputJSON);
            StringAssert.Contains(@"""shipping_country_code"":""US""", outputJSON);

            StringAssert.Contains(@"""product_code"":""1""", outputJSON, outputJSON);
            StringAssert.Contains(@"""delivery_timeframe"":""1""", outputJSON);
            StringAssert.Contains(@"""delivery_email"":""last.first@example.com""", outputJSON);
            StringAssert.Contains(@"""reorder_indicator"":""Y""", outputJSON);
            StringAssert.Contains(@"""preorder_indicator"":""Y""", outputJSON);
            StringAssert.Contains(@"""preorder_date"":""11/5/1955""", outputJSON);
            StringAssert.Contains(@"""gift_card_amount"":""10.00""", outputJSON);
            StringAssert.Contains(@"""gift_card_currency_code"":""USD""", outputJSON);
            StringAssert.Contains(@"""gift_card_count"":""1""", outputJSON);
            StringAssert.Contains(@"""account_age_indicator"":""1""", outputJSON);
            StringAssert.Contains(@"""account_create_date"":""11/5/1955""", outputJSON);
            StringAssert.Contains(@"""account_change_indicator"":""Y""", outputJSON);
            StringAssert.Contains(@"""account_change_date"":""11/5/1955""", outputJSON);
            StringAssert.Contains(@"""account_pwd_change_indicator"":""Y""", outputJSON);
            StringAssert.Contains(@"""account_pwd_change_date"":""11/5/1955""", outputJSON);
            StringAssert.Contains(@"""shipping_address_usage_indicator"":""Y""", outputJSON);
            StringAssert.Contains(@"""shipping_address_usage_date"":""11/5/1955""", outputJSON);
            StringAssert.Contains(@"""transaction_count_day"":""1""", outputJSON);
            StringAssert.Contains(@"""transaction_count_year"":""1""", outputJSON);
            StringAssert.Contains(@"""add_card_attempts"":""1""", outputJSON);
            StringAssert.Contains(@"""account_purchases"":""1""", outputJSON);
            StringAssert.Contains(@"""fraud_activity"":""Y""", outputJSON);
            StringAssert.Contains(@"""shipping_name_indicator"":""Y""", outputJSON);
            StringAssert.Contains(@"""payment_account_indicator"":""Y""", outputJSON);
            StringAssert.Contains(@"""payment_account_age"":""1""", outputJSON);
            StringAssert.Contains(@"""address_match"":""Y""", outputJSON);
            StringAssert.Contains(@"""account_id"":""1""", outputJSON);
            StringAssert.Contains(@"""ip_address"":""127.0.0.1""", outputJSON);
            StringAssert.Contains(@"""order_description"":""Fake Description""", outputJSON);
            StringAssert.Contains(@"""tax_amount"":""1""", outputJSON);
            StringAssert.Contains(@"""user_agent"":""Interwebz""", outputJSON);
            StringAssert.Contains(@"""authentication_indicator"":""Y""", outputJSON);
            StringAssert.Contains(@"""installment"":""1""", outputJSON);
            StringAssert.Contains(@"""purchase_date"":""11/5/1955""", outputJSON);
            StringAssert.Contains(@"""recurring_end"":""11/12/1955""", outputJSON);
            StringAssert.Contains(@"""recurring_frequency"":""1""", outputJSON);
        }

        [Test]
        public void SerializesWithCardAddChallengeRequestedTrue()
        {
            var clientData = @"{
                ""authorizationFingerprint"": ""auth-fingerprint"",
                ""braintreeLibraryVersion"": ""braintree/web/3.44.0"",
                ""dfReferenceId"": ""ABC-123"",
                ""nonce"": ""FAKE-NONCE"",
                ""clientMetadata"": {
                     ""cardinalDeviceDataCollectionTimeElapsed"": 40,
                     ""issuerDeviceDataCollectionResult"": true,
                     ""issuerDeviceDataCollectionTimeElapsed"": 413,
                     ""requestedThreeDSecureVersion"": ""2"",
                     ""sdkVersion"": ""web/3.42.0""
                }
            }";

            ThreeDSecureLookupRequest request = new ThreeDSecureLookupRequest
            {
                Amount = "10.00",
                ClientData = clientData,
                CardAddChallengeRequested = true
            };
        
            Assert.AreEqual(true, request.CardAddChallengeRequested);

            var outputJSON = request.ToJSON();
            
            StringAssert.Contains(@"""cardAddChallengeRequested"":true", outputJSON);
        }

        [Test]
        public void SerializesWithChallengeRequestedTrue()
        {
            var clientData = @"{
                ""authorizationFingerprint"": ""auth-fingerprint"",
                ""braintreeLibraryVersion"": ""braintree/web/3.44.0"",
                ""dfReferenceId"": ""ABC-123"",
                ""nonce"": ""FAKE-NONCE"",
                ""clientMetadata"": {
                     ""cardinalDeviceDataCollectionTimeElapsed"": 40,
                     ""issuerDeviceDataCollectionResult"": true,
                     ""issuerDeviceDataCollectionTimeElapsed"": 413,
                     ""requestedThreeDSecureVersion"": ""2"",
                     ""sdkVersion"": ""web/3.42.0""
                }
            }";

            ThreeDSecureLookupRequest request = new ThreeDSecureLookupRequest
            {
                Amount = "10.00",
                ClientData = clientData,
                ChallengeRequested = true
            };

            Assert.AreEqual(true, request.ChallengeRequested);

            var outputJSON = request.ToJSON();

            StringAssert.Contains(@"""challengeRequested"":true", outputJSON);
        }

        [Test]
        public void SerializesWithRequestedExemptionType()
        {
            var clientData = @"{
                ""authorizationFingerprint"": ""auth-fingerprint"",
                ""braintreeLibraryVersion"": ""braintree/web/3.44.0"",
                ""dfReferenceId"": ""ABC-123"",
                ""nonce"": ""FAKE-NONCE"",
                ""clientMetadata"": {
                     ""cardinalDeviceDataCollectionTimeElapsed"": 40,
                     ""issuerDeviceDataCollectionResult"": true,
                     ""issuerDeviceDataCollectionTimeElapsed"": 413,
                     ""requestedThreeDSecureVersion"": ""2"",
                     ""sdkVersion"": ""web/3.42.0""
                }
            }";

            ThreeDSecureLookupRequest request = new ThreeDSecureLookupRequest
            {
                Amount = "10.00",
                ClientData = clientData,
                RequestedExemptionType = "low_value"
            };

            Assert.AreEqual("low_value", request.RequestedExemptionType);

            var outputJSON = request.ToJSON();

            StringAssert.Contains(@"""requestedExemptionType"":""low_value""", outputJSON);
        }

        [Test]
        public void SerializesWithMerchantInitiatedRequestType()
        {
            var clientData = @"{
                ""authorizationFingerprint"": ""auth-fingerprint"",
                ""braintreeLibraryVersion"": ""braintree/web/3.44.0"",
                ""dfReferenceId"": ""ABC-123"",
                ""nonce"": ""FAKE-NONCE"",
                ""clientMetadata"": {
                     ""cardinalDeviceDataCollectionTimeElapsed"": 40,
                     ""issuerDeviceDataCollectionResult"": true,
                     ""issuerDeviceDataCollectionTimeElapsed"": 413,
                     ""requestedThreeDSecureVersion"": ""2"",
                     ""sdkVersion"": ""web/3.42.0""
                }
            }";

            ThreeDSecureLookupRequest request = new ThreeDSecureLookupRequest
            {
                Amount = "10.00",
                ClientData = clientData,
                MerchantInitiatedRequestType = "split_shipment"
            };

            Assert.AreEqual("split_shipment", request.MerchantInitiatedRequestType);

            var outputJSON = request.ToJSON();

            StringAssert.Contains(@"""merchantInitiatedRequestType"":""split_shipment""", outputJSON);
        }

        [Test]
        public void SerializesWithMerchantOnRecordName()
        {
            var clientData = @"{
                ""authorizationFingerprint"": ""auth-fingerprint"",
                ""braintreeLibraryVersion"": ""braintree/web/3.44.0"",
                ""dfReferenceId"": ""ABC-123"",
                ""nonce"": ""FAKE-NONCE"",
                ""clientMetadata"": {
                     ""cardinalDeviceDataCollectionTimeElapsed"": 40,
                     ""issuerDeviceDataCollectionResult"": true,
                     ""issuerDeviceDataCollectionTimeElapsed"": 413,
                     ""requestedThreeDSecureVersion"": ""2"",
                     ""sdkVersion"": ""web/3.42.0""
                }
            }";

            ThreeDSecureLookupRequest request = new ThreeDSecureLookupRequest
            {
                Amount = "10.00",
                ClientData = clientData,
                MerchantOnRecordName = "some_merchant"
            };

            Assert.AreEqual("some_merchant", request.MerchantOnRecordName);

            var outputJSON = request.ToJSON();

            StringAssert.Contains(@"""merchantOnRecordName"":""some_merchant""", outputJSON);
        }

        [Test]
        public void SerializesWithPriorThreeDSecureAuthId()
        {
            var clientData = @"{
                ""authorizationFingerprint"": ""auth-fingerprint"",
                ""braintreeLibraryVersion"": ""braintree/web/3.44.0"",
                ""dfReferenceId"": ""ABC-123"",
                ""nonce"": ""FAKE-NONCE"",
                ""clientMetadata"": {
                    ""cardinalDeviceDataCollectionTimeElapsed"": 40,
                    ""issuerDeviceDataCollectionResult"": true,
                    ""issuerDeviceDataCollectionTimeElapsed"": 413,
                    ""requestedThreeDSecureVersion"": ""2"",
                    ""sdkVersion"": ""web/3.42.0""
                }
            }";

            ThreeDSecureLookupRequest request = new ThreeDSecureLookupRequest
            {
                Amount = "10.00",
                ClientData = clientData,
                PriorAuthenticationId = "threedsecureverification"
            };

            Assert.AreEqual("threedsecureverification", request.PriorAuthenticationId);

            var outputJSON = request.ToJSON();

            StringAssert.Contains(@"""priorAuthenticationId"":""threedsecureverification""", outputJSON);
        }

        [Test]
        public void SerializesPriorAuthenticationDetails()
        {
            var clientData = @"{
                ""authorizationFingerprint"": ""auth-fingerprint"",
                ""braintreeLibraryVersion"": ""braintree/web/3.44.0"",
                ""dfReferenceId"": ""ABC-123"",
                ""nonce"": ""FAKE-NONCE"",
                ""clientMetadata"": {
                     ""cardinalDeviceDataCollectionTimeElapsed"": 40,
                     ""issuerDeviceDataCollectionResult"": true,
                     ""issuerDeviceDataCollectionTimeElapsed"": 413,
                     ""requestedThreeDSecureVersion"": ""2"",
                     ""sdkVersion"": ""web/3.42.0""
                }
            }";

            ThreeDSecureLookupPriorAuthenticationDetails priorAuthenticationDetails = new ThreeDSecureLookupPriorAuthenticationDetails{
                DsTransactionId = "some_ds_transaction_id",
                AcsTransactionId = "some_acs_transaction_id",
                AuthenticationMethod = "01",
                AuthenticationTime = new DateTime(2024, 4, 12)
            };

            ThreeDSecureLookupRequest request = new ThreeDSecureLookupRequest
            {
                Amount = "10.00",
                ClientData = clientData,
                PriorAuthenticationDetails = priorAuthenticationDetails
            };

            var outputJSON = request.ToJSON();

            StringAssert.Contains(@"""ds_transaction_id"":""some_ds_transaction_id""", outputJSON);
            StringAssert.Contains(@"""acs_transaction_id"":""some_acs_transaction_id""", outputJSON);
            StringAssert.Contains(@"""authentication_method"":""01""", outputJSON);
            StringAssert.Contains(@"""authentication_time"":""2024-04-12T00:00:00""", outputJSON);
        }

        [Test]
        public void SerializesWithDataOnlyRequestedTrue()
        {
            var clientData = @"{
                ""authorizationFingerprint"": ""auth-fingerprint"",
                ""braintreeLibraryVersion"": ""braintree/web/3.44.0"",
                ""dfReferenceId"": ""ABC-123"",
                ""nonce"": ""FAKE-NONCE"",
                ""clientMetadata"": {
                     ""cardinalDeviceDataCollectionTimeElapsed"": 40,
                     ""issuerDeviceDataCollectionResult"": true,
                     ""issuerDeviceDataCollectionTimeElapsed"": 413,
                     ""requestedThreeDSecureVersion"": ""2"",
                     ""sdkVersion"": ""web/3.42.0""
                }
            }";

            ThreeDSecureLookupRequest request = new ThreeDSecureLookupRequest
            {
                Amount = "10.00",
                ClientData = clientData,
                DataOnlyRequested = true
            };

            Assert.AreEqual(true, request.DataOnlyRequested);

            var outputJSON = request.ToJSON();
            StringAssert.Contains(@"""dataOnlyRequested"":true", outputJSON);
        }

        [Test]
        public void SerializesWithExemptionRequestedTrue()
        {
            var clientData = @"{
                ""authorizationFingerprint"": ""auth-fingerprint"",
                ""braintreeLibraryVersion"": ""braintree/web/3.44.0"",
                ""dfReferenceId"": ""ABC-123"",
                ""nonce"": ""FAKE-NONCE"",
                ""clientMetadata"": {
                     ""cardinalDeviceDataCollectionTimeElapsed"": 40,
                     ""issuerDeviceDataCollectionResult"": true,
                     ""issuerDeviceDataCollectionTimeElapsed"": 413,
                     ""requestedThreeDSecureVersion"": ""2"",
                     ""sdkVersion"": ""web/3.42.0""
                }
            }";

            ThreeDSecureLookupRequest request = new ThreeDSecureLookupRequest
            {
                Amount = "10.00",
                ClientData = clientData,
                ExemptionRequested = true
            };
        
            Assert.AreEqual(true, request.ExemptionRequested);

            var outputJSON = request.ToJSON();
            
            StringAssert.Contains(@"""exemptionRequested"":true", outputJSON);
        }

        [Test]
        public void SerializesWithDeviceDataFields()
        {

            ThreeDSecureLookupRequest request = new ThreeDSecureLookupRequest
            {
                BrowserAcceptHeader = "text/html;q=0.8",
                BrowserColorDepth = "48",
                BrowserJavaEnabled = false,
                BrowserJavascriptEnabled = true,
                BrowserLanguage = "fr-CA",
                BrowserScreenHeight = "600",
                BrowserScreenWidth = "800",
                BrowserTimeZone = "-60",
                DeviceChannel = "Browser",
                IpAddress = "2001:0db8:0000:0000:0000:ff00:0042:8329",
                UserAgent = "Mozilla/5.0"
            };

            var outputJSON = request.ToJSON();

            StringAssert.Contains(@"""browserHeader"":""text/html;q=0.8""", outputJSON);
            StringAssert.Contains(@"""browserColorDepth"":""48""", outputJSON);
            StringAssert.Contains(@"""browserJavaEnabled"":false", outputJSON);
            StringAssert.Contains(@"""browserJavascriptEnabled"":true", outputJSON);
            StringAssert.Contains(@"""browserLanguage"":""fr-CA""", outputJSON);
            StringAssert.Contains(@"""browserScreenHeight"":""600""", outputJSON);
            StringAssert.Contains(@"""browserScreenWidth"":""800""", outputJSON);
            StringAssert.Contains(@"""browserTimeZone"":""-60""", outputJSON);
            StringAssert.Contains(@"""deviceChannel"":""Browser""", outputJSON);
            StringAssert.Contains(@"""ipAddress"":""2001:0db8:0000:0000:0000:ff00:0042:8329""", outputJSON);
            StringAssert.Contains(@"""userAgent"":""Mozilla/5.0""", outputJSON);
        }
    }
}
