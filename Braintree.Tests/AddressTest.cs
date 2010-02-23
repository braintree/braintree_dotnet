using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Braintree;

namespace Braintree.Tests
{
    [TestFixture]
    class AddressTest
    {
        private BraintreeGateway gateway;

        [SetUp]
        public void Setup()
        {
            gateway = new BraintreeGateway
            {
                Environment = Environment.DEVELOPMENT,
                MerchantID = "integration_merchant_id",
                PublicKey = "integration_public_key",
                PrivateKey = "integration_private_key"
            };
        }

        [Test]
        public void Create_CreatesAddressForGivenCustomerID()
        {
            String id = Guid.NewGuid().ToString();
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;

            var addressRequest = new AddressRequest
            {
                CustomerID = customer.ID,
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

            Address address = gateway.Address.Create(customer.ID, addressRequest).Target;

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
        public void Find_FindsAddress()
        {
            String id = Guid.NewGuid().ToString();
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;

            var addressRequest = new AddressRequest
            {
                CustomerID = customer.ID,
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

            Address createdAddress = gateway.Address.Create(customer.ID, addressRequest).Target;
            Address address = gateway.Address.Find(customer.ID, createdAddress.ID).Target;

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
        public void Update_UpdatesAddressForGivenCustomerIDAndAddressID()
        {
            String id = Guid.NewGuid().ToString();
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;

            var addressCreateRequest = new AddressRequest
            {
                CustomerID = customer.ID,
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

            Address originalAddress = gateway.Address.Create(customer.ID, addressCreateRequest).Target;

            var addressUpdateRequest = new AddressRequest
            {
                CustomerID = customer.ID,
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

            Address address = gateway.Address.Update(customer.ID, originalAddress.ID, addressUpdateRequest).Target;

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
                CustomerID = customer.ID,
                StreetAddress = "1 E Main St",
                ExtendedAddress = "Apt 3",
            };

            Address createdAddress = gateway.Address.Create(customer.ID, addressRequest).Target;
            Assert.AreEqual(createdAddress.ID, gateway.Address.Find(customer.ID, createdAddress.ID).Target.ID);
            gateway.Address.Delete(customer.ID, createdAddress.ID);
            Assert.Throws<NotFoundError>(() => gateway.Address.Find(customer.ID, createdAddress.ID));
        }

        [Test]
        public void Create_ReturnsAnErrorResult()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;
            AddressRequest request = new AddressRequest() { CountryName = "United States of Hammer" };

            Result<Address> createResult = gateway.Address.Create(customer.ID, request);
            Assert.IsFalse(createResult.IsSuccess());
            Dictionary<String, String> parameters = createResult.Parameters;
            Assert.AreEqual("integration_merchant_id", parameters["merchant_id"]);
            Assert.AreEqual(customer.ID, parameters["customer_id"]);
            Assert.AreEqual("United States of Hammer", parameters["address[country_name]"]);
        }
    }
}
