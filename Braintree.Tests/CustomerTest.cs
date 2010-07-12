using System;
using System.Collections.Generic;
using NUnit.Framework;
using Braintree.Exceptions;

namespace Braintree.Tests
{
    [TestFixture]
    public class CustomerTest
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
        public void Find_FindsCustomerWithGivenId()
        {
            String id = Guid.NewGuid().ToString();
            var createRequest = new CustomerRequest
            {
                Id = id,
                FirstName = "Michael",
                LastName = "Angelo",
                Company = "Some Company",
                Email = "hansolo64@compuserver.com",
                Phone = "312.555.1111",
                Fax = "312.555.1112",
                Website = "www.disney.com",
                CreditCard = new CreditCardRequest
                {
                    Number = "5105105105105100",
                    ExpirationDate = "05/12",
                    CVV = "123",
                    CardholderName = "Michael Angelo",
                    BillingAddress = new CreditCardAddressRequest()
                    {
                        FirstName = "Mike",
                        LastName = "Smith",
                        Company = "Smith Co.",
                        StreetAddress = "1 W Main St",
                        ExtendedAddress = "Suite 330",
                        Locality = "Chicago",
                        Region = "IL",
                        PostalCode = "60622",
                        CountryName = "United States of America"
                    }
                }
            };

            Customer createdCustomer = gateway.Customer.Create(createRequest).Target;
            Customer customer = gateway.Customer.Find(createdCustomer.Id);
            Assert.AreEqual(id, customer.Id);
            Assert.AreEqual("Michael", customer.FirstName);
            Assert.AreEqual("Angelo", customer.LastName);
            Assert.AreEqual("Some Company", customer.Company);
            Assert.AreEqual("hansolo64@compuserver.com", customer.Email);
            Assert.AreEqual("312.555.1111", customer.Phone);
            Assert.AreEqual("312.555.1112", customer.Fax);
            Assert.AreEqual("www.disney.com", customer.Website);
            Assert.AreEqual(DateTime.Now.Year, customer.CreatedAt.Value.Year);
            Assert.AreEqual(DateTime.Now.Year, customer.UpdatedAt.Value.Year);
            Assert.AreEqual(1, customer.CreditCards.Length);
            Assert.AreEqual("510510", customer.CreditCards[0].Bin);
            Assert.AreEqual("5100", customer.CreditCards[0].LastFour);
            Assert.AreEqual("05", customer.CreditCards[0].ExpirationMonth);
            Assert.AreEqual("2012", customer.CreditCards[0].ExpirationYear);
            Assert.AreEqual("Michael Angelo", customer.CreditCards[0].CardholderName);
            Assert.AreEqual(DateTime.Now.Year, customer.CreditCards[0].CreatedAt.Value.Year);
            Assert.AreEqual(DateTime.Now.Year, customer.CreditCards[0].UpdatedAt.Value.Year);
            Assert.AreEqual("Mike", customer.Addresses[0].FirstName);
            Assert.AreEqual("Smith", customer.Addresses[0].LastName);
            Assert.AreEqual("Smith Co.", customer.Addresses[0].Company);
            Assert.AreEqual("1 W Main St", customer.Addresses[0].StreetAddress);
            Assert.AreEqual("Suite 330", customer.Addresses[0].ExtendedAddress);
            Assert.AreEqual("Chicago", customer.Addresses[0].Locality);
            Assert.AreEqual("IL", customer.Addresses[0].Region);
            Assert.AreEqual("60622", customer.Addresses[0].PostalCode);
            Assert.AreEqual("United States of America", customer.Addresses[0].CountryName);
        }

        [Test]
        public void Find_RaisesIfIdIsInvalid()
        {
            try
            {
                gateway.Customer.Find("DOES_NOT_EXIST_999");
                Assert.Fail("Expected NotFoundException.");
            }
            catch (NotFoundException)
            {
                // expected
            }
        }

        [Test]
        public void Create_CanSetCustomFields()
        {
            var customFields = new Dictionary<String, String>();
            customFields.Add("store_me", "a custom value");
            var createRequest = new CustomerRequest()
            {
                CustomFields = customFields
            };

            Customer customer = gateway.Customer.Create(createRequest).Target;
            Assert.AreEqual("a custom value", customer.CustomFields["store_me"]);
        }

        [Test]
        public void Create_CreatesCustomerWithSpecifiedValues()
        {
            var createRequest = new CustomerRequest()
            {
                FirstName = "Michael",
                LastName = "Angelo",
                Company = "Some Company",
                Email = "hansolo64@compuserver.com",
                Phone = "312.555.1111",
                Fax = "312.555.1112",
                Website = "www.disney.com",
                CreditCard = new CreditCardRequest()
                {
                    Number = "5105105105105100",
                    ExpirationDate = "05/12",
                    BillingAddress = new CreditCardAddressRequest
                    {
                        CountryName = "Macau",
                        CountryCodeAlpha2 = "MO",
                        CountryCodeAlpha3 = "MAC",
                        CountryCodeNumeric = "446"
                    }
                }
            };

            Customer customer = gateway.Customer.Create(createRequest).Target;
            Assert.AreEqual("Michael", customer.FirstName);
            Assert.AreEqual("Angelo", customer.LastName);
            Assert.AreEqual("Some Company", customer.Company);
            Assert.AreEqual("hansolo64@compuserver.com", customer.Email);
            Assert.AreEqual("312.555.1111", customer.Phone);
            Assert.AreEqual("312.555.1112", customer.Fax);
            Assert.AreEqual("www.disney.com", customer.Website);
            Assert.AreEqual(DateTime.Now.Year, customer.CreatedAt.Value.Year);
            Assert.AreEqual(DateTime.Now.Year, customer.UpdatedAt.Value.Year);

            Address billingAddress = customer.CreditCards[0].BillingAddress;
            Assert.AreEqual("Macau", billingAddress.CountryName);
            Assert.AreEqual("MO", billingAddress.CountryCodeAlpha2);
            Assert.AreEqual("MAC", billingAddress.CountryCodeAlpha3);
            Assert.AreEqual("446", billingAddress.CountryCodeNumeric);
        }



        [Test]
        public void Create_withErrorsOnCountry()
        {
            var createRequest = new CustomerRequest()
            {
                FirstName = "Michael",
                LastName = "Angelo",
                Company = "Some Company",
                Email = "hansolo64@compuserver.com",
                Phone = "312.555.1111",
                Fax = "312.555.1112",
                Website = "www.disney.com",
                CreditCard = new CreditCardRequest()
                {
                    Number = "5105105105105100",
                    ExpirationDate = "05/12",
                    BillingAddress = new CreditCardAddressRequest
                    {
                        CountryName = "zzzzzz",
                        CountryCodeAlpha2 = "zz",
                        CountryCodeAlpha3 = "zzz",
                        CountryCodeNumeric = "000"
                    }
                }
            };

            Result<Customer> result = gateway.Customer.Create(createRequest);

            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(
                ValidationErrorCode.ADDRESS_COUNTRY_NAME_IS_NOT_ACCEPTED,
                result.Errors.ForObject("customer").ForObject("credit-card").ForObject("billing-address").OnField("country_name")[0].Code
            );
            Assert.AreEqual(
                ValidationErrorCode.ADDRESS_COUNTRY_CODE_ALPHA2_IS_NOT_ACCEPTED,
                result.Errors.ForObject("customer").ForObject("credit-card").ForObject("billing-address").OnField("country_code_alpha2")[0].Code
            );
            Assert.AreEqual(
                ValidationErrorCode.ADDRESS_COUNTRY_CODE_ALPHA3_IS_NOT_ACCEPTED,
                result.Errors.ForObject("customer").ForObject("credit-card").ForObject("billing-address").OnField("country_code_alpha3")[0].Code
            );
            Assert.AreEqual(
                ValidationErrorCode.ADDRESS_COUNTRY_CODE_NUMERIC_IS_NOT_ACCEPTED,
                result.Errors.ForObject("customer").ForObject("credit-card").ForObject("billing-address").OnField("country_code_numeric")[0].Code
            );
        }

        [Test]
        public void Create_CreateCustomerWithCreditCard()
        {
            var createRequest = new CustomerRequest()
            {
                FirstName = "Michael",
                LastName = "Angelo",
                Company = "Some Company",
                Email = "hansolo64@compuserver.com",
                Phone = "312.555.1111",
                Fax = "312.555.1112",
                Website = "www.disney.com",
                CreditCard = new CreditCardRequest()
                {
                    Number = "5105105105105100",
                    ExpirationDate = "05/12",
                    CVV = "123",
                    CardholderName = "Michael Angelo"
                }
            };

            Customer customer = gateway.Customer.Create(createRequest).Target;
            Assert.AreEqual("Michael", customer.FirstName);
            Assert.AreEqual("Angelo", customer.LastName);
            Assert.AreEqual("Some Company", customer.Company);
            Assert.AreEqual("hansolo64@compuserver.com", customer.Email);
            Assert.AreEqual("312.555.1111", customer.Phone);
            Assert.AreEqual("312.555.1112", customer.Fax);
            Assert.AreEqual("www.disney.com", customer.Website);
            Assert.AreEqual(DateTime.Now.Year, customer.CreatedAt.Value.Year);
            Assert.AreEqual(DateTime.Now.Year, customer.UpdatedAt.Value.Year);
            Assert.AreEqual(1, customer.CreditCards.Length);
            Assert.AreEqual("510510", customer.CreditCards[0].Bin);
            Assert.AreEqual("5100", customer.CreditCards[0].LastFour);
            Assert.AreEqual("05", customer.CreditCards[0].ExpirationMonth);
            Assert.AreEqual("2012", customer.CreditCards[0].ExpirationYear);
            Assert.AreEqual("Michael Angelo", customer.CreditCards[0].CardholderName);
            Assert.AreEqual(DateTime.Now.Year, customer.CreditCards[0].CreatedAt.Value.Year);
            Assert.AreEqual(DateTime.Now.Year, customer.CreditCards[0].UpdatedAt.Value.Year);
        }

        [Test]
        public void Create_CreateCustomerWithCreditCardAndBillingAddress()
        {
            var createRequest = new CustomerRequest()
            {
                FirstName = "Michael",
                LastName = "Angelo",
                Company = "Some Company",
                Email = "hansolo64@compuserver.com",
                Phone = "312.555.1111",
                Fax = "312.555.1112",
                Website = "www.disney.com",
                CreditCard = new CreditCardRequest()
                {
                    Number = "5105105105105100",
                    ExpirationDate = "05/12",
                    CVV = "123",
                    CardholderName = "Michael Angelo",
                    BillingAddress = new CreditCardAddressRequest()
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
                    }
                }
            };

            Customer customer = gateway.Customer.Create(createRequest).Target;
            Assert.AreEqual("Michael", customer.FirstName);
            Assert.AreEqual("Angelo", customer.LastName);
            Assert.AreEqual("Some Company", customer.Company);
            Assert.AreEqual("hansolo64@compuserver.com", customer.Email);
            Assert.AreEqual("312.555.1111", customer.Phone);
            Assert.AreEqual("312.555.1112", customer.Fax);
            Assert.AreEqual("www.disney.com", customer.Website);
            Assert.AreEqual(DateTime.Now.Year, customer.CreatedAt.Value.Year);
            Assert.AreEqual(DateTime.Now.Year, customer.UpdatedAt.Value.Year);
            Assert.AreEqual(1, customer.CreditCards.Length);
            Assert.AreEqual("510510", customer.CreditCards[0].Bin);
            Assert.AreEqual("5100", customer.CreditCards[0].LastFour);
            Assert.AreEqual("05", customer.CreditCards[0].ExpirationMonth);
            Assert.AreEqual("2012", customer.CreditCards[0].ExpirationYear);
            Assert.AreEqual("Michael Angelo", customer.CreditCards[0].CardholderName);
            Assert.AreEqual(DateTime.Now.Year, customer.CreditCards[0].CreatedAt.Value.Year);
            Assert.AreEqual(DateTime.Now.Year, customer.CreditCards[0].UpdatedAt.Value.Year);
            Assert.AreEqual(customer.Addresses[0].Id, customer.CreditCards[0].BillingAddress.Id);
            Assert.AreEqual("Michael", customer.Addresses[0].FirstName);
            Assert.AreEqual("Angelo", customer.Addresses[0].LastName);
            Assert.AreEqual("Angelo Co.", customer.Addresses[0].Company);
            Assert.AreEqual("1 E Main St", customer.Addresses[0].StreetAddress);
            Assert.AreEqual("Apt 3", customer.Addresses[0].ExtendedAddress);
            Assert.AreEqual("Chicago", customer.Addresses[0].Locality);
            Assert.AreEqual("IL", customer.Addresses[0].Region);
            Assert.AreEqual("60622", customer.Addresses[0].PostalCode);
            Assert.AreEqual("United States of America", customer.Addresses[0].CountryName);
        }

        [Test]
        public void ConfirmTransparentRedirect_CreatesTheCustomer()
        {
            CustomerRequest trParams = new CustomerRequest();

            CustomerRequest request = new CustomerRequest
            {
                FirstName = "John",
                LastName = "Doe"
            };

            String queryString = TestHelper.QueryStringForTR(trParams, request, gateway.Customer.TransparentRedirectURLForCreate(), service);
            Result<Customer> result = gateway.Customer.ConfirmTransparentRedirect(queryString);
            Assert.IsTrue(result.IsSuccess());
            Customer customer = result.Target;
            Assert.AreEqual("John", customer.FirstName);
            Assert.AreEqual("Doe", customer.LastName);
        }

        [Test]
        public void ConfirmTransparentRedirect_CreatesNestedElementsAndCustomFields()
        {
            CustomerRequest trParams = new CustomerRequest();

            CustomerRequest request = new CustomerRequest
            {
                FirstName = "John",
                LastName = "Doe",
                CreditCard = new CreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    CardholderName = "John Doe",
                    ExpirationDate = "05/10",
                    BillingAddress = new CreditCardAddressRequest
                    {
                        CountryName = "Mexico",
                        CountryCodeAlpha2 = "MX",
                        CountryCodeAlpha3 = "MEX",
                        CountryCodeNumeric = "484"
                    }
                },
                CustomFields = new Dictionary<String, String>
                {
                    { "store_me", "a custom value" }
                }
            };

            String queryString = TestHelper.QueryStringForTR(trParams, request, gateway.Customer.TransparentRedirectURLForCreate(), service);
            Result<Customer> result = gateway.Customer.ConfirmTransparentRedirect(queryString);
            Assert.IsTrue(result.IsSuccess());
            Customer customer = result.Target;
            Assert.AreEqual("John", customer.FirstName);
            Assert.AreEqual("Doe", customer.LastName);
            Assert.AreEqual("John Doe", customer.CreditCards[0].CardholderName);
            Assert.AreEqual("a custom value", customer.CustomFields["store_me"]);

            Address address = customer.CreditCards[0].BillingAddress;
            Assert.AreEqual("Mexico", address.CountryName);
            Assert.AreEqual("MX", address.CountryCodeAlpha2);
            Assert.AreEqual("MEX", address.CountryCodeAlpha3);
            Assert.AreEqual("484", address.CountryCodeNumeric);
        }

        [Test]
        public void ConfirmTransparentRedirect_UpdatesTheCustomer()
        {
            CustomerRequest createRequest = new CustomerRequest
            {
                FirstName = "Jane",
                LastName = "Deer"
            };
            Customer createdCustomer = gateway.Customer.Create(createRequest).Target;

            CustomerRequest trParams = new CustomerRequest
            {
                CustomerId = createdCustomer.Id
            };

            CustomerRequest request = new CustomerRequest
            {
                FirstName = "John",
                LastName = "Doe"
            };

            String queryString = TestHelper.QueryStringForTR(trParams, request, gateway.Customer.TransparentRedirectURLForUpdate(), service);
            Result<Customer> result = gateway.Customer.ConfirmTransparentRedirect(queryString);
            Assert.IsTrue(result.IsSuccess());
            Customer customer = result.Target;
            Assert.AreEqual("John", customer.FirstName);
            Assert.AreEqual("Doe", customer.LastName);
        }

        [Test]
        public void Update_UpdatesCustomerAndNestedValuesViaTr()
        {
            var createRequest = new CustomerRequest()
            {
                FirstName = "Old First",
                LastName = "Old Last",
                CreditCard = new CreditCardRequest()
                {
                    Number = "4111111111111111",
                    ExpirationDate = "10/10",
                    BillingAddress = new CreditCardAddressRequest()
                    {
                        PostalCode = "11111"
                    }
                }
            };

            Customer customer = gateway.Customer.Create(createRequest).Target;
            CreditCard creditCard = customer.CreditCards[0];
            Address address = creditCard.BillingAddress;

            var trParams = new CustomerRequest()
            {
                CustomerId = customer.Id,
                FirstName = "New First",
                LastName = "New Last",
                CreditCard = new CreditCardRequest()
                {
                    ExpirationDate = "12/12",
                    Options = new CreditCardOptionsRequest()
                    {
                        UpdateExistingToken = creditCard.Token
                    },
                    BillingAddress = new CreditCardAddressRequest()
                    {
                        PostalCode = "44444",
                        CountryName = "Chad",
                        CountryCodeAlpha2 = "TD",
                        CountryCodeAlpha3 = "TCD",
                        CountryCodeNumeric = "148",
                        Options = new CreditCardAddressOptionsRequest()
                        {
                            UpdateExisting = true
                        }
                    }
                }
            };

            String queryString = TestHelper.QueryStringForTR(trParams, new CustomerRequest(), gateway.Customer.TransparentRedirectURLForUpdate(), service);
            Customer updatedCustomer = gateway.Customer.ConfirmTransparentRedirect(queryString).Target;
            CreditCard updatedCreditCard = gateway.CreditCard.Find(creditCard.Token);

            Address updatedAddress = gateway.Address.Find(customer.Id, address.Id);

            Assert.AreEqual("New First", updatedCustomer.FirstName);
            Assert.AreEqual("New Last", updatedCustomer.LastName);
            Assert.AreEqual("12/2012", updatedCreditCard.ExpirationDate);
            Assert.AreEqual("44444", updatedAddress.PostalCode);
            Assert.AreEqual("Chad", updatedAddress.CountryName);
            Assert.AreEqual("TD", updatedAddress.CountryCodeAlpha2);
            Assert.AreEqual("TCD", updatedAddress.CountryCodeAlpha3);
            Assert.AreEqual("148", updatedAddress.CountryCodeNumeric);
        }

        [Test]
        public void Update_UpdatesCustomerWithNewValues()
        {
            string oldId = Guid.NewGuid().ToString();
            string newId = Guid.NewGuid().ToString();
            var createRequest = new CustomerRequest()
            {
                Id = oldId,
                FirstName = "Old First",
                LastName = "Old Last",
                Company = "Old Company",
                Email = "old@example.com",
                Phone = "312.555.1111 xOld",
                Fax = "312.555.1112 xOld",
                Website = "old.example.com"
            };

            gateway.Customer.Create(createRequest);

            var updateRequest = new CustomerRequest()
            {
                Id = newId,
                FirstName = "Michael",
                LastName = "Angelo",
                Company = "Some Company",
                Email = "hansolo64@compuserver.com",
                Phone = "312.555.1111",
                Fax = "312.555.1112",
                Website = "www.disney.com"
            };

            Customer updatedCustomer = gateway.Customer.Update(oldId, updateRequest).Target;
            Assert.AreEqual(newId, updatedCustomer.Id);
            Assert.AreEqual("Michael", updatedCustomer.FirstName);
            Assert.AreEqual("Angelo", updatedCustomer.LastName);
            Assert.AreEqual("Some Company", updatedCustomer.Company);
            Assert.AreEqual("hansolo64@compuserver.com", updatedCustomer.Email);
            Assert.AreEqual("312.555.1111", updatedCustomer.Phone);
            Assert.AreEqual("312.555.1112", updatedCustomer.Fax);
            Assert.AreEqual("www.disney.com", updatedCustomer.Website);
            Assert.AreEqual(DateTime.Now.Year, updatedCustomer.CreatedAt.Value.Year);
            Assert.AreEqual(DateTime.Now.Year, updatedCustomer.UpdatedAt.Value.Year);
        }

        [Test]
        public void Update_UpdatesCustomerAndNestedValues()
        {
            var createRequest = new CustomerRequest()
            {
                FirstName = "Old First",
                LastName = "Old Last",
                CreditCard = new CreditCardRequest()
                {
                    Number = "4111111111111111",
                    ExpirationDate = "10/10",
                    BillingAddress = new CreditCardAddressRequest()
                    {
                        PostalCode = "11111"
                    }
                }
            };

            Customer customer = gateway.Customer.Create(createRequest).Target;
            CreditCard creditCard = customer.CreditCards[0];
            Address address = creditCard.BillingAddress;

            var updateRequest = new CustomerRequest()
            {
                FirstName = "New First",
                LastName = "New Last",
                CreditCard = new CreditCardRequest()
                {
                    ExpirationDate = "12/12",
                    Options = new CreditCardOptionsRequest()
                    {
                        UpdateExistingToken = creditCard.Token
                    },
                    BillingAddress = new CreditCardAddressRequest()
                    {
                        PostalCode = "44444",
                        CountryName = "Chad",
                        CountryCodeAlpha2 = "TD",
                        CountryCodeAlpha3 = "TCD",
                        CountryCodeNumeric = "148",
                        Options = new CreditCardAddressOptionsRequest()
                        {
                            UpdateExisting = true
                        }
                    }
                }
            };

            Customer updatedCustomer = gateway.Customer.Update(customer.Id, updateRequest).Target;
            CreditCard updatedCreditCard = gateway.CreditCard.Find(creditCard.Token);
            Address updatedAddress = gateway.Address.Find(customer.Id, address.Id);

            Assert.AreEqual("New First", updatedCustomer.FirstName);
            Assert.AreEqual("New Last", updatedCustomer.LastName);
            Assert.AreEqual("12/2012", updatedCreditCard.ExpirationDate);
            Assert.AreEqual("44444", updatedAddress.PostalCode);
            Assert.AreEqual("Chad", updatedAddress.CountryName);
            Assert.AreEqual("TD", updatedAddress.CountryCodeAlpha2);
            Assert.AreEqual("TCD", updatedAddress.CountryCodeAlpha3);
            Assert.AreEqual("148", updatedAddress.CountryCodeNumeric);
        }

        [Test]
        public void Delete_DeletesTheCustomer()
        {
            String id = Guid.NewGuid().ToString();
            gateway.Customer.Create(new CustomerRequest() { Id = id });
            Assert.AreEqual(id, gateway.Customer.Find(id).Id);
            gateway.Customer.Delete(id);

            try
            {
                gateway.Customer.Find(id);
                Assert.Fail("Expected NotFoundException.");
            }
            catch (NotFoundException)
            {
                // expected
            }
        }

        [Test]
        public void All() {
            ResourceCollection<Customer> collection = gateway.Customer.All();

            Assert.IsTrue(collection.MaximumCount > 100);
    
            List<String> items = new List<String>();
            foreach (Customer item in collection) {
                items.Add(item.Id);
            }
            HashSet<String> uniqueItems = new HashSet<String>(items);

            Assert.AreEqual(uniqueItems.Count, collection.MaximumCount);
        }
    }
}
