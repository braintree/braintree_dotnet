using Braintree.Exceptions;
using Braintree.Test;
using Braintree.TestUtil;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Braintree.Tests.Integration
{
    [TestFixture]
    public class TransactionPaymentFacilitatorIntegrationTest
    {
        private BraintreeGateway gateway;
        private BraintreeGateway ezp_gateway; 
        private BraintreeService service;
        private BraintreeService ezp_service;

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

            ezp_gateway = new BraintreeGateway
            {
                Environment = Environment.DEVELOPMENT,
                MerchantId = "pp_credit_ezp_merchant",
                PublicKey = "pp_credit_ezp_merchant_public_key",
                PrivateKey = "pp_credit_ezp_merchant_private_key"
            };

            ezp_service = new BraintreeService(ezp_gateway.Configuration);
        }

        [Test]
        public void Sale_ShouldCreateTransactionWithSubMerchantAndPaymentFacilitator()
        {
            var transactionRequest = new TransactionRequest
            {
                Amount = 100.00M,
                MerchantAccountId = MerchantAccountIDs.CARD_PROCESSOR_BRAZIL_PAYFAC_MERCHANT_ACCOUNT_ID,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = "4111111111111111",
                    ExpirationDate = "06/2026",
                    CVV = "123"
                },
                Descriptor = new DescriptorRequest
                {
                    Name = "companynme12*product12",
                    Phone = "1232344444",
                    Url = "example.com"
                },
                BillingAddress = new AddressRequest
                {
                    FirstName = "Bob James",
                    CountryCodeAlpha2 = "CA",
                    ExtendedAddress = "",
                    Locality = "Trois-Rivires",
                    Region = "QC",
                    PostalCode = "G8Y 156",
                    StreetAddress = "2346 Boul Lane"
                },
                PaymentFacilitator = new PaymentFacilitatorRequest
                {
                    PaymentFacilitatorId = "98765432109",
                    SubMerchant = new TransactionSubMerchantRequest
                    {   
                        LegalName = "Fooda",
                        ReferenceNumber = "123456789012345",
                        TaxId = "99112233445577",
                        Address = new AddressRequest
                        {
                            StreetAddress = "10880 Ibitinga",
                            Locality = "Araraquara",
                            Region = "SP",
                            CountryCodeAlpha2 = "BR",
                            PostalCode = "13525000",
                            InternationalPhone = new InternationalPhoneRequest
                            {
                                CountryCode = "55",
                                NationalNumber = "9876543210"
                            }
                        },
                  
                    }
                },
                Options = new TransactionOptionsRequest
                {
                    StoreInVaultOnSuccess = true
                }
            };
            try{
                var result = gateway.Transaction.Sale(transactionRequest);
                Assert.IsTrue(result.IsSuccess());
                Assert.AreEqual(TransactionStatus.AUTHORIZED, result.Target.Status);
            }
             catch(Exception ex){
                Assert.Fail(ex.ToString());
             }
           
        }

        [Test]
        public void Sale_ShouldNotCreateTransactionWithSubMerchantAndPaymentFacilitatorNonPayFacMerchant()
        {

            var transactionRequest = new TransactionRequest
            {
                Amount = 100.00M,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = "4111111111111111",
                    ExpirationDate = "06/2026",
                    CVV = "123"
                },
                Descriptor = new DescriptorRequest
                {
                    Name = "companynme12*product12",
                    Phone = "1232344444",
                    Url = "example.com"
                },
                BillingAddress = new AddressRequest
                {
                    FirstName = "Bob James",
                    CountryCodeAlpha2 = "CA",
                    ExtendedAddress = "",
                    Locality = "Trois-Rivires",
                    Region = "QC",
                    PostalCode = "G8Y 156",
                    StreetAddress = "2346 Boul Lane"
                },
                PaymentFacilitator = new PaymentFacilitatorRequest
                {
                    PaymentFacilitatorId = "98765432109",
                    SubMerchant = new TransactionSubMerchantRequest
                    {   
                        
                        LegalName = "Fooda",
                        ReferenceNumber = "123456789012345",
                        TaxId = "99112233445577",
                        Address = new AddressRequest
                        {
                            StreetAddress = "10880 Ibitinga",
                            Locality = "Araraquara",
                            Region = "SP",
                            CountryCodeAlpha2 = "BR",
                            PostalCode = "13525000",
                            InternationalPhone = new InternationalPhoneRequest
                            {
                                CountryCode = "55",
                                NationalNumber = "9876543210"
                            }
                        },
                    }
                },
                Options = new TransactionOptionsRequest
                {
                    StoreInVaultOnSuccess = true
                }
            };
            try{

                var result = ezp_gateway.Transaction.Sale(transactionRequest);
                Assert.IsFalse(result.IsSuccess());
                Assert.AreEqual(ValidationErrorCode.TRANSACTION_PAYMENT_FACILITATOR_NOT_APPLICABLE, result.Errors.DeepAll()[0].Code);

            }
             catch(Exception ex){
                Assert.Fail(ex.ToString());
             }
           
        }
    }
}