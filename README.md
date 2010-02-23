# Braintree .NET Client Library

The Braintree assembly provides integration access to the Braintree Gateway.

## Dependencies

* none

## Quick Start Example

    using System;
    using Braintree;

    namespace BraintreeExample
    {
        class Program
        {
            static void Main(string[] args)
            {
                var gateway = new BraintreeGateway
                {
                    Environment = Braintree.Environment.SANDBOX,
                    MerchantID = "the_merchant_id",
                    PublicKey = "a_public_key",
                    PrivateKey = "a_private_key"
                };

                var request = new TransactionRequest
                {
                    Amount = 100.00M,
                    CreditCardRequest = new CreditCardRequest
                    {
                        Number = "5105105105105100",
                        ExpirationDate = "05/12"
                    }
                };

                Transaction transaction = gateway.Transaction.Sale(request).Target;

                Console.WriteLine(String.Format("Transaction ID: {0}", transaction.ID));
                Console.WriteLine(String.Format("Status: {0}", transaction.Status));
            }
        }
    }

## License

See the LICENSE file.

