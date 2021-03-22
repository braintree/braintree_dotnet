using Braintree.TestUtil;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Braintree.Tests.Integration
{
    [TestFixture()]
    public class DiscountIntegrationTest
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
        public void All_ReturnsAllDiscounts()
        {
            string discountId = string.Format("dotnet_discount{0}", new Random().Next(1000000).ToString());

            service.Post(service.MerchantPath() + "/modifications/create_modification_for_tests", new ModificationRequestForTests {
                Amount = 100M,
                Description = "a test discount",
                Id = discountId,
                Kind = "discount",
                Name = "discount_name",
                NeverExpires = false,
                NumberOfBillingCycles = 12
            });

            List<Discount> collection = gateway.Discount.All();
            Assert.IsNotEmpty(collection);

            Discount discount = collection.Find
            (
                d => d.Id == discountId
            );

            Assert.AreEqual(100M, discount.Amount);
            Assert.AreEqual("a test discount", discount.Description);
            Assert.AreEqual(discountId, discount.Id);
            Assert.AreEqual("discount", discount.Kind);
            Assert.AreEqual("discount_name", discount.Name);
            Assert.AreEqual(false, discount.NeverExpires);
            Assert.AreEqual(12, discount.NumberOfBillingCycles);
            Assert.IsNotNull(discount.CreatedAt);
            Assert.IsNotNull(discount.UpdatedAt);
        }

        [Test]
#if netcore
        public async Task AllAsync_ReturnsAllDiscounts()
#else
        public void AllAsync_ReturnsAllDiscounts()
        {
            Task.Run(async () =>
#endif
        {
            string discountId = string.Format("dotnet_discount{0}", new Random().Next(1000000).ToString());

            await service.PostAsync(service.MerchantPath() + "/modifications/create_modification_for_tests", new ModificationRequestForTests {
                Amount = 100M,
                Description = "a test discount",
                Id = discountId,
                Kind = "discount",
                Name = "discount_name",
                NeverExpires = false,
                NumberOfBillingCycles = 12
            });

            List<Discount> collection = await gateway.Discount.AllAsync();
            Assert.IsNotEmpty(collection);

            Discount discount = collection.Find
            (
                d => d.Id == discountId
            );

            Assert.AreEqual(100M, discount.Amount);
            Assert.AreEqual("a test discount", discount.Description);
            Assert.AreEqual(discountId, discount.Id);
            Assert.AreEqual("discount", discount.Kind);
            Assert.AreEqual("discount_name", discount.Name);
            Assert.AreEqual(false, discount.NeverExpires);
            Assert.AreEqual(12, discount.NumberOfBillingCycles);
            Assert.IsNotNull(discount.CreatedAt);
            Assert.IsNotNull(discount.UpdatedAt);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif
    }
}
