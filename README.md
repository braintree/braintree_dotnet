# Braintree .NET library

The Braintree .NET library provides integration access to the Braintree Gateway.

## Please Note
> **The Payment Card Industry (PCI) Council has [mandated](https://blog.pcisecuritystandards.org/migrating-from-ssl-and-early-tls) that early versions of TLS be retired from service.  All organizations that handle credit card information are required to comply with this standard. As part of this obligation, Braintree is updating its services to require TLS 1.2 for all HTTPS connections. Braintree will also require HTTP/1.1 for all connections. Please see our [technical documentation](https://github.com/paypal/tls-update) for more information.**

## Dependencies

* The Braintree .NET library targets Net Framework 4.5.2 and Net Standard 2.0. It is tested against .NET Framework 4.5.2 (via Mono) and NET Core 3.1.

## Versions

Braintree employs a deprecation policy for our SDKs. For more information on the statuses of an SDK check our [developer docs](https://developer.paypal.com/braintree/docs/reference/general/server-sdk-deprecation-policy).

| Major version number | Status      | Released        | Deprecated   | Unsupported  |
| -------------------- | ----------- | --------------- | ------------ | ------------ |
| 5.x.x                | Active      | August 2020     | TBA          | TBA          |
| 4.x.x                | Inactive    | March 2018      | August 2022  | August 2023  |

## Documentation

* [Official documentation](https://developer.paypal.com/braintree/docs/start/hello-server/dotnet)

Updating from an Inactive, Deprecated, or Unsupported version of this SDK? Check our [Migration Guide](https://developer.paypal.com/braintree/docs/reference/general/server-sdk-migration-guide/dotnet) for tips.

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

## Developing and Tests

See [DEVELOPMENT.md](DEVELOPMENT.md).

## License

See the LICENSE file.
