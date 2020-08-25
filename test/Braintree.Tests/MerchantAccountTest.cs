using Braintree.Exceptions;
using NUnit.Framework;

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
        public void Find_FindsErrorsOutOnWhitespaceIds()
        {
            Assert.Throws<NotFoundException>(() => gateway.MerchantAccount.Find(" "));
        }
        // this is part of Marketplace and shouldn't be removed unless we're removing all Marketplace code
        private MerchantAccountRequest deprecatedCreateRequest(string id)
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

        private MerchantAccountRequest createRequest(string id)
        {
            return new MerchantAccountRequest
            {
                Individual = new IndividualRequest
                {
                    FirstName = "Job",
                    LastName = "Leoggs",
                    Email = "job@leoggs.com",
                    Phone = "555-555-1212",
                    Address = new AddressRequest
                    {
                        StreetAddress = "193 Credibility St.",
                        PostalCode = "60611",
                        Locality = "Avondale",
                        Region = "IN",
                    },
                    DateOfBirth = "10/9/1985",
                    Ssn = "123-00-1235",
                },
                Business = new BusinessRequest
                {
                    LegalName = "coaterie.com",
                    DbaName = "Coaterie",
                    TaxId = "123456780",
                    Address = new AddressRequest
                    {
                        StreetAddress = "135 Credibility St.",
                        PostalCode = "60602",
                        Locality = "Gary",
                        Region = "OH",
                    },
                },
                Funding = new FundingRequest
                {
                    Destination = FundingDestination.EMAIL,
                    Email = "joe+funding@bloggs.com",
                    MobilePhone = "3125551212",
                    RoutingNumber = "122100024",
                    AccountNumber = "43759348798",
                    Descriptor = "Job Leoggs OH",
                },
                TosAccepted = true,
                Id = id,
                MasterMerchantAccountId = "sandbox_master_merchant_account"
            };
        }
    }
}
