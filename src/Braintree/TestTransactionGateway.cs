#pragma warning disable 1591

using System.Xml;

namespace Braintree
{
    /// <summary>
    /// Provides test operations for settling, settlement confirming, settlement pending, and settlement declining for transactions in the sandbox vault
    /// </summary>
    public class TestTransactionGateway : ITestTransactionGateway
    {
        private readonly BraintreeService service;
        private readonly BraintreeGateway gateway;

        protected internal TestTransactionGateway(BraintreeGateway gateway)
        {
            gateway.Configuration.AssertHasAccessTokenOrKeys();
            this.gateway = gateway;
            service = new BraintreeService(gateway.Configuration);
        }

        private void CheckEnvironment()
        {
            if (gateway.Configuration.Environment == Environment.PRODUCTION)
            {
                throw new Braintree.Exceptions.TestOperationPerformedInProductionException("Test operation performed in production");
            }
        }

        public virtual Transaction Settle(string id)
        {
            CheckEnvironment();
            XmlNode response = service.Put(service.MerchantPath() + "/transactions/" + id + "/settle");
            return new Transaction(new NodeWrapper(response), gateway);
        }

        public virtual Transaction SettlementConfirm(string id)
        {
            CheckEnvironment();
            XmlNode response = service.Put(service.MerchantPath() + "/transactions/" + id + "/settlement_confirm");
            return new Transaction(new NodeWrapper(response), gateway);
        }

        public virtual Transaction SettlementPending(string id)
        {
            CheckEnvironment();
            XmlNode response = service.Put(service.MerchantPath() + "/transactions/" + id + "/settlement_pending");
            return new Transaction(new NodeWrapper(response), gateway);
        }

        public virtual Transaction SettlementDecline(string id)
        {
            CheckEnvironment();
            XmlNode response = service.Put(service.MerchantPath() + "/transactions/" + id + "/settlement_decline");
            return new Transaction(new NodeWrapper(response), gateway);
        }
    }
}
