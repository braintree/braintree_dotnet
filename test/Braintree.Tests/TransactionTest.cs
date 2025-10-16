using Braintree.Exceptions;
using NUnit.Framework;
using System;
using System.Xml;

namespace Braintree.Tests
{
    [TestFixture]
    public class TransactionTest
    {
        private BraintreeGateway gateway;
        private BraintreeService service;

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

            service = new BraintreeService(gateway.Configuration);
        }

        [Test]
        public void Find_FindsErrorsOutOnWhitespaceIds()
        {
            Assert.Throws<NotFoundException>(() => gateway.Transaction.Find(" "));
        }

        [Test]
        public void TransactionRequest_ToXml_Includes_SkipAdvancedFraudChecking()
        {
            var request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2016",
                },
                Options = new TransactionOptionsRequest
                {
                    SkipAdvancedFraudChecking = false
                }
            };
            Assert.IsTrue(request.ToXml().Contains("<skip-advanced-fraud-checking>false</skip-advanced-fraud-checking>"));
        }

        [Test]
        public void UnrecognizedValuesAreCategorizedAsSuch()
        {
            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
                "<transaction>\n" +
                "  <id>unrecognized_transaction_id</id>\n" +
                "  <status>unrecognizable status</status>\n" +
                "  <type>sale</type>\n" +
                "  <customer></customer>\n" +
                "  <billing></billing>\n" +
                "  <shipping></shipping>\n" +
                "  <custom-fields/>\n" +
                "  <gateway-rejection-reason>unrecognizable gateway rejection reason</gateway-rejection-reason>\n" +
                "  <credit-card></credit-card>\n" +
                "  <status-history type=\"array\"></status-history>\n" +
                "  <subscription></subscription>\n" +
                "  <descriptor></descriptor>\n" +
                "  <disbursement-details></disbursement-details>\n" +
                "  <payment-instrument-type>credit_card</payment-instrument-type>\n" +
                "</transaction>\n";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode newNode = doc.DocumentElement;

            var node = new NodeWrapper(newNode);

            Transaction transaction = new Transaction(node, gateway);

            Assert.AreEqual(TransactionGatewayRejectionReason.UNRECOGNIZED, transaction.GatewayRejectionReason);
            Assert.AreEqual(TransactionStatus.UNRECOGNIZED, transaction.Status);
        }

        [Test]
        public void RecognizesTokenIssuanceGatewayRejectReason()
        {
            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
                "<transaction>\n" +
                "  <id></id>\n" +
                "  <status></status>\n" +
                "  <type>sale</type>\n" +
                "  <customer></customer>\n" +
                "  <billing></billing>\n" +
                "  <shipping></shipping>\n" +
                "  <custom-fields/>\n" +
                "  <gateway-rejection-reason>token_issuance</gateway-rejection-reason>\n" +
                "  <credit-card></credit-card>\n" +
                "  <status-history type=\"array\"></status-history>\n" +
                "  <subscription></subscription>\n" +
                "  <descriptor></descriptor>\n" +
                "  <disbursement-details></disbursement-details>\n" +
                "  <payment-instrument-type>credit_card</payment-instrument-type>\n" +
                "</transaction>\n";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode newNode = doc.DocumentElement;

            var node = new NodeWrapper(newNode);

            Transaction transaction = new Transaction(node, gateway);

            Assert.AreEqual(TransactionGatewayRejectionReason.TOKEN_ISSUANCE, transaction.GatewayRejectionReason);
            Assert.AreEqual(TransactionStatus.UNRECOGNIZED, transaction.Status);
        }

        [Test]
        public void RecognizesExcessiveRetryGatewayRejectReason()
        {
            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
                "<transaction>\n" +
                "  <id></id>\n" +
                "  <status></status>\n" +
                "  <type>sale</type>\n" +
                "  <customer></customer>\n" +
                "  <billing></billing>\n" +
                "  <shipping></shipping>\n" +
                "  <custom-fields/>\n" +
                "  <gateway-rejection-reason>excessive_retry</gateway-rejection-reason>\n" +
                "  <credit-card></credit-card>\n" +
                "  <status-history type=\"array\"></status-history>\n" +
                "  <subscription></subscription>\n" +
                "  <descriptor></descriptor>\n" +
                "  <disbursement-details></disbursement-details>\n" +
                "  <payment-instrument-type>credit_card</payment-instrument-type>\n" +
                "</transaction>\n";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode newNode = doc.DocumentElement;

            var node = new NodeWrapper(newNode);

            Transaction transaction = new Transaction(node, gateway);

            Assert.AreEqual(TransactionGatewayRejectionReason.EXCESSIVE_RETRY, transaction.GatewayRejectionReason);
        }

        [Test]
        public void DeserializesAchReturnCodeFromXml()
        {
            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
                "<transaction>\n" +
                "  <shipping-amount>1.00</shipping-amount>\n" +
                "  <ach-return-code>R01</ach-return-code>\n" +
                "  <discount-amount>2.00</discount-amount>\n" +
                "  <ships-from-postal-code>12345</ships-from-postal-code>\n" +
                "  <disbursement-details></disbursement-details>\n" +
                "  <subscription></subscription>\n" +
                "</transaction>\n";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode newNode = doc.DocumentElement;
            var node = new NodeWrapper(newNode);

            Transaction transaction = new Transaction(node, gateway);

            Assert.AreEqual("R01", transaction.AchReturnCode);
        }

        [Test]
        public void DeserializesAchRejectReasonFromXml()
        {
            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
                "<transaction>\n" +
                "  <shipping-amount>1.00</shipping-amount>\n" +
                "  <ach-return-code>RJCT</ach-return-code>\n" +
                "  <ach-reject-reason>Reject Reason</ach-reject-reason>\n" +
                "  <discount-amount>2.00</discount-amount>\n" +
                "  <ships-from-postal-code>12345</ships-from-postal-code>\n" +
                "  <disbursement-details></disbursement-details>\n" +
                "  <subscription></subscription>\n" +
                "</transaction>\n";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode newNode = doc.DocumentElement;
            var node = new NodeWrapper(newNode);

            Transaction transaction = new Transaction(node, gateway);

            Assert.AreEqual("RJCT", transaction.AchReturnCode);
            Assert.AreEqual("Reject Reason", transaction.AchRejectReason);
        }

        [Test]
        public void DeserializesLevel3SummaryFieldsFromXml()
        {
            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
                "<transaction>\n" +
                "  <shipping-amount>1.00</shipping-amount>\n" +
                "  <shipping-tax-amount>1.00</shipping-tax-amount>\n" +
                "  <discount-amount>2.00</discount-amount>\n" +
                "  <ships-from-postal-code>12345</ships-from-postal-code>\n" +
                "  <disbursement-details></disbursement-details>\n" +
                "  <subscription></subscription>\n" +
                "</transaction>\n";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode newNode = doc.DocumentElement;
            var node = new NodeWrapper(newNode);

            Transaction transaction = new Transaction(node, gateway);

            Assert.AreEqual(1.00M, transaction.ShippingAmount);
            Assert.AreEqual(1.00M, transaction.ShippingTaxAmount);
            Assert.AreEqual(2.00M, transaction.DiscountAmount);
            Assert.AreEqual("12345", transaction.ShipsFromPostalCode);
        }

        [Test]
        public void DeserializesAuthorizationAdjustmentsFromXml()
        {
            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
                "<transaction>\n" +
                "  <authorization-adjustments>\n" +
                "    <authorization-adjustment>\n" +
                "      <amount>10.00</amount>\n" +
                "      <success>true</success>\n" +
                "      <timestamp>2018-05-16T12:00:00+00:00</timestamp>\n" +
                "      <processor-response-code>1000</processor-response-code>\n" +
                "      <processor-response-text>Approved</processor-response-text>\n" +
                "    </authorization-adjustment>\n" +
                "  </authorization-adjustments>\n" +
                "  <disbursement-details></disbursement-details>\n" +
                "  <subscription></subscription>\n" +
                "</transaction>\n";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode newNode = doc.DocumentElement;
            var node = new NodeWrapper(newNode);

            Transaction transaction = new Transaction(node, gateway);

            Assert.AreEqual(10.00M, transaction.AuthorizationAdjustments[0].Amount);
            Assert.AreEqual(true, transaction.AuthorizationAdjustments[0].Success);
            Assert.AreEqual(DateTime.Parse("5/16/18 12:00:00 PM"), transaction.AuthorizationAdjustments[0].Timestamp);
            Assert.AreEqual("1000", transaction.AuthorizationAdjustments[0].ProcessorResponseCode);
            Assert.AreEqual("Approved", transaction.AuthorizationAdjustments[0].ProcessorResponseText);
        }

        [Test]
        public void DeserializesNetworkTransactionIdFromXml()
        {
            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
                "<transaction>\n" +
                "  <network-transaction-id>123456789012345</network-transaction-id>\n" +
                "  <disbursement-details></disbursement-details>\n" +
                "  <subscription></subscription>\n" +
                "</transaction>\n";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode newNode = doc.DocumentElement;
            var node = new NodeWrapper(newNode);

            Transaction transaction = new Transaction(node, gateway);

            Assert.AreEqual("123456789012345", transaction.NetworkTransactionId);
        }

        [Test]
        public void DeserializesNetworkResponseCodeAndTextFromXml()
        {
            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
                "<transaction>\n" +
                "  <network-response-code>00</network-response-code>\n" +
                "  <network-response-text>Approved</network-response-text>\n" +
                "  <disbursement-details></disbursement-details>\n" +
                "  <subscription></subscription>\n" +
                "</transaction>\n";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode newNode = doc.DocumentElement;
            var node = new NodeWrapper(newNode);

            Transaction transaction = new Transaction(node, gateway);

            Assert.AreEqual("00", transaction.NetworkResponseCode);
            Assert.AreEqual("Approved", transaction.NetworkResponseText);
        }

        [Test]
        public void DeserializesMerchantAdviceCodeAndTextFromXml()
        {
            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
                "<transaction>\n" +
                "  <merchant-advice-code>01</merchant-advice-code>\n" +
                "  <merchant-advice-code-text>New account information available</merchant-advice-code-text>\n" +
                "</transaction>\n";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode newNode = doc.DocumentElement;
            var node = new NodeWrapper(newNode);

            Transaction transaction = new Transaction(node, gateway);
            Assert.AreEqual("01", transaction.MerchantAdviceCode);
            Assert.AreEqual("New account information available", transaction.MerchantAdviceCodeText);
        }

        [Test]
        public void DeserializesSepaDirectDebitReturnCode()
        {
            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
                "<transaction>\n" +
                "  <sepa-direct-debit-return-code>AM04</sepa-direct-debit-return-code>\n" +
                "  <disbursement-details></disbursement-details>\n" +
                "  <subscription></subscription>\n" +
                "</transaction>\n";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode newNode = doc.DocumentElement;
            var node = new NodeWrapper(newNode);

            Transaction transaction = new Transaction(node, gateway);

            Assert.AreEqual("AM04", transaction.SepaDirectDebitReturnCode);
        }

        [Test]
        public void DeserializesSepaDirectDebitAccountDetail()
        {
            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
                "<transaction>\n" +
                "  <sepa-debit-account-detail>\n" +
                "    <token>abcdef</token>\n" +
                "  </sepa-debit-account-detail>\n" +
                "  <disbursement-details></disbursement-details>\n" +
                "  <subscription></subscription>\n" +
                "</transaction>\n";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode newNode = doc.DocumentElement;
            var node = new NodeWrapper(newNode);

            Transaction transaction = new Transaction(node, gateway);

            Assert.AreEqual("abcdef", transaction.SepaDirectDebitAccountDetails.Token);
        }

        [Test]
        public void DeserializesRetryIdsFromXml()
        {
            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
                "<transaction>\n" +
                "<retry-ids>\n" +
                    "<value>123ccs</value>\n" +
                    "<value>8cnu3d</value>\n" +
                "</retry-ids>\n" +
                "<retried>true</retried>\n" +
                "</transaction>\n";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode newNode = doc.DocumentElement;
            var node = new NodeWrapper(newNode);

            Transaction transaction = new Transaction(node, gateway);
            Assert.AreEqual(transaction.RetryIds.Count, 2);
            Assert.IsTrue(transaction.Retried);
        }

        [Test]
        public void DeserializesUpcomingRetryDateFromXml()
        {
            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
                "<transaction>\n" +
                "<upcoming-retry-date>2024-12-31</upcoming-retry-date>\n" +
                "</transaction>\n";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode newNode = doc.DocumentElement;
            var node = new NodeWrapper(newNode);

            Transaction transaction = new Transaction(node, gateway);
            Assert.AreEqual("2024-12-31", transaction.UpcomingRetryDate);
        }

        [Test]
        public void DeserializesRetriedTransactionIdFromXml()
        {
            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
                "<transaction>\n" +
                "<retried>true</retried>\n" +
                "<retried-transaction-id>32fi8x</retried-transaction-id>\n" +
                "</transaction>\n";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode newNode = doc.DocumentElement;
            var node = new NodeWrapper(newNode);

            Transaction transaction = new Transaction(node, gateway);
            Assert.IsNotNull(transaction.RetriedTransactionId);
            Assert.IsTrue(transaction.Retried);
        }

        [Test]
        public void TestDebitNetwork()
        {
            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
                "<transaction>\n" +
                "  <id>recognized_transaction_id</id>\n" +
                "  <type>sale</type>\n" +
                "  <payment-method-nonce>fake-pinless-debit-visa-nonce</payment-method-nonce>\n" +
                "  <merchant-account-id>pinless_debit</merchant-account-id>\n" +
                "  <debit-network>STAR</debit-network>\n" +
                "</transaction>";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode newNode = doc.DocumentElement;
            var node = new NodeWrapper(newNode);

            Transaction transaction = new Transaction(node, gateway);
            Assert.AreEqual(Braintree.TransactionDebitNetwork.STAR,transaction.DebitNetwork);
        }

        [Test]
        public void DeserializesForeignRetailerFromXml()
        {
            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
                "<transaction>\n" +
                "  <foreign-retailer>true</foreign-retailer>\n" +
                "</transaction>\n";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode newNode = doc.DocumentElement;
            var node = new NodeWrapper(newNode);

            Transaction transaction = new Transaction(node, gateway);
            Assert.IsTrue(transaction.ForeignRetailer);
        }

        [Test]
        public void DeserializesExternalNetworkTokenResponseFromXml()
        {
            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
                "<transaction>\n" +
                "<network-token>\n" +
                "  <is-network-tokenized>true</is-network-tokenized>\n" +
                "</network-token>\n" +
                "</transaction>\n";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode newNode = doc.DocumentElement;
            var node = new NodeWrapper(newNode);

            Transaction transaction = new Transaction(node, gateway);

            Assert.IsTrue(transaction.NetworkToken.IsNetworkTokenized);
        }
        [Test]
        public void BuildRequest_PaymentFacilitator_IncludedWhenNotNull()
        {
        
            var transactionRequest = new TransactionRequest
            {
                PaymentFacilitator = new PaymentFacilitatorRequest
                {
                    
                    PaymentFacilitatorId = "4321",
                    SubMerchant = new TransactionSubMerchantRequest
                    {
                        Address = new AddressRequest
                        {   
                            CountryCodeAlpha2 = " BR",
                            ExtendedAddress = "Ibitinga",
                            Locality = "Araraquara",
                            PostalCode = "13525000",
                            Region = "SP",
                            StreetAddress = "10880 Ibitinga",
                            InternationalPhone = new InternationalPhoneRequest
                            {
                                CountryCode = "55",
                                NationalNumber = "9876543210"
                            },
                        },
                        LegalName = "Sub Tony Stark",
                        ReferenceNumber = "123456789012345",
                        TaxId = "ACSD1234567890"
                    }
                }
            };

            
            string xml = transactionRequest.ToXml();
            
            Assert.IsTrue(xml.Contains("<payment-facilitator>"));
            Assert.IsTrue(xml.Contains("<legal-name>Sub Tony Stark</legal-name>"));

            Assert.IsTrue(xml.Contains("<sub-merchant>"));
            Assert.IsTrue(xml.Contains("<address>"));
            Assert.IsTrue(xml.Contains("<extended-address>Ibitinga</extended-address>"));
            Assert.IsTrue(xml.Contains("<locality>Araraquara</locality>"));
            Assert.IsTrue(xml.Contains("<postal-code>13525000</postal-code>"));
            Assert.IsTrue(xml.Contains("<region>SP</region>"));
            Assert.IsTrue(xml.Contains("<street-address>10880 Ibitinga</street-address>"));
            Assert.IsTrue(xml.Contains("<international-phone>"));
            Assert.IsTrue(xml.Contains("<country-code>55</country-code>"));
            Assert.IsTrue(xml.Contains("<national-number>9876543210</national-number>"));
            Assert.IsTrue(xml.Contains("<reference-number>123456789012345</reference-number>"));
            Assert.IsTrue(xml.Contains("<tax-id>ACSD1234567890</tax-id>"));
        }

        [Test]
        public void BuildRequest_PaymentFacilitator_NotIncludedWhenNull()
        {
            var transactionRequest = new TransactionRequest
            {
                PaymentFacilitator = null 
            };
            
            var xml = transactionRequest.ToXml();

            Assert.IsFalse(xml.Contains("<payment-facilitator>"));
        }

        [Test]
        public void BuildRequest_Transfer_IncludedWhenNotNull()
        {
            string[] transferTypes = { "account_to_account", "boleto_ticket", "person_to_person", "wallet_transfer" };
            
            foreach (string transferType in transferTypes)
            {
                var transactionRequest = new TransactionRequest
                {
                    Amount = 100.00m,
                    Transfer = new TransferRequest
                    {
                        Type = transferType,
                        Sender = new SenderRequest
                        {
                            FirstName = "Alice",
                            LastName = "Silva",
                            AccountReferenceNumber = "1000012345",
                            TaxId = "12345678900",
                            Address = new AddressRequest
                            {
                                StreetAddress = "Rua das Flores, 100",
                                ExtendedAddress = "2B",
                                Locality = "São Paulo",
                                Region = "SP",
                                PostalCode = "01001-000",
                                CountryCodeAlpha2 = "BR",
                                InternationalPhone = new InternationalPhoneRequest
                                {
                                    CountryCode = "55",
                                    NationalNumber = "1234567890"
                                }
                            }
                        },
                        Receiver = new ReceiverRequest
                        {
                            FirstName = "Bob",
                            LastName = "Souza",
                            AccountReferenceNumber = "2000012345",
                            TaxId = "98765432100",
                            Address = new AddressRequest
                            {
                                StreetAddress = "Avenida Brasil, 200",
                                ExtendedAddress = "2B",
                                Locality = "Rio de Janeiro",
                                Region = "RJ",
                                PostalCode = "20040-002",
                                CountryCodeAlpha2 = "BR",
                                InternationalPhone = new InternationalPhoneRequest
                                {
                                    CountryCode = "55",
                                    NationalNumber = "9876543210"
                                }
                            }
                        }
                    }
                };

                string xml = transactionRequest.ToXml();

                Assert.IsTrue(xml.Contains("<transfer>"));
                Assert.IsTrue(xml.Contains($"<type>{transferType}</type>"));
                Assert.IsTrue(xml.Contains("<sender>"));
                Assert.IsTrue(xml.Contains("<first-name>Alice</first-name>"));
                Assert.IsTrue(xml.Contains("<last-name>Silva</last-name>"));
                Assert.IsTrue(xml.Contains("<account-reference-number>1000012345</account-reference-number>"));
                Assert.IsTrue(xml.Contains("<tax-id>12345678900</tax-id>"));
                Assert.IsTrue(xml.Contains("<address>"));
                Assert.IsTrue(xml.Contains("<street-address>Rua das Flores, 100</street-address>"));
                Assert.IsTrue(xml.Contains("<extended-address>2B</extended-address>"));
                Assert.IsTrue(xml.Contains("<locality>São Paulo</locality>"));
                Assert.IsTrue(xml.Contains("<region>SP</region>"));
                Assert.IsTrue(xml.Contains("<postal-code>01001-000</postal-code>"));
                Assert.IsTrue(xml.Contains("<country-code-alpha2>BR</country-code-alpha2>"));
                Assert.IsTrue(xml.Contains("<international-phone>"));
                Assert.IsTrue(xml.Contains("<country-code>55</country-code>"));
                Assert.IsTrue(xml.Contains("<national-number>1234567890</national-number>"));

                Assert.IsTrue(xml.Contains("<receiver>"));
                Assert.IsTrue(xml.Contains("<first-name>Bob</first-name>"));
                Assert.IsTrue(xml.Contains("<last-name>Souza</last-name>"));
                Assert.IsTrue(xml.Contains("<account-reference-number>2000012345</account-reference-number>"));
                Assert.IsTrue(xml.Contains("<tax-id>98765432100</tax-id>"));
                Assert.IsTrue(xml.Contains("<address>"));
                Assert.IsTrue(xml.Contains("<street-address>Avenida Brasil, 200</street-address>"));
                Assert.IsTrue(xml.Contains("<extended-address>2B</extended-address>"));
                Assert.IsTrue(xml.Contains("<locality>Rio de Janeiro</locality>"));
                Assert.IsTrue(xml.Contains("<region>RJ</region>"));
                Assert.IsTrue(xml.Contains("<postal-code>20040-002</postal-code>"));
                Assert.IsTrue(xml.Contains("<country-code-alpha2>BR</country-code-alpha2>"));
                Assert.IsTrue(xml.Contains("<international-phone>"));
                Assert.IsTrue(xml.Contains("<country-code>55</country-code>"));
                Assert.IsTrue(xml.Contains("<national-number>9876543210</national-number>"));
            }
        }

        [Test]
        public void BuildRequest_Transfer_NotIncludedWhenNull()
        {
            var transactionRequest = new TransactionRequest
            {
                Transfer = null
            };

            var xml = transactionRequest.ToXml();

            Assert.IsFalse(xml.Contains("<transfer>"));
        }
    }
}
