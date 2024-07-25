using Braintree.Test;
using Braintree.TestUtil;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Braintree.Tests.Integration
{
    [TestFixture]
    public class PackageTrackingIntegrationTest
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
        public void PackageTracking_Works()
        {
            // Create PayPal Transaction
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                PayPalAccount = new TransactionPayPalRequest()
                {
                    PaymentId = "fake-payment-id",
                    PayerId = "fake-payer-id"
                }
            };
            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());

            Transaction transaction = result.Target;

            // Create First Package with 2 products
            var lineItems = new TransactionLineItemRequest[2]
            {
                new TransactionLineItemRequest
                {
                    Quantity = 1,
                    Name = "Best Product Ever",
                    ProductCode = "ABC 01",
                    Description = "Best Description Ever"
                },
                new TransactionLineItemRequest
                {
                    Quantity = 1,
                    Name = "Best Product Ever",
                    ProductCode = "ABC 02",
                    Description = "Best Description Ever"
                },
            };
            var firstRequest = new PackageTrackingRequest
            {
                Carrier = "UPS",
                TrackingNumber = "tracking_number_1",
                NotifyPayer = false,
                LineItems = lineItems
            };

            // First package is shipped by the merchant
            Result<Transaction> firstPackageResult = gateway.Transaction.PackageTracking(transaction.Id, firstRequest);
            Assert.IsTrue(firstPackageResult.IsSuccess());

            Transaction txnWithFirstPackageTracking = firstPackageResult.Target;
            Assert.IsNotNull(txnWithFirstPackageTracking.Packages[0].Id);
            Assert.AreEqual(txnWithFirstPackageTracking.Packages[0].Carrier, "UPS");
            Assert.AreEqual(txnWithFirstPackageTracking.Packages[0].TrackingNumber, "tracking_number_1");
            Assert.IsNull(txnWithFirstPackageTracking.Packages[0].PaypalTrackerId);
            // NEXT_MAJOR_VERSION Remove PaypalTrackingId assertion
            Assert.IsNull(txnWithFirstPackageTracking.Packages[0].PaypalTrackingId);

            // Create second package with 1 product
            var secondRequest = new PackageTrackingRequest
            {
                Carrier = "FEDEX",
                TrackingNumber = "tracking_number_2",
                NotifyPayer = false,
                LineItems = new TransactionLineItemRequest[]
                {
                    new TransactionLineItemRequest
                    {
                        Quantity = 1,
                        Name = "Best Product Ever",
                        ProductCode = "ABC 03",
                        Description = "Best Description Ever"
                    }
                }
            };

            // Second package is shipped by the merchant
            Result<Transaction> secondPackageResult = gateway.Transaction.PackageTracking(transaction.Id, secondRequest);
            Assert.IsTrue(secondPackageResult.IsSuccess());

            Transaction txnWithSecondPackageTracking = secondPackageResult.Target;
            Assert.IsNotNull(txnWithSecondPackageTracking.Packages[1].Id);
            Assert.AreEqual(txnWithSecondPackageTracking.Packages[1].Carrier, "FEDEX");
            Assert.AreEqual(txnWithSecondPackageTracking.Packages[1].TrackingNumber, "tracking_number_2");
            Assert.IsNull(txnWithSecondPackageTracking.Packages[1].PaypalTrackerId);
            // NEXT_MAJOR_VERSION Remove PaypalTrackingId assertion
            Assert.IsNull(txnWithSecondPackageTracking.Packages[1].PaypalTrackingId);

            // Find transction gives both pckges
            Transaction findTransaction = gateway.Transaction.Find(transaction.Id);
            Assert.AreEqual(findTransaction.Packages.Length, 2);

            Assert.IsNotNull(findTransaction.Packages[0].Id);
            Assert.AreEqual(findTransaction.Packages[0].Carrier, "UPS");
            Assert.AreEqual(findTransaction.Packages[0].TrackingNumber, "tracking_number_1");
            Assert.IsNull(findTransaction.Packages[0].PaypalTrackerId);
            // NEXT_MAJOR_VERSION Remove PaypalTrackingId assertion
            Assert.IsNull(findTransaction.Packages[0].PaypalTrackingId);

            Assert.IsNotNull(findTransaction.Packages[1].Id);
            Assert.AreEqual(findTransaction.Packages[1].Carrier, "FEDEX");
            Assert.AreEqual(findTransaction.Packages[1].TrackingNumber, "tracking_number_2");
            Assert.IsNull(findTransaction.Packages[1].PaypalTrackerId);
            // NEXT_MAJOR_VERSION Remove PaypalTrackingId assertion
            Assert.IsNull(findTransaction.Packages[1].PaypalTrackingId);
        }

        [Test]
        public void PackageTracking_Retrieve_Works()
        {
            // Find transction with previously created packages
            Transaction findTransaction = gateway.Transaction.Find("package_tracking_tx");
            Assert.AreEqual(findTransaction.Packages.Length, 2);

            // NEXT_MAJOR_VERSION Remove PaypalTrackingId assertion
            Assert.IsNull(findTransaction.Packages[0].PaypalTrackingId);
            Assert.AreEqual(findTransaction.Packages[0].PaypalTrackerId, "paypal_tracker_id_1");

            // NEXT_MAJOR_VERSION Remove PaypalTrackingId assertion
            Assert.IsNull(findTransaction.Packages[1].PaypalTrackingId);
            Assert.AreEqual(findTransaction.Packages[1].PaypalTrackerId, "paypal_tracker_id_2");
        }

        [Test]
        public void PackageTracking_ValidationError()
        {
            // Create PayPal Transaction
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                PayPalAccount = new TransactionPayPalRequest()
                {
                    PaymentId = "fake-payment-id",
                    PayerId = "fake-payer-id"
                }
            };
            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());

            Transaction transaction = result.Target;

            // Tracking Number is absent
            var invalidRequest = new PackageTrackingRequest
            {
                Carrier = "UPS",
                NotifyPayer = false,
            };

            Result<Transaction> invalidResult = gateway.Transaction.PackageTracking(transaction.Id, invalidRequest);
            Assert.IsFalse(invalidResult.IsSuccess());
            Assert.AreEqual("Tracking number is required.", invalidResult.Message);

            // Carrier is absent
            invalidRequest = new PackageTrackingRequest
            {
                TrackingNumber = "tracking_number_1",
            };

            invalidResult = gateway.Transaction.PackageTracking(transaction.Id, invalidRequest);
            Assert.IsFalse(invalidResult.IsSuccess());
            Assert.AreEqual("Carrier name is required.", invalidResult.Message);
        }

        [Test]
#if netcore
        public async Task PackageTrackingAsync_Works()
#else
        public void PackageTrackingAsync_Works()
        {
            Task.Run(async () =>
#endif
        {
            // Create PayPal Transaction
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                PayPalAccount = new TransactionPayPalRequest()
                {
                    PaymentId = "fake-payment-id",
                    PayerId = "fake-payer-id"
                }
            };
            Result<Transaction> result = gateway.Transaction.Sale(request);
            Assert.IsTrue(result.IsSuccess());

            Transaction transaction = result.Target;

            // Create First Package with 2 products
            var lineItems = new TransactionLineItemRequest[2]
            {
                new TransactionLineItemRequest
                {
                    Quantity = 1,
                    Name = "Best Product Ever",
                    ProductCode = "ABC 01",
                    Description = "Best Description Ever"
                },
                new TransactionLineItemRequest
                {
                    Quantity = 1,
                    Name = "Best Product Ever",
                    ProductCode = "ABC 02",
                    Description = "Best Description Ever"
                },
            };
            var firstRequest = new PackageTrackingRequest
            {
                Carrier = "UPS",
                TrackingNumber = "tracking_number_1",
                NotifyPayer = false,
                LineItems = lineItems
            };

            // First package is shipped by the merchant
            Result<Transaction> firstPackageResult = gateway.Transaction.PackageTracking(transaction.Id, firstRequest);
            Assert.IsTrue(firstPackageResult.IsSuccess());

            Transaction txnWithFirstPackageTracking = firstPackageResult.Target;
            Assert.IsNotNull(txnWithFirstPackageTracking.Packages[0].Id);
            Assert.AreEqual(txnWithFirstPackageTracking.Packages[0].Carrier, "UPS");
            Assert.AreEqual(txnWithFirstPackageTracking.Packages[0].TrackingNumber, "tracking_number_1");

            // Create second package with 1 product
            var secondRequest = new PackageTrackingRequest
            {
                Carrier = "FEDEX",
                TrackingNumber = "tracking_number_2",
                NotifyPayer = false,
                LineItems = new TransactionLineItemRequest[]
                {
                    new TransactionLineItemRequest
                    {
                        Quantity = 1,
                        Name = "Best Product Ever",
                        ProductCode = "ABC 03",
                        Description = "Best Description Ever"
                    }
                }
            };

            // Second package is shipped by the merchant
            Result<Transaction> secondPackageResult = gateway.Transaction.PackageTracking(transaction.Id, secondRequest);
            Assert.IsTrue(secondPackageResult.IsSuccess());

            Transaction txnWithSecondPackageTracking = secondPackageResult.Target;
            Assert.IsNotNull(txnWithSecondPackageTracking.Packages[1].Id);
            Assert.AreEqual(txnWithSecondPackageTracking.Packages[1].Carrier, "FEDEX");
            Assert.AreEqual(txnWithSecondPackageTracking.Packages[1].TrackingNumber, "tracking_number_2");

            // Find transction gives both pckges
            Transaction findTransaction = gateway.Transaction.Find(transaction.Id);
            Assert.AreEqual(findTransaction.Packages.Length, 2);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

            [Test]
#if netcore
        public async Task PackageTrackingAsync_ValidationError()
#else
        public void PackageTrackingAsync_ValidationError()
            {
                Task.Run(async () =>
#endif
            {
                // Create PayPal Transaction
                TransactionRequest request = new TransactionRequest
                {
                    Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                    PayPalAccount = new TransactionPayPalRequest()
                    {
                        PaymentId = "fake-payment-id",
                        PayerId = "fake-payer-id"
                    }
                };
                Result<Transaction> result = gateway.Transaction.Sale(request);
                Assert.IsTrue(result.IsSuccess());

                Transaction transaction = result.Target;

                // Tracking Number is absent
                var invalidRequest = new PackageTrackingRequest
                {
                    Carrier = "UPS",
                    NotifyPayer = false,
                };

                Result<Transaction> invalidResult = gateway.Transaction.PackageTracking(transaction.Id, invalidRequest);
                Assert.IsFalse(invalidResult.IsSuccess());
                Assert.AreEqual("Tracking number is required.", invalidResult.Message);

                // Carrier is absent
                invalidRequest = new PackageTrackingRequest
                {
                    TrackingNumber = "tracking_number_1",
                };

                invalidResult = gateway.Transaction.PackageTracking(transaction.Id, invalidRequest);
                Assert.IsFalse(invalidResult.IsSuccess());
                Assert.AreEqual("Carrier name is required.", invalidResult.Message);
            }
#if net452
                ).GetAwaiter().GetResult();
            }
#endif
     }
}
