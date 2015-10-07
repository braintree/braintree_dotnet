# Braintree .NET Client Library [![Build status](https://ci.appveyor.com/api/projects/status/03h2gcx7m0h80o0o/branch/master?svg=true)](https://ci.appveyor.com/project/ronin1/braintree-dotnet/branch/master)

The Braintree assembly provides integration access to the Braintree Gateway.

## Dependencies

* none

## Quick Start Example

```csharp
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
                Amount = 1000.00M,
                PaymentMethodNonce = nonceFromTheClient,
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
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
```

## Documentation

* [Official documentation](https://developers.braintreepayments.com/dotnet/sdk/server/overview)
* For *bleeding edge* pre-releases, add the following nuget path: https://ci.appveyor.com/nuget/braintree-dotnet

## Open Source Attribution

A list of open source projects that help power Braintree can be found [here](https://www.braintreepayments.com/developers/open-source).

## License

See the LICENSE file.
