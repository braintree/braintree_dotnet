using Braintree.Exceptions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Braintree.Tests.Integration
{
    [TestFixture]
    public class AddressIntegrationTest
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
                CountryName = "United States of America",
                PhoneNumber = "312-123-4567",
                InternationalPhone = new InternationalPhoneRequest
                {
                    CountryCode = "1",
                    NationalNumber = "3121234567" 
                }
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
            Assert.AreEqual("312-123-4567", address.PhoneNumber);
            Assert.AreEqual("1", address.InternationalPhone.CountryCode);
            Assert.AreEqual("3121234567", address.InternationalPhone.NationalNumber);
            Assert.IsNotNull(address.CreatedAt);
            Assert.IsNotNull(address.UpdatedAt);
        }

        [Test]
#if netcore
        public async Task CreateAsync_CreatesAddressForGivenCustomerId()
#else
        public void CreateAsync_CreatesAddressForGivenCustomerId()
        {
            Task.Run(async() =>
#endif
        {
            Result<Customer> customerResult = await gateway.Customer.CreateAsync(new CustomerRequest());
            Customer customer = customerResult.Target;

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

            Result<Address> addressResult = await gateway.Address.CreateAsync(customer.Id, addressRequest);
            Address address = addressResult.Target;

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
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

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
#if netcore
        public async Task FindAsync_FindsAddress()
#else
        public void FindAsync_FindsAddress()
        {
            Task.Run(async() =>
#endif
        {
            Result<Customer> customerResult = await gateway.Customer.CreateAsync(new CustomerRequest());
            Customer customer = customerResult.Target;

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

            Result<Address> addressResult = await gateway.Address.CreateAsync(customer.Id, addressRequest);
            Address createdAddress = addressResult.Target;
            Address address = await gateway.Address.FindAsync(customer.Id, createdAddress.Id);

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
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

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
                CountryCodeNumeric = "840",
                PhoneNumber = "312-123-4567",
                InternationalPhone = new InternationalPhoneRequest
                {
                    CountryCode = "1",
                    NationalNumber = "3121234567" 
                }
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
            Assert.AreEqual("312-123-4567", address.PhoneNumber);
            Assert.AreEqual("1", address.InternationalPhone.CountryCode);
            Assert.AreEqual("3121234567", address.InternationalPhone.NationalNumber);
        }

        [Test]
#if netcore
        public async Task UpdateAsync_UpdatesAddressForGivenCustomerIdAndAddressId()
#else
        public void UpdateAsync_UpdatesAddressForGivenCustomerIdAndAddressId()
        {
            Task.Run(async() =>
#endif
        {
            Result<Customer> customerResult = await gateway.Customer.CreateAsync(new CustomerRequest());
            Customer customer = customerResult.Target;

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

            Result<Address> addressResult = await gateway.Address.CreateAsync(customer.Id, addressCreateRequest);
            Address originalAddress = addressResult.Target;

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

            addressResult = await gateway.Address.UpdateAsync(customer.Id, originalAddress.Id, addressUpdateRequest);
            Address address = addressResult.Target;

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
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void Update_ReturnsAnErrorResult_ForInconsistentCountry()
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

            Assert.Throws<NotFoundException> (() => gateway.Address.Find(customer.Id, createdAddress.Id));
        }

        [Test]
#if netcore
        public async Task DeleteAsync_DeletesTheAddress()
#else
        public void DeleteAsync_DeletesTheAddress()
        {
            Task.Run(async() =>
#endif
        {
            Result<Customer> customerResult = await gateway.Customer.CreateAsync(new CustomerRequest());
            Customer customer = customerResult.Target;

            var addressRequest = new AddressRequest
            {
                StreetAddress = "1 E Main St",
                ExtendedAddress = "Apt 3",
            };

            Result<Address> addressResult = await gateway.Address.CreateAsync(customer.Id, addressRequest);
            Address createdAddress = addressResult.Target;
            Assert.AreEqual(createdAddress.Id, gateway.Address.Find(customer.Id, createdAddress.Id).Id);

            try {
                Result<Address> result = await gateway.Address.DeleteAsync(customer.Id, createdAddress.Id);
                Assert.IsTrue(result.IsSuccess());
            } catch (NotFoundException) {
                Assert.Fail("Unable to delete the created address");
            }

            Assert.Throws<NotFoundException> (() => gateway.Address.Find(customer.Id, createdAddress.Id));
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
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
