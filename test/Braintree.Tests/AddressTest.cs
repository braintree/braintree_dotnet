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
        [Category("Integration")]
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
        [Category("Integration")]
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
        [Category("Unit")]
        public void Find_FindsErrorsOutOnWhitespaceAddressId()
        {
            Assert.Throws<NotFoundException>(()=> gateway.Address.Find(" ", "address_id"));
        }

        [Test]
        [Category("Unit")]
        public void Find_FindsErrorsOutOnWhitespaceCustomerId()
        {
            Assert.Throws<NotFoundException>(() => gateway.Address.Find("customer_id", " "));
        }

        [Test]
        [Category("Integration")]
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
                CountryName = "Canada",
                CountryCodeAlpha2 = "CA",
                CountryCodeAlpha3 = "CAN",
                CountryCodeNumeric = "124"
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
                CountryName = "United States of America",
                CountryCodeAlpha2 = "US",
                CountryCodeAlpha3 = "USA",
                CountryCodeNumeric = "840"
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
            Assert.AreEqual("US", address.CountryCodeAlpha2);
            Assert.AreEqual("USA", address.CountryCodeAlpha3);
            Assert.AreEqual("840", address.CountryCodeNumeric);
        }

        [Test]
        [Category("Integration")]
        public void Update_ReturnsAnErrorResult_ForInconsistenCountry()
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
                CountryName = "Canada",
                CountryCodeAlpha2 = "CA",
                CountryCodeAlpha3 = "CAN",
                CountryCodeNumeric = "124"
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
                CountryName = "United States of America",
                CountryCodeAlpha3 = "MEX"
            };

            Result<Address> result = gateway.Address.Update(customer.Id, originalAddress.Id, addressUpdateRequest);

            Assert.AreEqual(
                ValidationErrorCode.ADDRESS_INCONSISTENT_COUNTRY,
                result.Errors.ForObject("Address").OnField("Base")[0].Code
            );
        }

        [Test]
        [Category("Integration")]
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

            try {
                Result<Address> result = gateway.Address.Delete(customer.Id, createdAddress.Id);
                Assert.IsTrue(result.IsSuccess());
            } catch (NotFoundException) {
                Assert.Fail("Unable to delete the created address");
            }

            Assert.Throws<NotFoundException>(() => gateway.Address.Find(customer.Id, createdAddress.Id));
        }

        [Test]
        [Category("Integration")]
        public void Create_ReturnsAnErrorResult()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;
            AddressRequest request = new AddressRequest() { CountryName = "United States of Hammer" };

            Result<Address> createResult = gateway.Address.Create(customer.Id, request);
            Assert.IsFalse(createResult.IsSuccess());
            Dictionary<string, string> parameters = createResult.Parameters;
            Assert.AreEqual("integration_merchant_id", parameters["merchant_id"]);
            Assert.AreEqual(customer.Id, parameters["customer_id"]);
            Assert.AreEqual("United States of Hammer", parameters["address[country_name]"]);
        }

        [Test]
        [Category("Integration")]
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
                createResult.Errors.ForObject("Address").OnField("Base")[0].Code
            );
        }

        [Test]
        [Category("Integration")]
        public void Create_ReturnsAnErrorResult_ForIncorrectAlpha2()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;
            AddressRequest request = new AddressRequest()
            {
                CountryCodeAlpha2 = "ZZ"
            };

            Result<Address> createResult = gateway.Address.Create(customer.Id, request);
            Assert.IsFalse(createResult.IsSuccess());
            Assert.AreEqual(
                ValidationErrorCode.ADDRESS_COUNTRY_CODE_ALPHA2_IS_NOT_ACCEPTED,
                createResult.Errors.ForObject("Address").OnField("CountryCodeAlpha2")[0].Code
            );
        }

        [Test]
        [Category("Integration")]
        public void Create_ReturnsAnErrorResult_ForIncorrectAlpha3()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;
            AddressRequest request = new AddressRequest()
            {
                CountryCodeAlpha3 = "ZZZ"
            };

            Result<Address> createResult = gateway.Address.Create(customer.Id, request);
            Assert.IsFalse(createResult.IsSuccess());
            Assert.AreEqual(
                ValidationErrorCode.ADDRESS_COUNTRY_CODE_ALPHA3_IS_NOT_ACCEPTED,
                createResult.Errors.ForObject("Address").OnField("CountryCodeAlpha3")[0].Code
            );
        }

        [Test]
        [Category("Integration")]
        public void Create_ReturnsAnErrorResult_ForIncorrectNumeric()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;
            AddressRequest request = new AddressRequest()
            {
                CountryCodeNumeric = "000"
            };

            Result<Address> createResult = gateway.Address.Create(customer.Id, request);
            Assert.IsFalse(createResult.IsSuccess());
            Assert.AreEqual(
                ValidationErrorCode.ADDRESS_COUNTRY_CODE_NUMERIC_IS_NOT_ACCEPTED,
                createResult.Errors.ForObject("Address").OnField("CountryCodeNumeric")[0].Code
            );
        }
    }
}
