using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Braintree;
using Braintree.Exceptions;

namespace Braintree.Tests
{
    [TestFixture]
    public class AddressTest
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
        public void Create_CreatesAddressForGivenCustomerId()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;

            var addressRequest = new AddressRequest
            {
                FirstName = "Michael",
                LastName = "Angelo",
                Company = "Angelo Co.",
                StreetAddress = "1 E Main St",
                ExtendedAddress = "Apt 3",
                Locality = "Chicago",
                Region = "IL",
                PostalCode = "60622",
                CountryCodeAlpha2 = "US",
                CountryCodeAlpha3 = "USA",
                CountryCodeNumeric = "840",
                CountryName = "United States of America"
            };

            Address address = gateway.Address.Create(customer.Id, addressRequest).Target;

            Assert.AreEqual("Michael", address.FirstName);
            Assert.AreEqual("Angelo", address.LastName);
            Assert.AreEqual("Angelo Co.", address.Company);
            Assert.AreEqual("1 E Main St", address.StreetAddress);
            Assert.AreEqual("Apt 3", address.ExtendedAddress);
            Assert.AreEqual("Chicago", address.Locality);
            Assert.AreEqual("IL", address.Region);
            Assert.AreEqual("60622", address.PostalCode);
            Assert.AreEqual("US", address.CountryCodeAlpha2);
            Assert.AreEqual("USA", address.CountryCodeAlpha3);
            Assert.AreEqual("840", address.CountryCodeNumeric);
            Assert.AreEqual("United States of America", address.CountryName);
            Assert.IsNotNull(address.CreatedAt);
            Assert.IsNotNull(address.UpdatedAt);
        }

        [Test]
        public void Find_FindsAddress()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;

            var addressRequest = new AddressRequest
            {
                FirstName = "Michael",
                LastName = "Angelo",
                Company = "Angelo Co.",
                StreetAddress = "1 E Main St",
                ExtendedAddress = "Apt 3",
                Locality = "Chicago",
                Region = "IL",
                PostalCode = "60622",
                CountryName = "United States of America"
            };

            Address createdAddress = gateway.Address.Create(customer.Id, addressRequest).Target;
            Address address = gateway.Address.Find(customer.Id, createdAddress.Id);

            Assert.AreEqual("Michael", address.FirstName);
            Assert.AreEqual("Angelo", address.LastName);
            Assert.AreEqual("Angelo Co.", address.Company);
            Assert.AreEqual("1 E Main St", address.StreetAddress);
            Assert.AreEqual("Apt 3", address.ExtendedAddress);
            Assert.AreEqual("Chicago", address.Locality);
            Assert.AreEqual("IL", address.Region);
            Assert.AreEqual("60622", address.PostalCode);
            Assert.AreEqual("United States of America", address.CountryName);
        }

        [Test]
        public void Update_UpdatesAddressForGivenCustomerIdAndAddressId()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;

            var addressCreateRequest = new AddressRequest
            {
                FirstName = "Dave",
                LastName = "Inchy",
                Company = "Leon Ardo Co.",
                StreetAddress = "1 E State St",
                ExtendedAddress = "Apt 4",
                Locality = "Boston",
                Region = "MA",
                PostalCode = "11111",
                CountryName = "Canada"
            };

            Address originalAddress = gateway.Address.Create(customer.Id, addressCreateRequest).Target;

            var addressUpdateRequest = new AddressRequest
            {
                FirstName = "Michael",
                LastName = "Angelo",
                Company = "Angelo Co.",
                StreetAddress = "1 E Main St",
                ExtendedAddress = "Apt 3",
                Locality = "Chicago",
                Region = "IL",
                PostalCode = "60622",
                CountryName = "United States of America"
            };

            Address address = gateway.Address.Update(customer.Id, originalAddress.Id, addressUpdateRequest).Target;

            Assert.AreEqual("Michael", address.FirstName);
            Assert.AreEqual("Angelo", address.LastName);
            Assert.AreEqual("Angelo Co.", address.Company);
            Assert.AreEqual("1 E Main St", address.StreetAddress);
            Assert.AreEqual("Apt 3", address.ExtendedAddress);
            Assert.AreEqual("Chicago", address.Locality);
            Assert.AreEqual("IL", address.Region);
            Assert.AreEqual("60622", address.PostalCode);
            Assert.AreEqual("United States of America", address.CountryName);
        }

        [Test]
        public void Delete_DeletesTheAddress()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;

            var addressRequest = new AddressRequest
            {
                StreetAddress = "1 E Main St",
                ExtendedAddress = "Apt 3",
            };

            Address createdAddress = gateway.Address.Create(customer.Id, addressRequest).Target;
            Assert.AreEqual(createdAddress.Id, gateway.Address.Find(customer.Id, createdAddress.Id).Id);
            gateway.Address.Delete(customer.Id, createdAddress.Id);
            try
            {
                gateway.Address.Find(customer.Id, createdAddress.Id);
                Assert.Fail("Expected NotFoundException.");
            }
            catch (NotFoundException)
            {
                // expected
            }
        }

        [Test]
        public void Create_ReturnsAnErrorResult()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;
            AddressRequest request = new AddressRequest() { CountryName = "United States of Hammer" };

            Result<Address> createResult = gateway.Address.Create(customer.Id, request);
            Assert.IsFalse(createResult.IsSuccess());
            Dictionary<String, String> parameters = createResult.Parameters;
            Assert.AreEqual("integration_merchant_id", parameters["merchant_id"]);
            Assert.AreEqual(customer.Id, parameters["customer_id"]);
            Assert.AreEqual("United States of Hammer", parameters["address[country_name]"]);
        }

        [Test]
        public void Create_ReturnsAnErrorResult_ForInconsistentCountry()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;
            AddressRequest request = new AddressRequest()
            {
                CountryName = "United States of America",
                CountryCodeAlpha2 = "BZ"
            };

            Result<Address> createResult = gateway.Address.Create(customer.Id, request);
            Assert.IsFalse(createResult.IsSuccess());
            Assert.AreEqual(
                ValidationErrorCode.ADDRESS_INCONSISTENT_COUNTRY,
                createResult.Errors.ForObject("address").OnField("base")[0].Code
            );
        }
    }
}
