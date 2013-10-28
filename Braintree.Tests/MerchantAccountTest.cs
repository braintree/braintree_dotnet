using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Braintree;
using Braintree.Exceptions;

namespace Braintree.Tests
{
    [TestFixture]
    public class MerchantAccountTest
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
        public void Create_WithoutId()
        {
            var request = createRequest(null);
            Result<MerchantAccount> result = gateway.MerchantAccount.Create(request);
            Assert.IsTrue(result.IsSuccess());

            MerchantAccount merchantAccount = result.Target;
            Assert.AreEqual(MerchantAccountStatus.PENDING, merchantAccount.Status);
            Assert.AreEqual("sandbox_master_merchant_account", merchantAccount.MasterMerchantAccount.Id);
            Assert.IsTrue(merchantAccount.IsSubMerchant);
            Assert.IsFalse(merchantAccount.MasterMerchantAccount.IsSubMerchant);
            Assert.IsTrue(merchantAccount.Id != null);
        }

        [Test]
        public void Create_WithId()
        {
            Random random = new Random();
            int randomNumber = random.Next(0, 10000);
            var subMerchantAccountId = "sub_merchant_account_id_" + randomNumber;
            var request = createRequest(subMerchantAccountId);
            Result<MerchantAccount> result = gateway.MerchantAccount.Create(request);
            Assert.IsTrue(result.IsSuccess());

            MerchantAccount merchantAccount = result.Target;
            Assert.AreEqual(MerchantAccountStatus.PENDING, merchantAccount.Status);
            Assert.AreEqual("sandbox_master_merchant_account", merchantAccount.MasterMerchantAccount.Id);
            Assert.IsTrue(merchantAccount.IsSubMerchant);
            Assert.IsFalse(merchantAccount.MasterMerchantAccount.IsSubMerchant);
            Assert.AreEqual(subMerchantAccountId, merchantAccount.Id);
        }

        [Test]
        public void Create_HandlesUnsuccessfulResults()
        {
            Result<MerchantAccount> result = gateway.MerchantAccount.Create(new MerchantAccountRequest());
            Assert.IsFalse(result.IsSuccess());
            List<ValidationError> errors = result.Errors.ForObject("merchant-account").OnField("master-merchant-account-id");
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual(ValidationErrorCode.MERCHANT_ACCOUNT_MASTER_MERCHANT_ACCOUNT_ID_IS_REQUIRED, errors[0].Code);

        }

        private MerchantAccountRequest createRequest(String id)
        {
            return new MerchantAccountRequest
            {
                ApplicantDetails = new ApplicantDetailsRequest
                {
                    CompanyName = "coattree.com",
                    FirstName = "Joe",
                    LastName = "Bloggs",
                    Email = "joe@bloggs.com",
                    Phone = "555-555-5555",
                    Address = new AddressRequest
                    {
                        StreetAddress = "123 Credibility St.",
                        PostalCode = "60606",
                        Locality = "Chicago",
                        Region = "IL",
                    },
                    DateOfBirth = "10/9/1980",
                    Ssn = "123-00-1234",
                    TaxId = "123456789",
                    RoutingNumber = "122100024",
                    AccountNumber = "43759348798"
              },
              TosAccepted = true,
              Id = id,
              MasterMerchantAccountId = "sandbox_master_merchant_account"
            };
        }
    }
}
