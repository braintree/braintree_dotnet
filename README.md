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
                    MerchantId = "the_merchant_id",
                    PublicKey = "a_public_key",
                    PrivateKey = "a_private_key"
                };

                TransactionRequest request = new TransactionRequest
                {
                    Amount = 1000M,
                    CreditCard = new CreditCardRequest
                    {
                        Number = "4111111111111111",
                        ExpirationDate = "05/2012"
                    }
                };

                Result<Transaction> result = gateway.Transaction.Sale(request);

                if (result.IsSuccess())
                {
                    Transaction transaction = result.Target;
                    Console.WriteLine("Success!: " + transaction.Id);
                }
                else if (result.Transaction != null)
                {
                    Transaction transaction = result.Transaction;
                    Console.WriteLine("Error processing transaction:");
                    Console.WriteLine("  Status: " + transaction.Status);
                    Console.WriteLine("  Code: " + transaction.ProcessorResponseCode);
                    Console.WriteLine("  Text: " + transaction.ProcessorResponseText);
                }
                else
                {
                    foreach (ValidationError error in result.Errors.DeepAll())
                    {
                        Console.WriteLine("Attribute: " + error.Attribute);
                        Console.WriteLine("  Code: " + error.Code);
                        Console.WriteLine("  Message: " + error.Message);
                    }
                }
            }
        }
    }

## License

See the LICENSE file.

